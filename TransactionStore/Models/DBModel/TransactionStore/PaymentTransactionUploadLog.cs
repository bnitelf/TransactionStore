using System;
using System.Collections.Generic;

namespace TransactionStore.Models.DBModel.TransactionStore
{
    public partial class PaymentTransactionUploadLog
    {
        public long Id { get; set; }
        public string RequestId { get; set; } = null!;
        public DateTime CreateDt { get; set; }
        public string ErrorMessage { get; set; } = null!;
    }
}
