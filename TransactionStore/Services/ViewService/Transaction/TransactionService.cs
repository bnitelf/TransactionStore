using TransactionStore.Factory;
using TransactionStore.Models.DBModel.TransactionStore;
using TransactionStore.Models.ServiceModel.Transaction;
using TransactionStore.Models.ServiceModel.Validation;
using TransactionStore.Services.FileIO;
using TransactionStore.Services.Validation;
using TransactionStore.Utils;

namespace TransactionStore.Services.ViewService.Transaction
{
    public class TransactionService : ITransactionService
    {
        private readonly ILogger<TransactionService> _logger;
        private readonly TransactionStoreEntities _transactionStoreEntities;

        public TransactionService(ILogger<TransactionService> logger
            , TransactionStoreEntities transactionStoreEntities)
        {
            _logger = logger;
            _transactionStoreEntities = transactionStoreEntities;
        }

        public UploadToDbResult UploadToDb(string fileExtension, Stream steamFileContent)
        {
            Guid guidRequestId = Guid.NewGuid();
            UploadToDbResult uploadToDbResult = new UploadToDbResult();
            uploadToDbResult.RequestId = guidRequestId.ToString();

            try
            {
                FileParserFactory fileParserFactory = new FileParserFactory();
                IFileParser fileParser = fileParserFactory.GetFileParser(fileExtension);
                List<InputTransaction> listInputTransactions = fileParser.Parse(steamFileContent);

                // Validate 
                ValidatorFactory validatorFactory = new ValidatorFactory();
                IValidator<InputTransaction> validator = validatorFactory.GetValidator(fileExtension);
                ValidateResult validateResult = validator.Validate(listInputTransactions);

                uploadToDbResult.IsValidationSuccess = validateResult.IsValid;
                uploadToDbResult.ValidateResult = validateResult;

                DateTime dtNow = DateTime.Now;

                if (!validateResult.IsValid)
                {
                    uploadToDbResult.IsSuccess = false;

                    // Log Validation Error in DB (for later support)
                    var paymentTransLog = new PaymentTransactionUploadLog();
                    paymentTransLog.RequestId = guidRequestId.ToString();
                    paymentTransLog.CreateDt = dtNow;
                    paymentTransLog.ErrorMessage = string.Join("\r\n", validateResult.ListRowErrors.Select(x => $"Row {x.Row}, Msg={x.ErrorMessage}"));
                    
                    _transactionStoreEntities.PaymentTransactionUploadLogs.Add(paymentTransLog);
                    _transactionStoreEntities.SaveChanges();

                    return uploadToDbResult;
                }


                // Convert to Db Model
                List<PaymentTransaction> listInputPaymentTrans = listInputTransactions.Select(x => new PaymentTransaction()
                {
                    TransactionId = x.TransactionId,
                    Amount = (decimal)x.Amount,
                    CurrencyCode = x.CurrencyCode,
                    TransactionDt = x.TransactionDate,
                    StatusRaw = x.Status,
                    Status = ConvertStatusRawToStatus(x.Status),
                    CreatedDt = dtNow,
                    UpdatedDt = dtNow
                }).ToList();

                // NOTE: For simplicity, I will use EF Core to do upsert operation.
                // In real world, we should use Bulk Upsert to improve performance.
                // as of current EF Core (2024-15-11), there is no built-in support for Bulk Upsert.
                // We need to write custom code or use 3rd party library like EFCore.BulkExtensions to do so.

                // Save to Db   
                using var transaction = _transactionStoreEntities.Database.BeginTransaction();

                try
                {
                    List<string> listTranIds = listInputTransactions.Select(x => x.TransactionId).ToList();

                    // Query for Update
                    List<PaymentTransaction> listPaymentTransExist = _transactionStoreEntities.PaymentTransactions.Where(x => listTranIds.Contains(x.TransactionId)).ToList();

                    if (listPaymentTransExist.Count > 0)
                    {
                        // Build dictionary for fast lookup
                        Dictionary<string, PaymentTransaction> dictPaymentTransUpdate = listInputPaymentTrans.ToDictionary(x => x.TransactionId);

                        foreach (PaymentTransaction paymentTransExist in listPaymentTransExist)
                        {
                            PaymentTransaction paymentTrans = dictPaymentTransUpdate[paymentTransExist.TransactionId];

                            paymentTransExist.Amount = paymentTrans.Amount;
                            paymentTransExist.CurrencyCode = paymentTrans.CurrencyCode;
                            paymentTransExist.TransactionDt = paymentTrans.TransactionDt;
                            paymentTransExist.StatusRaw = paymentTrans.StatusRaw;
                            paymentTransExist.Status = paymentTrans.Status;

                            paymentTransExist.UpdatedDt = dtNow;
                        }

                        _transactionStoreEntities.UpdateRange(listPaymentTransExist);
                    }

                    // Find new Transaction for Insert
                    List<string> listTranIdsExist = listPaymentTransExist.Select(x => x.TransactionId).ToList();
                    List<string> listTranIdsNew = listTranIds.Except(listTranIdsExist).ToList();

                    List<PaymentTransaction> listPaymentTransNew = listInputPaymentTrans.Where(x => listTranIdsNew.Contains(x.TransactionId)).ToList();
                    _transactionStoreEntities.AddRange(listPaymentTransNew);

                    _transactionStoreEntities.SaveChanges();

                    transaction.Commit();

                    uploadToDbResult.IsSuccess = true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while saving data to DB");

                    uploadToDbResult.IsSuccess = false;
                    transaction.Rollback();

                    LogExceptionToDb(guidRequestId.ToString(), ex);
                }

                return uploadToDbResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UploadToDb Error");
                uploadToDbResult.IsSuccess = false;

                LogExceptionToDb(guidRequestId.ToString(), ex);
                
                return uploadToDbResult;
            }
        }

        private string ConvertStatusRawToStatus(string statusRaw)
        {
            switch (statusRaw.ToUpper())
            {
                case "APPROVED":
                    return "A";

                case "FAILED":
                case "REJECTED":
                    return "R";

                case "FINISHED":
                case "DONE":
                    return "D";
                default:
                    return "";
            }
        }

        private void LogExceptionToDb(string requestId, Exception ex)
        {
            var paymentTransLog = new PaymentTransactionUploadLog();
            paymentTransLog.RequestId = requestId;
            paymentTransLog.CreateDt = DateTime.Now;
            paymentTransLog.ErrorMessage = DebugUtil.GetFullExceptionMessage(ex);

            _transactionStoreEntities.PaymentTransactionUploadLogs.Add(paymentTransLog);
            _transactionStoreEntities.SaveChanges();
        }
    }
}
