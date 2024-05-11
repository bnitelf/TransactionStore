using System.ComponentModel.DataAnnotations;

namespace TransactionStore.Areas.Api.v1.Models.Transaction
{
    public class Req_Transaction_GetByStatus
    {
        [Required]
        public string Status { get; set; } = "";
    }
}
