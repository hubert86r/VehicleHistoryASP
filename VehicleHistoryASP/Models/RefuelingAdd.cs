using System;
using System.ComponentModel.DataAnnotations;

namespace VehicleHistoryASP.Models
{
    public class RefuelingAdd
    {
        
        public int idCar { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane")]
        public float fuelQquantity { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane")]
        public float price { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane")]
        public DateTime date { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane")]
        public int milage { get; set; }

    }
}
