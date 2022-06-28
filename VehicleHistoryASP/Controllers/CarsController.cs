using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using VehicleHistoryASP.Models;

namespace VehicleHistoryASP.Controllers
{
    public class CarsController : Controller
    {
        public IActionResult Index(string search)
        {
            using SqlConnection con = new SqlConnection("Data Source=HP-HUBERT;Initial Catalog=Vehicles;Integrated Security=True");
            using SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            if (search == null)
            {
                cmd.CommandText = @"select id_car, make, model, license_plate
                                from cars";
            }
            else
            {
                cmd.CommandText = @"select id_car, make, model, license_plate
                                from cars
                                where license_plate = @search ";
                cmd.Parameters.AddWithValue("@search", search);
            }

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
                    LicensePlate = dr["license_plate"].ToString(),
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
            cmd.CommandText = @"select id_car, make, model, initial_mileage, term_tech_exam, term_oc, name from Cars c
join Fuel_Type ft ON c.id_fuel_type = ft.id_fuel_type where id_car =" + id;

            con.Open();
            var dr = cmd.ExecuteReader();
            // ExecuteReader - dane zwrotne 
            // ExecuteNonQuery - brak danych zwrotnych insert, delete, update
            var carsDetails = new List<CarDetails>();
            while (dr.Read())
            {
                var car = new CarDetails
                {
                    IdCar = (int)dr["id_car"],
                    Make = dr["make"].ToString(),
                    Model = dr["model"].ToString(),
                    InitialMileage = dr["initial_mileage"].ToString(),
                    TermTechExam = (DateTime)dr["term_tech_exam"],
                    TermOC = (DateTime)dr["term_oc"],
                    FuelType = dr["name"].ToString()
                };
                carsDetails.Add(car);
            }

            return View(carsDetails);
        }
        public IActionResult Caradd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Caradd(CarAdd newCar)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            using SqlConnection con = new SqlConnection("Data Source=HP-HUBERT;Initial Catalog=Vehicles;Integrated Security=True");
            using SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;

            cmd.CommandText = @"insert into Cars (make, model, initial_mileage, term_tech_exam, term_oc, id_fuel_type, license_plate) 
                                values (@make, @model, @initialMileage, @termTechExam, @termOC, @fuelType, @licensePlate)";

            cmd.Parameters.AddWithValue("@make", newCar.make);
            cmd.Parameters.AddWithValue("@model", newCar.model);
            cmd.Parameters.AddWithValue("@initialMileage", newCar.initialMileage);
            cmd.Parameters.AddWithValue("@termTechExam", newCar.termTechExam);
            cmd.Parameters.AddWithValue("@termOC", newCar.termOC);
            cmd.Parameters.AddWithValue("@fuelType", newCar.idFuelType);
            cmd.Parameters.AddWithValue("@licensePlate", newCar.licensePlate);

            con.Open(); 
            int rowsAffected=cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }
    }

    
}
