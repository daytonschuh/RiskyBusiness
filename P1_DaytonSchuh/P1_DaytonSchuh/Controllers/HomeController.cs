using BusinessLogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using ModelLayer.ViewModels;
using P1_DaytonSchuh.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace P1_DaytonSchuh.Controllers
{
    public class HomeController : Controller
    {
        private readonly BusinessLogicClass _businessLogicClass;
        private readonly ILogger<HomeController> _logger;

        public HomeController(BusinessLogicClass businessLogicClass, ILogger<HomeController> logger)
        {
            _businessLogicClass = businessLogicClass;
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<LocationLineViewModel> hotDeals = _businessLogicClass.GetHotDeals();
            return View(hotDeals);
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
