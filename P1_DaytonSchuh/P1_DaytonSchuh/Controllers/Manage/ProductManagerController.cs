using BusinessLogicLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelLayer.ViewModels;
using P1_DaytonSchuh.Data;
using P1_DaytonSchuh.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace P1_DaytonSchuh.Controllers
{
    public class ProductManagerController : Controller
    {
        private readonly ILogger<ProductManagerController> _logger;
        private BusinessLogicClass _businessLogicClass;

        public ProductManagerController(ApplicationDbContext context, BusinessLogicClass businessLogicClass, ILogger<ProductManagerController> logger)
        {
            _businessLogicClass = businessLogicClass;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View(_businessLogicClass.GetProducts());
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("AddProduct")]
        public ActionResult Create(AddProductViewModel addProductViewModel)
        {
            ProductViewModel pvm = _businessLogicClass.AddProduct(addProductViewModel);

            if (pvm != null)
            {
                // Return the details
                return View("DisplayProductDetails", pvm);
            }
            else
            {
                return View("ProductError");
            }
        }

        public ActionResult ProductDetails(int id)
        {
            ProductViewModel productViewModel = _businessLogicClass.EditProduct(id);
            return View("DisplayProductDetails", productViewModel);
        }

        // GET: ProductController/Edit/5
        [Route("ProductManager/EditProduct/{productId}")]
        public ActionResult EditProduct(int productId)
        {
            ProductViewModel productViewModel = _businessLogicClass.EditProduct(productId);
            return View(productViewModel);
        }

        [HttpPost]
        [ActionName("EditedProduct")]
        public ActionResult EditProduct(ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid) { return View("DisplayProductDetails", productViewModel); }
            // Get Profile Picture
            if (Request.Form.Files.Count > 0)
            {
                IFormFile file = Request.Form.Files.FirstOrDefault();
                using (var dataStream = new MemoryStream())
                {
                    file.CopyTo(dataStream);
                    productViewModel.ProductPicture = dataStream.ToArray();
                }
            }
            _businessLogicClass.EditedProduct(productViewModel);
            return RedirectToAction("Index");
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            ProductViewModel productViewModel = _businessLogicClass.EditProduct(id);
            return View(productViewModel);
        }

        public ActionResult DeleteProduct(int id)
        {
            _businessLogicClass.DeleteProduct(id);
            // Returns an updated view of the list without the deleted item
            List<ProductViewModel> pvmList = _businessLogicClass.GetProducts();
            return View("ProductsList", pvmList);
        }
    }
}
