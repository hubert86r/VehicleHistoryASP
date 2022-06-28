using System;

namespace VehicleHistoryASP.Models
{
    public class CarDetails
    {
        public int IdCar { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string InitialMileage { get; set; }
        public DateTime TermTechExam { get; set; }
        public DateTime TermOC { get; set; }
        public string FuelType { get; set; }
    }
}
