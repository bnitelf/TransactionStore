using System.Xml.Serialization;

namespace TransactionStore.Models.XmlModel.Transaction
{
    [XmlRoot("Transactions")]
    public class Transactions
    {
        [XmlElement("Transaction")]
        public List<Transaction> TransactionList { get; set; }
    }
}
