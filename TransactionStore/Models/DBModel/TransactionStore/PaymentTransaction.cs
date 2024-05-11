using System;
using System.Collections.Generic;

namespace TransactionStore.Models.DBModel.TransactionStore
{
    public partial class PaymentTransaction
    {
        public long PaymentTransactionId { get; set; }
        public string TransactionId { get; set; } = null!;
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; } = null!;
        public DateTime TransactionDt { get; set; }
        public string StatusRaw { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime CreatedDt { get; set; }
        public DateTime UpdatedDt { get; set; }
    }
}
