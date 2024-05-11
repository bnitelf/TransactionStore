using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Globalization;
using TransactionStore.Models.CsvModel;
using TransactionStore.Models.ServiceModel.Transaction;

namespace TransactionStore.Services.FileIO
{
    public class CsvFileParser : IFileParser
    {
        public List<InputTransaction> Parse(Stream content)
        {
            using (var reader = new StreamReader(content))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<TransactionClassMap>();
                List<InputTransaction> listInputTransactions = csv.GetRecords<InputTransaction>().ToList();
                return listInputTransactions;
            }
        }
    }
}
