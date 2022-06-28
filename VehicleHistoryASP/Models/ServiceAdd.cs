using System;
using System.ComponentModel.DataAnnotations;

namespace VehicleHistoryASP.Models
{
    public class ServiceAdd
    {
        public int idCar { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane")]
        public int idType { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane")]
        public string description { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane")]
        public DateTime date { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane")]
        public int milage { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane")]
        public float price { get; set; }
    }
}
