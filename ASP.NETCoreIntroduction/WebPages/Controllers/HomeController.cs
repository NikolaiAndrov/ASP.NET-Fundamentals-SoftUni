﻿namespace WebPages.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
    using WebPages.Models;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.Message = "Hello World!"; 
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "This is an ASP.NET Core MVC app.";
            return View();
        }

        public IActionResult Numbers()
        {
            return View();
        }

        public IActionResult NumbersToN(int count = 0)
        {
            ViewBag.Count = count;
            return View();
        }


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}