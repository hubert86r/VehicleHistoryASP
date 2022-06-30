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
        public string DayTechExam { get; set; }
        public string DayOC { get; set; }
        public string ActualMileage { get; set; }
        public DateTime UpdateDate { get; set; }
        public string FuelUsage { get; set; }
    }
}
