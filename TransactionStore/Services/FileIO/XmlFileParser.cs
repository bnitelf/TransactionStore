using System.Xml;
using System.Xml.Serialization;
using TransactionStore.Models.ServiceModel.Transaction;
using TransactionStore.Models.XmlModel.Transaction;

namespace TransactionStore.Services.FileIO
{
    public class XmlFileParser : IFileParser
    {
        public List<InputTransaction> Parse(Stream content)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Transactions));
            using (StreamReader reader = new StreamReader(content))
            {
                Transactions transactions = (Transactions)serializer.Deserialize(reader);

                // Convert XML data to InputTransaction
                List<InputTransaction> listInputTransactions = new List<InputTransaction>();
                foreach (var transaction in transactions.TransactionList)
                {
                    listInputTransactions.Add(new InputTransaction
                    {
                        TransactionId = transaction.Id,
                        Amount = (double)transaction.PaymentDetails.Amount,
                        CurrencyCode = transaction.PaymentDetails.CurrencyCode,
                        TransactionDate = transaction.TransactionDate,
                        Status = transaction.Status
                    });
                }
                return listInputTransactions;
            }
        }
    }
}
