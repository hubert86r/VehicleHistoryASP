using System;

namespace VehicleHistoryASP.Models
{
    public class CarUpdate
    {
        public int initialMileage { get; set; }
       
        public DateTime termTechExam { get; set; }
      
        public DateTime termOC { get; set; }
    }
}
