using TransactionStore.Models.DBModel.TransactionStore;

namespace TransactionStore.Services.ApiService.Transaction
{
    public class TransactionApiService : ITransactionApiService
    {
        private readonly TransactionStoreEntities _transactionStoreEntities;

        public TransactionApiService(TransactionStoreEntities transactionStoreEntities)
        {
            _transactionStoreEntities = transactionStoreEntities;
        }

        public List<PaymentTransaction> GetTransactionsByCurrencyCode(string currencyCode)
        {
            List<PaymentTransaction> listPaymentTrans = _transactionStoreEntities.PaymentTransactions
                                                            .Where(x => x.CurrencyCode == currencyCode).ToList();
            return listPaymentTrans;
        }

        public List<PaymentTransaction> GetTransactionsByDateRange(DateTime startDate, DateTime endDate)
        {
            List<PaymentTransaction> listPaymentTrans = _transactionStoreEntities.PaymentTransactions
                                                            .Where(x => x.TransactionDt >= startDate && x.TransactionDt <= endDate).ToList();
            return listPaymentTrans;
        }

        public List<PaymentTransaction> GetTransactionsByStatus(string status)
        {
            List<PaymentTransaction> listPaymentTrans = _transactionStoreEntities.PaymentTransactions
                                                            .Where(x => x.Status == status).ToList();
            return listPaymentTrans;
        }
    }
}
