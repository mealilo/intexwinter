﻿using intex2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            List<Accident> AllAccidents = repo.Accidents.Take(100).ToList();
            //AllAccidents = AllAccidents.Where(x => x.CRASH_DATETIME.ToString("yyyy") == "2019" && x.CRASH_DATETIME.ToString("M") == "12").ToList();
            ViewBag.accidents = AllAccidents;

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


        public IActionResult Index1()
        {
            List<Accident> AllAccidents = repo.Accidents.ToList();
            ViewBag.accidents = AllAccidents;
            return View();
        }
        // This is the view with all the edit buttons and delete. Only for logged in and authorized users
        [Authorize]
        public IActionResult AdminView()
        {

            List<Accident> AllAccidents = repo.Accidents.Take(100).ToList();
            ViewBag.accidents = AllAccidents;

            return View();
        }
        //Methods
        [Authorize]
        [HttpGet]
        public IActionResult AddEditAccident(int crashid)
        {

            ViewBag.Cities = repo.Accidents.Select(x => x.CITY).Distinct().ToList();
            ViewBag.Counties = repo.Accidents.Select(x => x.COUNTY_NAME).Distinct().ToList();
            // if new bowler
            if (crashid == 0)
            {
                return View();
            }

            else
            {
                Accident accident = repo.Accidents.Single(x => x.CRASH_ID == crashid);
                return View(accident);
            }

        }
        [Authorize]
        [HttpPost]
        public IActionResult AddEditAccident(Accident accident)
        {
        
            ViewBag.Cities = repo.Accidents.Select(x => x.CITY).Distinct().ToList();
            ViewBag.Counties = repo.Accidents.Select(x => x.COUNTY_NAME).Distinct().ToList();

            // if we are having issues with models and the db check here cause i removed the model check
            if (ModelState.ErrorCount <= 1)
            {
                repo.DoAccident(accident);
                return RedirectToAction("AdminView");
            }
          


            return View(accident);
            
        }

        //Delete
        [Authorize]
        [HttpPost]
        public IActionResult Delete(int crashid)
        {
            Accident accident = repo.Accidents.Single(x => x.CRASH_ID == crashid);
            repo.Delete(accident);

            return RedirectToAction("AdminView");
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

    }
}
