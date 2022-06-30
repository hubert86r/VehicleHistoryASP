using System;

namespace VehicleHistoryASP.Models
{
    public class RefuelingHistory
    {
        public int idCar { get; set; }
        
        public string fuelQquantity { get; set; }
        
        public string price { get; set; }
        
        public DateTime date { get; set; }
        
        public int milage { get; set; }

        public string cost { get; set; }

        public string fuelUsage { get; set; }
    }
}
