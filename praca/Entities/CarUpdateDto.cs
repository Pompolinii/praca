﻿public class CarUpdateDto
{
    public string Marka { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public bool IsRented { get; set; }
    public IFormFile ImagePath { get; set; } // Obsługa pliku obrazu
}