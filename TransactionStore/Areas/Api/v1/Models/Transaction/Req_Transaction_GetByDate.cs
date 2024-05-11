using System.ComponentModel.DataAnnotations;

namespace TransactionStore.Areas.Api.v1.Models.Transaction
{
    public class Req_Transaction_GetByDate
    {
        [Required]
        public DateTime? StartDate { get; set; }
        [Required]
        public DateTime? EndDate { get; set; }
    }
}
