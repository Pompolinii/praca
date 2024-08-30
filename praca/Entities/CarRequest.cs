namespace praca.Entities
{
    public class CarRequest
    {
        public string Marka { get; set; } 
        public string Model { get; set; } 
        public int Year { get; set; } 
        public IFormFile ImagePath { get; set; } 
    }
}
