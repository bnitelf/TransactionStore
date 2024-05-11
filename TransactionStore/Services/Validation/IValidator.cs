using TransactionStore.Models.ServiceModel.Validation;

namespace TransactionStore.Services.Validation
{
    public interface IValidator<T>
    {
        ValidateResult Validate(List<T> listDatas);
    }
}
