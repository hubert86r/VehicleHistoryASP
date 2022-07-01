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
            cmd.CommandText = @"select id_car, make, model, initial_mileage, term_tech_exam, term_oc, name, license_plate, 
		                        DATEDIFF(day, GETDATE(), term_tech_exam) as day_tech_exam, DATEDIFF(day, GETDATE(), term_oc) as day_oc,
								actual_mileage, update_date, fuel_usage, 
                                oil_date, DATEDIFF(day, GETDATE(), oil_date) as oil_day, oil_milage, (oil_milage - actual_mileage) as oil_milage_left, 
                                timing_date, DATEDIFF(day, GETDATE(), timing_date) as timing_day, timing_milage, (timing_milage - actual_mileage) as timing_milage_left
								from Cars c
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
                    LicensePlate = dr["license_plate"].ToString(),
                    DayTechExam = dr["day_tech_exam"].ToString(),
                    DayOC = dr["day_oc"].ToString(),
                    UpdateDate = (DateTime)dr["update_date"],
                    ActualMileage = dr["actual_mileage"].ToString(),
                    FuelUsage = dr["fuel_usage"].ToString(),
                    OilDate = (DateTime)dr["oil_date"],
                    OilDay = dr["oil_day"].ToString(),
                    OilMilage = dr["oil_milage"].ToString(),
                    OilMilageLeft = dr["oil_milage_left"].ToString(),
                    TimingDate = (DateTime)dr["timing_date"],
                    TimingDay = dr["timing_day"].ToString(),
                    TimingMilage = dr["timing_milage"].ToString(),
                    TimingMilageLeft = dr["timing_milage_left"].ToString()
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

            cmd.CommandText = @"insert into Cars (make, model, initial_mileage, term_tech_exam, term_oc, id_fuel_type, license_plate, actual_mileage, update_date, oil_date, timing_date, oil_milage, timing_milage) 
                                values (@make, @model, @initialMileage, @termTechExam, @termOC, @fuelType, @licensePlate, @initialMileage, GETDATE(), DATEADD(YEAR, 1, GETDATE()), DATEADD(YEAR, 5, GETDATE()), (@initialMileage + 15000), (@initialMileage + 80000))";

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

            return RedirectToAction("details", new { id = id });
        }
        public IActionResult Refuelinghistory(int id)
        {
            using SqlConnection con = new SqlConnection("Data Source=HP-HUBERT;Initial Catalog=Vehicles;Integrated Security=True");
            using SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"select id_car, fuel_quantity, price, date, milage, ROUND((price*fuel_quantity),2) as cost, fuel_usage from Refueling_History
                                where id_car =" + id+"order by date desc";

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
                    milage = (int)dr["milage"],
                    cost = dr["cost"].ToString(),
                    fuelUsage = dr["fuel_usage"].ToString()
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

            return RedirectToAction("details", new { id = id });
        }
        public IActionResult Servicehistory(int id)
        {
            using SqlConnection con = new SqlConnection("Data Source=HP-HUBERT;Initial Catalog=Vehicles;Integrated Security=True");
            using SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"select name, Dscription, date, milage, price from Service_History sh
                                join Service_Type st ON sh.id_type = st.id_type
                                where id_car =" + id + "order by date desc";

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
        public IActionResult Carupdate(int id)
        {
            return View();
        }
        [HttpPost]
        public IActionResult Carupdate(CarUpdate CarUpdate, int id)
        {
            

            using SqlConnection con = new SqlConnection("Data Source=HP-HUBERT;Initial Catalog=Vehicles;Integrated Security=True");
            using SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;

            if (CarUpdate.initialMileage != 0 && CarUpdate.termTechExam.ToString() == "01.01.0001 00:00:00" && CarUpdate.termOC.ToString() == "01.01.0001 00:00:00")
            {
                cmd.CommandText = @"update Cars set actual_mileage =@milage where id_car =" + id;
                cmd.Parameters.AddWithValue("@milage", CarUpdate.initialMileage);
                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
            }
            else if (CarUpdate.initialMileage == 0 && CarUpdate.termTechExam.ToString() != "01.01.0001 00:00:00" && CarUpdate.termOC.ToString() == "01.01.0001 00:00:00")
            {
                cmd.CommandText = @"update Cars set term_tech_exam =@term_tech_exam where id_car =" + id;
                cmd.Parameters.AddWithValue("@term_tech_exam", CarUpdate.termTechExam);
                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
            }
            else if (CarUpdate.initialMileage == 0 && CarUpdate.termTechExam.ToString() == "01.01.0001 00:00:00" && CarUpdate.termOC.ToString() != "01.01.0001 00:00:00")
            {
                cmd.CommandText = @"update Cars set term_oc =@termOC where id_car =" + id;
                cmd.Parameters.AddWithValue("@termOC", CarUpdate.termOC);
                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
            }
            else if (CarUpdate.initialMileage != 0 && CarUpdate.termTechExam.ToString() != "01.01.0001 00:00:00" && CarUpdate.termOC.ToString() == "01.01.0001 00:00:00")
            {
                cmd.CommandText = @"update Cars set initial_mileage = @milage, term_tech_exam = @term_tech_exam where id_car =" + id;
                cmd.Parameters.AddWithValue("@milage", CarUpdate.initialMileage);
                cmd.Parameters.AddWithValue("@term_tech_exam", CarUpdate.termTechExam);
                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
            }
            else if (CarUpdate.initialMileage != 0 && CarUpdate.termTechExam.ToString() == "01.01.0001 00:00:00" && CarUpdate.termOC.ToString() != "01.01.0001 00:00:00")
            {
                cmd.CommandText = @"update Cars set initial_mileage = @milage, term_oc = @termOC where id_car =" + id;
                cmd.Parameters.AddWithValue("@milage", CarUpdate.initialMileage);
                cmd.Parameters.AddWithValue("@termOC", CarUpdate.termOC);
                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
            }
            else if (CarUpdate.initialMileage == 0 && CarUpdate.termTechExam.ToString() != "01.01.0001 00:00:00" && CarUpdate.termOC.ToString() != "01.01.0001 00:00:00")
            {
                cmd.CommandText = @"update Cars set term_tech_exam = @term_tech_exam, term_oc = @termOC where id_car =" + id;
                cmd.Parameters.AddWithValue("@term_tech_exam", CarUpdate.termTechExam);
                cmd.Parameters.AddWithValue("@termOC", CarUpdate.termOC);
                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
            }
            else
            {
                cmd.CommandText = @"update Cars set initial_mileage = @milage, term_tech_exam = @term_tech_exam, term_oc = @termOC where id_car =" + id;
                cmd.Parameters.AddWithValue("@termOC", CarUpdate.termOC);
                cmd.Parameters.AddWithValue("@term_tech_exam", CarUpdate.termTechExam);
                cmd.Parameters.AddWithValue("@milage", CarUpdate.initialMileage);
                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
            }


            


            return RedirectToAction("details", new { id = id });
        }
    }

    
}
