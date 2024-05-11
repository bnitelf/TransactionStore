using CsvHelper.Configuration;
using TransactionStore.Models.ServiceModel.Transaction;

namespace TransactionStore.Models.CsvModel
{
    public class TransactionClassMap : ClassMap<InputTransaction>
    {
        public TransactionClassMap()
        {
            Map(m => m.TransactionId).Name("TransactionId");
            Map(m => m.Amount).Name("Amount");
            Map(m => m.CurrencyCode).Name("CurrencyCode");
            Map(m => m.TransactionDate).Name("TransactionDate").TypeConverterOption.Format("dd/MM/yyyy HH:mm:ss");
            Map(m => m.Status).Name("Status");
        }
    }
}
