﻿using intex2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace intex2.Controllers
{
    public class HomeController : Controller
    {

        //creates repo for the DB
        private IAccidents repo { get; set; }

        public HomeController(IAccidents temp)
        {
            repo = temp;
        }

        public IActionResult Index()
        {
            return View();
        }


        //loads all accidents, enables filtering etc
        public IActionResult Accidents()
        {

            List<Accident> AllAccidents = repo.Accidents.ToList();

            return View();
        }

        public IActionResult About()
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
