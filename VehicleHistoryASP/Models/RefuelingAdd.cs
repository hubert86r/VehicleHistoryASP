using System;
using System.ComponentModel.DataAnnotations;

namespace VehicleHistoryASP.Models
{
    public class RefuelingAdd
    {
        
        public int idCar { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane")]
        [RegularExpression(@"^[0-9]{1,3}(\.+[0-9]{0,2}){0,1}$")]
        public string fuelQquantity { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane")]
        [RegularExpression(@"^[0-9]{1,2}(\.+[0-9]{0,2}){0,1}$")]
        public string price { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane")]
        public DateTime date { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane")]
        public int milage { get; set; }

    }
}
