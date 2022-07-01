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
        public DateTime OilDate { get; set; }
        public string OilDay { get; set; }
        public string OilMilage { get; set; }
        public string OilMilageLeft { get; set; }
        public DateTime TimingDate { get; set; }
        public string TimingDay { get; set; }
        public string TimingMilage { get; set; }
        public string TimingMilageLeft { get; set; }
        public string LicensePlate { get; set; }
    }
}
