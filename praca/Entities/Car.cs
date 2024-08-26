using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace praca.Entities
{
    public class Car
    {
        public int Id { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public bool IsRented { get; set; }
        public string ImagePath { get; set; }  
        public string Transmission { get; set; }

        public float Enginesize   {get; set; }

        public int Price { get; set; }

     
    }
}
