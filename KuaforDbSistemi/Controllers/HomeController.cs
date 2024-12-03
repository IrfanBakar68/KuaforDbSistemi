using System.Diagnostics;
using KuaforDbSistemi.Models;
using Microsoft.AspNetCore.Mvc;

namespace KuaforDbSistemi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        // Logger baðýmlýlýðý için yapýcý metot
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Ana sayfa
        public IActionResult Index()
        {
            _logger.LogInformation("Ana sayfa yüklendi.");
            return View();
        }

        // Gizlilik politikasý sayfasý
        public IActionResult Privacy()
        {
            _logger.LogInformation("Gizlilik politikasý sayfasý yüklendi.");
            return View();
        }

        // Hata sayfasý
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogError("Bir hata oluþtu.");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
