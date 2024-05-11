using TransactionStore.Models.ServiceModel.Validation;

namespace TransactionStore.Models.ViewModel.Home
{
    public class UploadViewModel
    {
        public bool HasProcessed { get; set; }
        public bool IsSuccess { get; set; }
        public List<RowError> ListRowErrors { get; set; } = new List<RowError>();
    }
}
