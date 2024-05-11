using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using System.Xml;
using TransactionStore.Models.ServiceModel.Transaction;
using TransactionStore.Models.ServiceModel.Validation;
using TransactionStore.Models.ViewModel;
using TransactionStore.Models.ViewModel.Home;
using TransactionStore.Services.ViewService.Transaction;

namespace TransactionStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITransactionService _transactionService;

        public HomeController(ILogger<HomeController> logger
            , ITransactionService transactionService)
        {
            _logger = logger;
            _transactionService = transactionService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upload()
        {
            var uploadViewModel = new UploadViewModel();
            return View(uploadViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FileUpload(IFormFile? transactionFile)
        {
            var uploadViewModel = new UploadViewModel();

            // Validate 
            if (transactionFile == null || transactionFile.Length == 0)
            {
                ModelState.AddModelError("", "Please select file. (support csv, xml)");
                return View("Upload", uploadViewModel);
            }

            List<string> listAllowExtensions = new List<string>() { ".csv", ".xml" };
            string uploadedfileExtension = Path.GetExtension(transactionFile.FileName).ToLower();


            if (string.IsNullOrEmpty(uploadedfileExtension))
            {
                ModelState.AddModelError("", "Unknown format.");
            }
            else if (!listAllowExtensions.Contains(uploadedfileExtension))
            {
                ModelState.AddModelError("", "Invalid file type. (support csv, xml)");
            }

            int allowedFileSize = 1_048_576; // 1MB
            if (transactionFile.Length > allowedFileSize)
            {
                ModelState.AddModelError("", "The file is too large. (support up to 1MB)");
            }

            if (!ModelState.IsValid)
                return View("Upload", uploadViewModel);

            
            //var sb = new StringBuilder();
            //using var streamReader = new StreamReader();
            //sb.AppendLine(streamReader.ReadToEnd());

            Stream fileContent = transactionFile.OpenReadStream();

            UploadToDbResult result = _transactionService.UploadToDb(uploadedfileExtension, fileContent);

            uploadViewModel.HasProcessed = true;
            uploadViewModel.RequestId = result.RequestId;
            uploadViewModel.IsSuccess = result.IsSuccess;
            uploadViewModel.IsValidationSuccess = result.IsValidationSuccess;
            uploadViewModel.ListRowErrors = result.ValidateResult.ListRowErrors;

            return View("Upload", uploadViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
