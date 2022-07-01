using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using VehicleHistoryASP.Models;

namespace VehicleHistoryASP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            using SqlConnection con = new SqlConnection("Data Source=HP-HUBERT;Initial Catalog=Vehicles;Integrated Security=True");
            using SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            
                cmd.CommandText = @"select id_car, make, model, license_plate, DATEDIFF(day, GETDATE(), term_tech_exam) as day_tech_exam, 
                                    DATEDIFF(day, GETDATE(), term_oc) as day_oc, DATEDIFF(day, GETDATE(), oil_date) as oil_day, 
                                    (oil_milage - actual_mileage) as oil_milage_left,DATEDIFF(day, GETDATE(), timing_date) as timing_day,   
                                    (timing_milage - actual_mileage) as timing_milage_left
                                    from cars
                                    where  DATEDIFF(day, GETDATE(), oil_date) < 30 OR DATEDIFF(day, GETDATE(), timing_date) < 30 OR 
                                    DATEDIFF(day, GETDATE(), term_tech_exam) < 30 OR DATEDIFF(day, GETDATE(), term_oc) < 30";
            

            con.Open();
            var dr = cmd.ExecuteReader();
            // ExecuteReader - dane zwrotne 
            // ExecuteNonQuery - brak danych zwrotnych insert, delete, update
            var carsList = new List<CarDetails>();
            while (dr.Read())
            {
                var car = new CarDetails
                {
                    IdCar = (int)dr["id_car"],
                    Make = dr["make"].ToString(),
                    Model = dr["model"].ToString(),
                    LicensePlate = dr["license_plate"].ToString(),
                    DayTechExam = dr["day_tech_exam"].ToString(),
                    DayOC = dr["day_oc"].ToString(),                   
                    OilDay = dr["oil_day"].ToString(),                  
                    OilMilageLeft = dr["oil_milage_left"].ToString(),                 
                    TimingDay = dr["timing_day"].ToString(),                  
                    TimingMilageLeft = dr["timing_milage_left"].ToString()
                };
                carsList.Add(car);
            }

            return View(carsList);
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
    }
}
