using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P1_DaytonSchuh.Controllers
{
    public class LocationLineController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
