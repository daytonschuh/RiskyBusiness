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
using System.Security.Claims;
using System.Threading.Tasks;

namespace P1_DaytonSchuh.Controllers.Manage
{
    public class UserManagerController : Controller
    {
        private readonly ILogger<UserManagerController> _logger;
        private BusinessLogicClass _businessLogicClass;

        public UserManagerController(ApplicationDbContext context, BusinessLogicClass businessLogicClass, ILogger<UserManagerController> logger)
        {
            _businessLogicClass = businessLogicClass;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View(_businessLogicClass.GetUsers());
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("AddUser")]
        public ActionResult Create(AddUserViewModel addUserViewModel)
        {
            UserViewModel pvm = _businessLogicClass.AddUser(addUserViewModel);

            if (pvm != null)
            {
                // Return the details
                return View("DisplayUserDetails", pvm);
            }
            else
            {
                return View("UserError");
            }
        }

        [ActionName("Search")]
        public ActionResult UsersList(string searchString)
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            if (claimsIdentity == null)
            {
                return RedirectToAction("Index", "Home");
            }
            // Create Instance of List<ProductViewModel>
            List<UserViewModel> userViewModel;

            // Select all products filtered by searchstring
            if (searchString != null)
            {
                userViewModel = _businessLogicClass.GetUsers(searchString);
            }
            else
            {
                userViewModel = _businessLogicClass.GetUsers();
            }

            // Return the view
            return View("UsersList", userViewModel);
        }

        // GET: UserController/Edit/5
        [Route("UserManager/EditUser/{Id}")]
        public ActionResult EditUser(string id)
        {
            UserViewModel userViewModel = _businessLogicClass.EditUser(id);
            return View(userViewModel);
        }

        [HttpPost]
        [ActionName("EditedUser")]
        public async Task<ActionResult> EditUser(UserViewModel userViewModel)
        {
            if (!ModelState.IsValid) { return View("DisplayUserDetails", userViewModel); }
            // Get Profile Picture
            if (Request.Form.Files.Count > 0)
            {
                IFormFile file = Request.Form.Files.FirstOrDefault();
                using (var dataStream = new MemoryStream())
                {
                    file.CopyTo(dataStream);
                    userViewModel.UserPicture = dataStream.ToArray();
                }
            }
            await _businessLogicClass.EditedUser(userViewModel);
            return RedirectToAction("Index");
        }

        public ActionResult UserDetails(string id)
        {
            UserViewModel userViewModel = _businessLogicClass.EditUser(id);
            return View("DisplayUserDetails", userViewModel);
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(string id)
        {
            UserViewModel UserViewModel = _businessLogicClass.EditUser(id);
            return View(UserViewModel);
        }

        public ActionResult DeleteUser(string id)
        {
            _businessLogicClass.DeleteUser(id);
            // Returns an updated view of the list without the deleted item
            List<UserViewModel> pvmList = _businessLogicClass.GetUsers();
            return View("UsersList", pvmList);
        }
    }
}