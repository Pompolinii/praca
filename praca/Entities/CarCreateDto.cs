public class CarCreateDto
{
    public string Marka { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public bool IsRented { get; set; }
    public IFormFile ImagePath { get; set; }

    public string Transmission { get; set; }

    public float Enginesize { get; set; }

    public int Price { get; set; }
}