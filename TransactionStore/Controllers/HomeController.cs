using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using System.Xml;
using TransactionStore.Models.ViewModel;
using TransactionStore.Models.ViewModel.Home;

namespace TransactionStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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

            if (!listAllowExtensions.Contains(uploadedfileExtension))
            {
                ModelState.AddModelError("", "Invalid file type. (support csv, xml)");
            }

            int allowedFileSize = 2_097_152; // 2MB
            if (transactionFile.Length > allowedFileSize)
            {
                ModelState.AddModelError("", "The file is too large. (support up to 2MB)");
            }

            if (!ModelState.IsValid)
                return View("Upload", uploadViewModel);

            
            var sb = new StringBuilder();
            using var streamReader = new StreamReader(transactionFile.OpenReadStream());
            sb.AppendLine(streamReader.ReadToEnd());

            uploadViewModel.HasProcessed = true;
            uploadViewModel.IsSuccess = false;
            uploadViewModel.ListRowErrors = new List<RowError>();
            uploadViewModel.ListRowErrors.Add(new RowError { Row = 1, ErrorMessage = "Invalid data" });
            uploadViewModel.ListRowErrors.Add(new RowError { Row = 2, ErrorMessage = "Invalid data" });

            return View("Upload", uploadViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
