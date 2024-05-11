namespace TransactionStore.Models.ServiceModel.Transaction
{
    public class InputTransaction
    {
        public string TransactionId { get; set; } = "";
        public double Amount { get; set; }
        public string CurrencyCode { get; set; } = "";
        public DateTime TransactionDate { get; set; }
        public string Status { get; set; } = "";
    }
}
