using System.ComponentModel.DataAnnotations;

namespace TransactionStore.Areas.Api.v1.Models.Transaction
{
    public class Req_Transaction_GetByCurrency
    {
        [Required]
        public string CurrencyCode { get; set; } = "";
    }
}
