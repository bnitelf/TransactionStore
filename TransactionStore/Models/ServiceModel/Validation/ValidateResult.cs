namespace TransactionStore.Models.ServiceModel.Validation
{
    public class ValidateResult
    {
        public bool IsValid { get; set; }
        public List<RowError> ListRowErrors { get; set; } = new List<RowError>();
    }
}
