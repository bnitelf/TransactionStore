using TransactionStore.Models.ServiceModel.Transaction;
using TransactionStore.Models.ServiceModel.Validation;
using TransactionStore.Utils;

namespace TransactionStore.Services.Validation
{
    public class CsvTransactionValidator : IValidator<InputTransaction>
    {
        public ValidateResult Validate(List<InputTransaction> listDatas)
        {
            ValidateResult result = new ValidateResult();
            result.IsValid = true;

            int row;
            InputTransaction data;
            string message;
            bool isValid;
            List<string> listMessageCols = new List<string>();

            for (int i = 0; i < listDatas.Count; i++)
            {
                data = listDatas[i];

                row = i + 1;
                isValid = true;
                listMessageCols.Clear();

                if (!ValidateTransactionId(data, out message))
                {
                    isValid = false;
                    listMessageCols.Add(message);
                }
                if (!ValidateTransactionDate(data, out message))
                {
                    isValid = false;
                    listMessageCols.Add(message);
                }
                if (!ValidateAmount(data, out message))
                {
                    isValid = false;
                    listMessageCols.Add(message);
                }
                if (!ValidateCurrencyCode(data, out message))
                {
                    isValid = false;
                    listMessageCols.Add(message);
                }
                if (!ValidateStatus(data, out message))
                {
                    isValid = false;
                    listMessageCols.Add(message);
                }

                if (!isValid)
                {
                    result.IsValid = false;
                    result.ListRowErrors.Add(new RowError()
                    {
                        Row = row,
                        ErrorMessage = string.Join(" | ", listMessageCols)
                    });
                }
            }

            return result;
        }

        private bool ValidateTransactionId(InputTransaction data, out string message)
        {
            message = "";
            if (string.IsNullOrEmpty(data.TransactionId))
            {
                message = "TransactionId is required.";
                return false;
            }
            if (data.TransactionId.Length > 50)
            {
                message = "TransactionId is too long. (max 50 characters)";
                return false;
            }
            return true;
        }

        private bool ValidateTransactionDate(InputTransaction data, out string message)
        {
            message = "";
            if (data.TransactionDate == null)
            {
                message = "TransactionDate is required.";
                return false;
            }
            return true;
        }

        private bool ValidateAmount(InputTransaction data, out string message)
        {
            message = "";
            if (data.Amount <= 0)
            {
                message = "Amount must be greater than 0.";
                return false;
            }
            return true;
        }

        private bool ValidateCurrencyCode(InputTransaction data, out string message)
        {
            message = "";
            if (string.IsNullOrEmpty(data.CurrencyCode))
            {
                message = "CurrencyCode is required.";
                return false;
            }
            if (!ValidationUtil.IsValidCurrencyCode(data.CurrencyCode))
            {
                message = "CurrencyCode is not valid.";
                return false;
            }
            return true;
        }

        private bool ValidateStatus(InputTransaction data, out string message)
        {
            message = "";
            if (string.IsNullOrEmpty(data.Status))
            {
                message = "Status is required.";
                return false;
            }
            List<string> listValids = new List<string>() { "Approved", "Failed", "Finished" };
            if (!listValids.Contains(data.Status, StringComparer.OrdinalIgnoreCase))
            {
                string strValidStatus = string.Join(", ", listValids);
                message = $"Status is not valid. (expected {strValidStatus})";
                return false;
            }
            return true;
        }
    }
}
