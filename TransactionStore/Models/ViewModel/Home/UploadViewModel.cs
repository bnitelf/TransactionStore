using TransactionStore.Models.ServiceModel.Validation;

namespace TransactionStore.Models.ViewModel.Home
{
    public class UploadViewModel
    {
        public string RequestId { get; set; } = "";
        public bool HasProcessed { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsValidationSuccess { get; set; }
        public List<RowError> ListRowErrors { get; set; } = new List<RowError>();
    }
}
