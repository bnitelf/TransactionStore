using TransactionStore.Models.ServiceModel.Transaction;

namespace TransactionStore.Services.FileIO
{
    public interface IFileParser
    {
        List<InputTransaction> Parse(Stream content);

    }
}
