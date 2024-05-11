using System.ComponentModel.DataAnnotations;
using TransactionStore.Models.ServiceModel.Validation;

namespace TransactionStore.Models.ServiceModel.Transaction
{
    public class UploadToDbResult
    {
        public bool IsSuccess { get; set; }
        public bool IsValidationSuccess { get; set; }
        public ValidateResult ValidateResult { get; set; }
    }
}
