using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using TransactionStore.Areas.Api.v1.Models.Transaction;
using TransactionStore.Models.DBModel.TransactionStore;
using TransactionStore.Services.ApiService.Transaction;
using TransactionStore.Utils;

namespace TransactionStore.Areas.Api.v1.Controllers.Transaction
{
    [Area("api")]
    public class TransactionController : Controller
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly ITransactionApiService _transactionApiService;

        public TransactionController(ILogger<TransactionController> logger
            , ITransactionApiService transactionApiService) 
        {
            _logger = logger;
            _transactionApiService = transactionApiService;
        }


        // GET: api/v1/Transaction/GetByCurrency
        [HttpGet]
        public async Task<IActionResult> GetByCurrency([FromQuery] Req_Transaction_GetByCurrency req)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(req.CurrencyCode) && !ValidationUtil.IsValidCurrencyCode(req.CurrencyCode))
                {
                    ModelState.AddModelError("CurrencyCode", "Invalid currency code.");
                }

                if (!ModelState.IsValid) return BadRequest(ModelState);

                // For testing unhandle exception purpose
                //throw new Exception("test");

                List<PaymentTransaction> listPaymentTransactions = _transactionApiService.GetTransactionsByCurrencyCode(req.CurrencyCode);

                List<Resp_Transaction> listRespTrans = listPaymentTransactions.Select(x => new Resp_Transaction()
                {
                    Id = x.TransactionId,
                    Payment = $"{x.Amount.ToString("F2")} {x.CurrencyCode}",
                    Status = x.Status
                }).ToList();

                return Ok(listRespTrans);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error GetByCurrency");
                return StatusCode(500, new { Error = "Error occurred internally. Please contact administrator for support." });
            }
        }

        // GET: api/v1/Transaction/GetByDate
        [HttpGet]
        public async Task<IActionResult> GetByDate([FromQuery] Req_Transaction_GetByDate req)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);


                List<PaymentTransaction> listPaymentTransactions = _transactionApiService.GetTransactionsByDateRange(req.StartDate!.Value, req.EndDate!.Value);

                List<Resp_Transaction> listRespTrans = listPaymentTransactions.Select(x => new Resp_Transaction()
                {
                    Id = x.TransactionId,
                    Payment = $"{x.Amount.ToString("F2")} {x.CurrencyCode}",
                    Status = x.Status
                }).ToList();

                return Ok(listRespTrans);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error GetByDate");
                return StatusCode(500, new { Error = "Error occurred internally. Please contact administrator for support." });
            }
        }

        // GET: api/v1/Transaction/GetByStatus
        [HttpGet]
        public async Task<IActionResult> GetByStatus([FromQuery] Req_Transaction_GetByStatus req)
        {
            try
            {
                List<string> listValidStatus = new List<string>() { "A", "R", "D" };
                if (!string.IsNullOrWhiteSpace(req.Status) && !listValidStatus.Contains(req.Status))
                {
                    ModelState.AddModelError("Status", "Invalid status.");
                }

                if (!ModelState.IsValid) return BadRequest(ModelState);


                List<PaymentTransaction> listPaymentTransactions = _transactionApiService.GetTransactionsByStatus(req.Status);

                List<Resp_Transaction> listRespTrans = listPaymentTransactions.Select(x => new Resp_Transaction()
                {
                    Id = x.TransactionId,
                    Payment = $"{x.Amount.ToString("F2")} {x.CurrencyCode}",
                    Status = x.Status
                }).ToList();

                return Ok(listRespTrans);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error GetByCurrency");
                return StatusCode(500, new { Error = "Error occurred internally. Please contact administrator for support." });
            }
        }
    }
}
