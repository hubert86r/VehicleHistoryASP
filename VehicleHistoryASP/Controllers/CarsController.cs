using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using VehicleHistoryASP.Models;

namespace VehicleHistoryASP.Controllers
{
    public class CarsController : Controller
    {
        public IActionResult Index()
        {
            using SqlConnection con = new SqlConnection("Data Source=HP-HUBERT;Initial Catalog=Vehicles;Integrated Security=True");
            using SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"select id_car, make, model, initial_mileage, term_tech_exam, term_oc, id_fuel_type
                                from cars";

            con.Open();
            var dr = cmd.ExecuteReader();
            // ExecuteReader - dane zwrotne 
            // ExecuteNonQuery - brak danych zwrotnych insert, delete, update
            var carsList = new List<Cars>();
            while (dr.Read())
            {
                var car = new Cars
                {
                    IdCar = (int)dr["id_car"],
                    Make = dr["make"].ToString(),
                    Model = dr["model"].ToString(),
                    InitialMileage = dr["initial_mileage"].ToString(),
                    term_tech_exam = (DateTime)dr["term_tech_exam"],
                    term_oc = (DateTime)dr["term_oc"],
                    IdFuelType = (int)dr["id_fuel_type"]
                };
                carsList.Add(car);
            }

            return View(carsList);
        }

        public IActionResult Details(int id)
        {
            using SqlConnection con = new SqlConnection("Data Source=HP-HUBERT;Initial Catalog=Vehicles;Integrated Security=True");
            using SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"select id_car, make, model, initial_mileage, term_tech_exam, term_oc, id_fuel_type
                                from cars where id_car ="+id;

            con.Open();
            var dr = cmd.ExecuteReader();
            // ExecuteReader - dane zwrotne 
            // ExecuteNonQuery - brak danych zwrotnych insert, delete, update
            var carsDetails = new List<Cars>();
            while (dr.Read())
            {
                var car = new Cars
                {
                    IdCar = (int)dr["id_car"],
                    Make = dr["make"].ToString(),
                    Model = dr["model"].ToString(),
                    InitialMileage = dr["initial_mileage"].ToString(),
                    term_tech_exam = (DateTime)dr["term_tech_exam"],
                    term_oc = (DateTime)dr["term_oc"],
                    IdFuelType = (int)dr["id_fuel_type"]
                };
                carsDetails.Add(car);
            }

            return View(carsDetails);
        }
    }

    
}
