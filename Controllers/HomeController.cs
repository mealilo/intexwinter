using intex2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
//using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using Microsoft.EntityFrameworkCore;
using static intex2.Models.AuxiliaryModels.DataTableModels;
using jQueryDatatableServerSideNetCore.Extensions;


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
            var allAccident = repo.Accidents.Take(1000);
            var jsonarray = new List<string>();
            foreach (Accident a in allAccident)
            {
                string json = JsonConvert.SerializeObject(a, Formatting.Indented);
                jsonarray = jsonarray.Append(json).ToList();
            }

            //System.IO.File.WriteAllText(@"\Users\intext.json", JsonConvert.SerializeObject(jsonarray));
            Console.WriteLine(jsonarray);
            ; return View();
        }


        //loads all accidents, enables filtering etc
        public IActionResult Accidents()
        {

            List<Accident> AllAccidents = repo.Accidents.ToList();
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

        public IActionResult AdminView()
        {

            List<Accident> AllAccidents = repo.Accidents.ToList();
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


            // if we are having issues with models and the db check here cause i removed the model check.
            repo.DoAccident(accident);

            return RedirectToAction("AdminView");

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

        [HttpGet] public IActionResult testtables()
        {
            return View();
        }
      [HttpPost]
        public async Task<ActionResult> AjaxHandler([FromBody] DtParameters dtParameters)
        {
           
            var searchBy = dtParameters.Search?.Value;

            // if we have an empty search then just order the results by Id ascending
            var orderCriteria = "CRASH_ID";
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }

            var result = repo.Accidents.AsQueryable();

            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.CRASH_ID.ToString() != null && r.CRASH_ID.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.CRASH_DATETIME.ToString() != null && r.CRASH_DATETIME.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.CRASH_SEVERITY_ID.ToString() != null && r.CRASH_SEVERITY_ID.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.ROUTE.ToString() != null && r.ROUTE.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.MILEPOINT.ToString() != null && r.MILEPOINT.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.LAT_UTM_Y.ToString() != null && r.LAT_UTM_Y.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.LONG_UTM_X.ToString() != null && r.LONG_UTM_X.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.MAIN_ROAD_NAME != null && r.MAIN_ROAD_NAME.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.CITY != null && r.CITY.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.COUNTY_NAME != null && r.COUNTY_NAME.ToUpper().Contains(searchBy.ToUpper())
                                           );
            }

            result = orderAscendingDirection ? result.OrderByDynamic(orderCriteria, DtOrderDir.Asc) : result.OrderByDynamic(orderCriteria, DtOrderDir.Desc);

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = await result.CountAsync();
            var totalResultsCount = await repo.Accidents.CountAsync();

            return Json(new DtResult<Accident>
            {
                Draw = dtParameters.Draw,
                RecordsTotal = totalResultsCount,
                RecordsFiltered = filteredResultsCount,
                Data = await result
                    .Skip(dtParameters.Start)
                    .Take(dtParameters.Length)
                    .ToListAsync()
            });
        }

      

    }
    }


//CRASH_ID </ td >
//CRASH_DATETIME </ td >
//ROUTE </ td >
//MILEPOINT </ td >
//LATITUDE </ td >
//LONGITUDE </ td >
//MAIN_ROAD_NAME </ td >
//CITY </ td >
//COUNTY_NAME </ td >
//PEDESTRIAN_INVOLVED </ td >
//WORK_ZONE_RELATED </ td >
//BICYCLIST_INVOLVED </ td >
//MOTORCYCLE_INVOLVED </ td >
//IMPROPER_RESTRAINT </ td >
//UNRESTRAINED </ td >
//CRASH_SEVERITY_ID </ td >
//DUI </ td >
//INTERSECTION_RELATED </ td >
//WILD_ANIMAL_RELATED </ td >
//DOMESTIC_ANIMAL_RELATED </ td >
//OVERTURN_ROLLOVER </ td >
//COMMERCIAL_MOTOR_VEH_INVOLVED