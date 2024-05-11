using TransactionStore.Models.ServiceModel.Transaction;
using TransactionStore.Services.Validation;

namespace TransactionStore.Factory
{
    public class ValidatorFactory
    {
        public IValidator<InputTransaction> GetValidator(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".csv":
                    return new CsvTransactionValidator();
                case ".xml":
                    return new XmlTransactionValidator();
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
