using TransactionStore.Factory;
using TransactionStore.Models.ServiceModel.Transaction;
using TransactionStore.Models.ServiceModel.Validation;
using TransactionStore.Services.FileIO;
using TransactionStore.Services.Validation;

namespace TransactionStore.Services.ViewService.Transaction
{
    public class TransactionService : ITransactionService
    {
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(ILogger<TransactionService> logger)
        {
            _logger = logger;
        }

        public UploadToDbResult UploadToDb(string fileExtension, Stream steamFileContent)
        {
            UploadToDbResult uploadToDbResult = new UploadToDbResult();

            FileParserFactory fileParserFactory = new FileParserFactory();
            IFileParser fileParser = fileParserFactory.GetFileParser(fileExtension);
            List<InputTransaction> listInputTransactions = fileParser.Parse(steamFileContent);

            // Validate 
            ValidatorFactory validatorFactory = new ValidatorFactory();
            IValidator<InputTransaction> validator = validatorFactory.GetValidator(fileExtension);
            ValidateResult validateResult = validator.Validate(listInputTransactions);

            uploadToDbResult.IsValidationSuccess = validateResult.IsValid;
            uploadToDbResult.ValidateResult = validateResult;

            if (!validateResult.IsValid)
            {
                uploadToDbResult.IsSuccess = false;
                return uploadToDbResult;
            }

            // Convert to Db Model


            // Save to Db   


            uploadToDbResult.IsSuccess = true;

            return uploadToDbResult;
        }


    }
}
