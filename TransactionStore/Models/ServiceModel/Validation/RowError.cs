namespace TransactionStore.Models.ServiceModel.Validation
{
    public class RowError
    {
        public int Row { get; set; }
        public string ErrorMessage { get; set; } = "";
    }
}
