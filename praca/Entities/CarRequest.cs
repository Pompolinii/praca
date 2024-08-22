namespace praca.Entities
{
    public class CarRequest
    {
        public string Marka { get; set; } // Definiujemy właściwość Marka
        public string Model { get; set; } // Definiujemy właściwość Model
        public int Year { get; set; } // Definiujemy właściwość Year
        public IFormFile ImagePath { get; set; } // Definiujemy właściwość Image do obsługi pliku
    }
}
