using System;
using System.ComponentModel.DataAnnotations;

namespace VehicleHistoryASP.Models
{
    public class CarAdd
    {
        [Required(ErrorMessage ="Pole jest wymagane")]
        public string make { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane")]
        public string model { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane")]
        public int initialMileage { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane")]
        public DateTime termTechExam { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane")]
        public DateTime termOC { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane")]
        public int idFuelType { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane")]
        public string licensePlate { get; set; }
    }
}
