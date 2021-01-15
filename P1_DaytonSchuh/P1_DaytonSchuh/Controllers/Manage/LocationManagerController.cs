using BusinessLogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using P1_DaytonSchuh.Data;
using P1_DaytonSchuh.Models;
using System.Collections.Generic;

namespace P1_DaytonSchuh.Controllers
{
    public class LocationManagerController : Controller
    {
        private readonly ILogger<LocationManagerController> _logger;
        private BusinessLogicClass _businessLogicClass;

        public LocationManagerController(ApplicationDbContext context, BusinessLogicClass businessLogicClass, ILogger<LocationManagerController> logger)
        {
            _businessLogicClass = businessLogicClass;
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<Location> ll;
            ll = _businessLogicClass.GetAllLocations();
            return View(ll);
        }
        public ActionResult OrderHistory(int locationId)
        {
            List<Order> ol  = _businessLogicClass.GetOrderHistoryByLocation(locationId);
            return View(ol);
        }

        [Route("OrderHistory/Details")]
        public ActionResult Details(int locationId)
        {
            //List<ProductViewModel> pvm = _businessLogicClass.
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
    }
}
