using System;

namespace VehicleHistoryASP.Models
{
    public class ServiceHistory
    {      
        public string type { get; set; }     
        public string description { get; set; }
        public DateTime date { get; set; }
        public int milage { get; set; }
        public string price { get; set; }
    }
}
