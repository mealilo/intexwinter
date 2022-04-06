using intex2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.ML.Transforms.Onnx;
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
        private InferenceSession _session;


        public HomeController(IAccidents temp, InferenceSession session)
        {
            repo = temp;
            _session = session;

        }



        public IActionResult Index()
        {
            return View();
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

        [HttpPost]
        public IActionResult AddEditAccident(Accident accident)
        {
            if (ModelState.IsValid)
            {
                repo.DoAccident(accident);

                return RedirectToAction("Index");
            }

            return View(accident);
        }

        //Delete
        [HttpPost]
        public IActionResult Delete(int crashid)
        {
            Accident accident = repo.Accidents.Single(x => x.CRASH_ID == crashid);
            repo.Delete(accident);

            return RedirectToAction("AdminView");
        }

        //Onnx
        [HttpGet]
        public IActionResult Score()
        {
            return View();
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

    }
}
