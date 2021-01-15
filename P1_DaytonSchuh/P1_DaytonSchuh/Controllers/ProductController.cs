
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using BusinessLogicLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelLayer.Models;
using ModelLayer.ViewModels;
using P1_DaytonSchuh.Data;
using P1_DaytonSchuh.Models;

namespace P1_DaytonSchuh.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private BusinessLogicClass _businessLogicClass;

        public ProductController(ApplicationDbContext context, BusinessLogicClass businessLogicClass, ILogger<ProductController> logger)
        {
            _businessLogicClass = businessLogicClass;
            _logger = logger;
        }

        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// dont touch this
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="defLoc"></param>
        /// <returns></returns>
        [ActionName("Search")]
        public ActionResult ProductsList(string searchString, int defLoc)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            if (claimsIdentity == null || defLoc == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            // Create Instance of List<ProductViewModel>
            List<ProductViewModel> productViewModel;

            // Select all products filtered by searchstring
            //int? defLoc = HttpContext.Session.GetInt32("_DefaultLocation");
            if (searchString != null)
            {
                productViewModel = _businessLogicClass.GetProducts(searchString, defLoc);
            }
            else
            {
                productViewModel = _businessLogicClass.GetProducts(null, defLoc);
            }

            // Return the view
            return View("ProductsList", productViewModel);
        }

        //[ActionName("Search")]
        public ActionResult ProductDetails(int id)
        {
            ProductViewModel productViewModel = _businessLogicClass.EditProduct(id);
            return View("DisplayProductDetails", productViewModel);
        }

        public ActionResult ProductsList()
        {
            List<ProductViewModel> pvmList = _businessLogicClass.GetProducts();
            return View(pvmList);
        }

        [Route("Product/AddToCart/{id}")]
        public ActionResult AddToOrder(int id)
        {
            ProductViewModel productViewModel = _businessLogicClass.EditProduct(id);
            return View(productViewModel);
        }

        [HttpPost]
        [ActionName("AddedToCart")]
        public ActionResult AddToOrder(ProductViewModel productViewModel)
        {
            //_businessLogicClass.AddedToCart(productViewModel, number);
            int? defLoc = HttpContext.Session.GetInt32("_DefaultLocation");
            string? userId = HttpContext.Session.GetString("_Id");
            ShoppingCart cart = _businessLogicClass.GetCartNA(userId);
            if (productViewModel.Quantity > 0)
            {
                _businessLogicClass.AddedToCart(cart, productViewModel);
            }

            //return View(cartViewModel);
            return RedirectToAction("Search", "Product", defLoc);
        }
    }
}
