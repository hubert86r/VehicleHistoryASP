using System;

namespace VehicleHistoryASP.Models
{
    public class Cars
    {
        //prop +2xtab
        public int IdCar { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string InitialMileage { get; set; }
        public DateTime term_tech_exam { get; set; }
        public DateTime term_oc { get; set; }
        public int IdFuelType { get; set; }
    }
}
