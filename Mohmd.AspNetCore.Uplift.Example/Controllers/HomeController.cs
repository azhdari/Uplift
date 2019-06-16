using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Mohmd.AspNetCore.Uplift.Example.Models;

namespace Mohmd.AspNetCore.Uplift.Example.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
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

        public IActionResult CreateBook()
        {
            return View(new BookModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateBook(BookModel model)
        {
            if (ModelState.IsValid)
            {
                // do something
            }

            return View(model);
        }
    }
}
