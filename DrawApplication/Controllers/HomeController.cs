using DrawApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DrawApplication.Controllers
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult Draw(ValueModel value)
        {
            var response = DrawWinners(value);
            ViewBag.Status = response.Status;
            ViewBag.WinnersNames = response.WinnersNames;
            return View("Index");
        }

        private ResponseValue DrawWinners(ValueModel value)
        {
            ResponseValue responseValue = new ResponseValue();
            var namesArray = value.Names.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList();
            Random random = new Random();
            List<string> winners = new List<string>();

            // Katýlýmcý sayýsýndan fazla kazanan olamaz
            if (namesArray.Count < value.NumberOfWinners)
            {
                responseValue.Status = "Katýlan sayýsýndan fazla kazanan olamaz.";
                return responseValue;
            }

            // Kazananlarý belirle
            for (int i = 0; i < value.NumberOfWinners; i++)
            {
                int randomIndex = random.Next(namesArray.Count);
                string selectedName = namesArray[randomIndex];
                winners.Add(selectedName);
                namesArray.RemoveAt(randomIndex); // Seçilen ismi listeden çýkar
            }

            responseValue.WinnersNames = winners;
            responseValue.Status = "Kazananlar belirlendi.";
            return responseValue;
        }
    }
}
