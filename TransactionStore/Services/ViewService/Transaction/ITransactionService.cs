using TransactionStore.Models.ServiceModel.Transaction;

namespace TransactionStore.Services.ViewService.Transaction
{
    public interface ITransactionService
    {
        UploadToDbResult UploadToDb(string fileExtension, Stream steamFileContent);
    }
}
