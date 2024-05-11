using TransactionStore.Models.DBModel.TransactionStore;

namespace TransactionStore.Services.ApiService.Transaction
{
    public interface ITransactionApiService
    {
        List<PaymentTransaction> GetTransactionsByCurrencyCode(string currencyCode);
        List<PaymentTransaction> GetTransactionsByDateRange(DateTime startDate, DateTime endDate);
        List<PaymentTransaction> GetTransactionsByStatus(string status);
    }
}
