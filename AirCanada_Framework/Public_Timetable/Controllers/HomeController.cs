using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Public_Timetable.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Rotativa.AspNetCore;

namespace Public_Timetable.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public ActionResult Index()
        {
            getFlights();

            return View();
        }

        //Generate pdf
        [HttpGet]
        public IActionResult PrintFlights()
        {
            var flightData = getFlightData();
            return new ViewAsPdf("Index", flightData);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //Get Flight data stored in JSON
        public List<Flight> getFlightData()
        {
            var model = new List<Flight>();
            string startupPath = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).FullName, "flights.json");
            if (!System.IO.File.Exists(startupPath))
            {
                return model;
            }
            var jsonValues = System.IO.File.ReadAllText(startupPath);
            var jsondata = JArray.Parse(jsonValues);

            foreach (var flight_data in jsondata)
            {
                var val_flight = new Flight();

                val_flight.flightno = flight_data["flightNo"].ToString();
                val_flight.PeriodOperation = flight_data["PeriodOfOperation"].ToString();
                val_flight.DaysOfOperation = flight_data["DaysofOperation"].ToString();
                val_flight.DepartureTime = flight_data["DepartureTime"].ToString();
                val_flight.OriginStation = flight_data["OriginSta"].ToString();
                val_flight.DestinationStation = flight_data["DestStation"].ToString();
                //val_flight.Aircraft = flight_data["Aircraft"].ToString();

                model.Add(val_flight);
            }

            return model;
        }

        //Build view 
        public ActionResult getFlights()
        {
            var model = getFlightData();

            return PartialView(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
