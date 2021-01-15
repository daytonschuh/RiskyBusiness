using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using P1_DaytonSchuh.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P1_DaytonSchuh.Controllers
{
    public class ManageSiteController : Controller
    {
        private readonly ILogger<ManageSiteController> _logger;
        private readonly ApplicationDbContext _dbContext;
        public ManageSiteController(ApplicationDbContext context, ILogger<ManageSiteController> logger)
        {
            _dbContext = context;
            _logger = logger;
        }
        public ActionResult Index()
        {
            return View();
        }

        /*[HttpPost]
        public async Task<IActionResult> AddRole(string roleName)
        {
            if (roleName != null)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
            }
            return RedirectToAction("Index");
        }*/
    }
}
