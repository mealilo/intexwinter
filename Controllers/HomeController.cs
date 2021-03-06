using intex2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.ML.Transforms.Onnx;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
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

        //Creates session for onnx file to be called
        private InferenceSession _session;


        public HomeController(IAccidents temp, InferenceSession session)
        {
            repo = temp;
            _session = session;

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

        //search. This is our backup if we can't get datatables working

        public IActionResult Search()
        {
            return View();
        }
        //[HttpPost]
        //public IActionResult Search(IFormCollection form)
        //{


        //    string date = form["date"];


        //    DateTime dateD = DateTime.Parse(date); 


        //    List<Accident> FilteredAccidents = repo.Accidents.Where(x => x.CRASH_DATETIME.Value.Date == dateD).ToList();
        //    ViewBag.FilteredAccidents = FilteredAccidents;
        //    return View();
        //}
        // This is the view with all the edit buttons and delete. Only for logged in and authorized users
        [Authorize]
        public IActionResult AdminView()
        {

            List<Accident> AllAccidents = repo.Accidents.OrderByDescending(d => d.CRASH_DATETIME).Take(100).ToList();
            ViewBag.accidents = AllAccidents;

            return View();
        }
        //Methods
        [Authorize]
        [HttpGet]
        public IActionResult AddEditAccident(int crashid)
        {

            ViewBag.Cities = repo.Accidents.OrderBy(x => x.CITY).Select(x => x.CITY).Distinct().ToList();

            ViewBag.Counties = repo.Accidents.OrderBy(x => x.COUNTY_NAME).Select(x => x.COUNTY_NAME).Distinct().ToList();
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
        
            var city = repo.Accidents.OrderBy(x => x.CITY).Select(x => x.CITY).Distinct().ToList();


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

        //Onnx Crash Severity Score
        [HttpGet]
        public IActionResult Score(int? crashid)
        {
            if (crashid != null)
            {
            Accident accident = repo.Accidents.Single(x => x.CRASH_ID == crashid);
            CrashSeverityData data = new CrashSeverityData();
            data.route = Convert.ToSingle(accident.ROUTE);
            data.milepoint = Convert.ToSingle(accident.ROUTE);
            data.lat_utm_y = Convert.ToSingle(accident.LAT_UTM_Y);
            data.long_utm_x = Convert.ToSingle(accident.LONG_UTM_X);


                if (accident.PEDESTRIAN_INVOLVED == "TRUE")
                {
                    data.pedestrian_involved = 1;
                }
                else
                {
                    data.pedestrian_involved = 0;
                }

                if (accident.MOTORCYCLE_INVOLVED == "TRUE")
                {
                    data.motorcycle_involved = 1;
                }
                else
                {
                    data.motorcycle_involved = 0;
                }

                if (accident.INTERSECTION_RELATED == "TRUE")
                {
                    data.intersection_related = 1;
                }
                else
                {
                    data.intersection_related = 0;
                }

                if (accident.OVERTURN_ROLLOVER == "TRUE")
                {
                    data.overturn_rollover = 1;
                }
                else
                {
                    data.overturn_rollover = 0;
                }

                if (accident.OLDER_DRIVER_INVOLVED == "TRUE")
                {
                    data.older_driver_involved = 1;
                }
                else
                {
                    data.older_driver_involved = 0;
                }

                if (accident.NIGHT_DARK_CONDITION == "TRUE")
                {
                    data.night_dark_condition = 1;
                }
                else
                {
                    data.night_dark_condition = 0;
                }

                if (accident.DISTRACTED_DRIVING == "TRUE")
                {
                    data.distracted_driving = 1;
                }
                else
                {
                    data.distracted_driving = 0;
                }

                var result = _session.Run(new List<NamedOnnxValue>
                {
                    NamedOnnxValue.CreateFromTensor("float_input", data.AsTensor())
                });
                    Tensor<float> score = result.First().AsTensor<float>();
                    var prediction = new Prediction { PredictedValue = score.First() };
                    result.Dispose();
                    ViewBag.Message = "Predicted Score: " + Math.Round(prediction.PredictedValue, 0);

                return View(data);
            }
            else
            {
                return View();
            }


        }

        [HttpPost]
        public IActionResult Score(CrashSeverityData data)
        {
            var result = _session.Run(new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("float_input", data.AsTensor())
            });
            Tensor<float> score = result.First().AsTensor<float>();
            var prediction = new Prediction { PredictedValue = score.First() };
            result.Dispose();

            ViewBag.Message = "Predicted Score: " + Math.Round(prediction.PredictedValue,0);
            return View(data);
        }

        //Onnx Survey Prediction
        [HttpGet]
        public IActionResult Survey()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Survey(CrashSeverityData survey)
        {
            Random rd = new Random();

            float rand_num = rd.Next(1, 1000000);
            survey.route = rand_num;

            rand_num = rd.Next(0, 500);
            survey.milepoint = rand_num;

            rand_num = rd.Next(4000000, 5000000);
            survey.lat_utm_y = rand_num;

            rand_num = rd.Next(200000, 700000);
            survey.long_utm_x = rand_num;

            var result = _session.Run(new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("float_input", survey.AsTensor())
            });
            Tensor<float> surveyscore = result.First().AsTensor<float>();
            var surveyprediction = new Prediction { PredictedValue = surveyscore.First() };
            result.Dispose();
            ViewBag.Message = "Predicted Score: " + Math.Round(surveyprediction.PredictedValue, 0);
            return View(survey);
        }

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