using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using State.Models;
using System.Diagnostics;

namespace State.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Counter _counter;
        public HomeController(ILogger<HomeController> logger, Counter counter)
        {
            _logger = logger;
            _counter = counter;
        }

        public IActionResult Index()
        {
            var sessionVariabeleAantalBezoeken = HttpContext.Session.GetInt32("aantalBezoeken");
            if (sessionVariabeleAantalBezoeken == null)
                sessionVariabeleAantalBezoeken = 1;
            else
                sessionVariabeleAantalBezoeken += 1;
            HttpContext.Session.SetInt32("aantalBezoeken", (int)sessionVariabeleAantalBezoeken);
            ViewBag.aantalBezoeken = (int)sessionVariabeleAantalBezoeken;
            var sessionVariabeleBezoeken = HttpContext.Session.GetString("visits");
            List<DateTime>? lijstBezoeken;
            if (string.IsNullOrEmpty(sessionVariabeleBezoeken))
                lijstBezoeken = new();
            else
                lijstBezoeken = JsonConvert.DeserializeObject<List<DateTime>>(sessionVariabeleBezoeken);
            lijstBezoeken?.Add(DateTime.Now);
            var geserializeerdeLijst = JsonConvert.SerializeObject(lijstBezoeken);
            HttpContext.Session.SetString("visits", geserializeerdeLijst);
            ViewBag.lastvisits = lijstBezoeken;
            _counter.TotaalAantalBezoeken += 1;
            ViewBag.totaalAantalBezoeken = _counter.TotaalAantalBezoeken;
            return View();
        }

        public IActionResult Wissen()
        {
            // is er een cookie met de naam "lastvisit"?
            HttpContext.Session.Clear();
            _counter.TotaalAantalBezoeken = 0;
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
    }
}