namespace TransactionStore.Models.XmlModel.Transaction
{
    public class PaymentDetails
    {
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; } = "";
    }
}
