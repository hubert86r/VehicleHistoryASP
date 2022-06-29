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
            cmd.CommandText = @"select id_car, make, model, initial_mileage, term_tech_exam, term_oc, name, 
		                        DATEDIFF(day, GETDATE(), term_tech_exam) as day_tech_exam, DATEDIFF(day, GETDATE(), term_oc) as day_oc  from Cars c
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
                    FuelType = dr["name"].ToString(),
                    DayTechExam = dr["day_tech_exam"].ToString(),
                    DayOC = dr["day_oc"].ToString()
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
        public IActionResult Refuelingadd(int id)
        {
            return View();
        }
        [HttpPost]
        public IActionResult Refuelingadd(RefuelingAdd newRefueling,int id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            using SqlConnection con = new SqlConnection("Data Source=HP-HUBERT;Initial Catalog=Vehicles;Integrated Security=True");
            using SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;

            cmd.CommandText = @"insert into Refueling_History (id_car, fuel_quantity, price, date, milage) 
                                values (@idCar, @fuelQuantity, @price, @date, @milage)";

            cmd.Parameters.AddWithValue("@idCar", id);
            cmd.Parameters.AddWithValue("@fuelQuantity", newRefueling.fuelQquantity);
            cmd.Parameters.AddWithValue("@price", newRefueling.price);
            cmd.Parameters.AddWithValue("@date", newRefueling.date);
            cmd.Parameters.AddWithValue("@milage", newRefueling.milage);

            con.Open();
            int rowsAffected = cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }
        public IActionResult Refuelinghistory(int id)
        {
            using SqlConnection con = new SqlConnection("Data Source=HP-HUBERT;Initial Catalog=Vehicles;Integrated Security=True");
            using SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"select id_car, fuel_quantity, price, date, milage from Refueling_History
                                where id_car =" + id;

            con.Open();
            var dr = cmd.ExecuteReader();
            // ExecuteReader - dane zwrotne 
            // ExecuteNonQuery - brak danych zwrotnych insert, delete, update
            var RefuelingHistory = new List<RefuelingHistory>();
            while (dr.Read())
            {
                var refueling = new RefuelingHistory
                {
                    idCar = (int)dr["id_car"],
                    fuelQquantity = dr["fuel_quantity"].ToString(),
                    price = dr["price"].ToString(),
                    date = (DateTime)dr["date"],
                    milage = (int)dr["milage"]
                };
                RefuelingHistory.Add(refueling);
            }

            return View(RefuelingHistory);
        }
        public IActionResult Serviceadd(int id)
        {
            return View();
        }
        [HttpPost]
        public IActionResult Serviceadd(ServiceAdd newService, int id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            using SqlConnection con = new SqlConnection("Data Source=HP-HUBERT;Initial Catalog=Vehicles;Integrated Security=True");
            using SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;

            cmd.CommandText = @"insert into Service_History (id_car, id_type, Dscription, date, milage, price ) 
                                values (@idCar, @idType, @description, @date, @milage, @price)";

            cmd.Parameters.AddWithValue("@idCar", id);
            cmd.Parameters.AddWithValue("@idType", newService.idType);
            cmd.Parameters.AddWithValue("@description", newService.description);
            cmd.Parameters.AddWithValue("@date", newService.date);
            cmd.Parameters.AddWithValue("@milage", newService.milage);
            cmd.Parameters.AddWithValue("@price", newService.price);

            con.Open();
            int rowsAffected = cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }
        public IActionResult Servicehistory(int id)
        {
            using SqlConnection con = new SqlConnection("Data Source=HP-HUBERT;Initial Catalog=Vehicles;Integrated Security=True");
            using SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"select name, Dscription, date, milage, price from Service_History sh
                                join Service_Type st ON sh.id_type = st.id_type
                                where id_car =" + id;

            con.Open();
            var dr = cmd.ExecuteReader();
            // ExecuteReader - dane zwrotne 
            // ExecuteNonQuery - brak danych zwrotnych insert, delete, update
            var ServiceHistory = new List<ServiceHistory>();
            while (dr.Read())
            {
                var service = new ServiceHistory
                {
                    type = dr["name"].ToString(),
                    description = dr["Dscription"].ToString(),
                    date = (DateTime)dr["date"],
                    milage = (int)dr["milage"],
                    price = dr["price"].ToString()
                };
                ServiceHistory.Add(service);
            }

            return View(ServiceHistory);
        }
    }

    
}
