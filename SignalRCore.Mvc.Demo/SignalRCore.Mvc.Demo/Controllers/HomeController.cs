using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalRCore.Mvc.Demo.Hubs;
using SignalRCore.Mvc.Demo.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SignalRCore.Mvc.Demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHubContext<WeatherHub> weatherHub;

        public HomeController(ILogger<HomeController> logger, IHubContext<WeatherHub> weatherHub)
        {
            _logger = logger;
            this.weatherHub = weatherHub;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            await weatherHub.Clients.All.SendAsync("Broadcast", $"Privacy page visited at: {DateTime.Now}");

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
