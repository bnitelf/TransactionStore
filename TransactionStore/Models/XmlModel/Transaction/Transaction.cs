using System.Xml.Serialization;

namespace TransactionStore.Models.XmlModel.Transaction
{
    public class Transaction
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = "";

        [XmlIgnore] // Ignore original property
        public DateTime TransactionDate { get; set; }

        [XmlElement("TransactionDate")]
        public string TransactionDateString // New property to handle serialization/deserialization
        {
            get { return TransactionDate.ToString("yyyy-MM-ddThh:mm:ss"); }
            set { TransactionDate = DateTime.ParseExact(value, "yyyy-MM-ddTHH:mm:ss", null); }
        }

        public PaymentDetails PaymentDetails { get; set; }

        public string Status { get; set; } = "";
    }
}
