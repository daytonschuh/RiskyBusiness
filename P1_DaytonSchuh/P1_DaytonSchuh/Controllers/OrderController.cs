using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ModelLayer.ViewModels;
using BusinessLogicLayer;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using P1_DaytonSchuh.Models;
using ModelLayer.Models;

namespace P1_DaytonSchuh.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;
        private readonly MapperClass _mapperClass;
        private BusinessLogicClass _businessLogicClass;

        public OrderController(MapperClass mapperClass, BusinessLogicClass businessLogicClass, ILogger<OrderController> logger)
        {
            _mapperClass = mapperClass;
            _businessLogicClass = businessLogicClass;
            _logger = logger;
        }
        public IActionResult Index()
        {
            // Create Instance of List<ProductViewModel>
            List<CartItem> cartViewModel = _businessLogicClass.GetCartViewModel(HttpContext.Session.GetInt32("_Cart"));

            List<ProductViewModel> pvm = _mapperClass.ConvertCartItemsToProductViewModel(cartViewModel);
            // Return the view
            return View(pvm);
        }

        public ActionResult Checkout()
        {
            List<CartItem> cartViewModel = _businessLogicClass.GetCartViewModel(HttpContext.Session.GetInt32("_Cart"));

            List<ProductViewModel> pvm = _mapperClass.ConvertCartItemsToProductViewModel(cartViewModel);
            return View(pvm);
        }

        public ActionResult Checkedout()
        {
            int? defLoc = HttpContext.Session.GetInt32("_DefaultLocation");
            string? userId = HttpContext.Session.GetString("_Id");
            int? cartId = HttpContext.Session.GetInt32("_Cart");
            _businessLogicClass.HandleOrder(defLoc, userId, cartId);
            return View();
        }

        public ActionResult History()
        {
            List<OrderViewModel> ovm;
            var s = HttpContext.Session.GetString("_Id");
            ovm = _businessLogicClass.GetUserOrderHistory(s);
            return View(ovm);
        }

        public ActionResult Details(int id)
        {
            List<ProductViewModel> pvm;
            var s = HttpContext.Session.GetString("_Id");
            pvm = _businessLogicClass.GetSpecificOrderHistory(s, id);
            return View(pvm);
        }
    }
}
