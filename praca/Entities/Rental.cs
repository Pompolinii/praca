using praca.Entities;

public class Rental
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public string UserId { get; set; } 
    public DateTime RentedAt { get; set; }
    public DateTime? ReturnedAt { get; set; } 
    public bool IsReturned { get; set; } 

    public Car Car { get; set; }
    public ApplicationUser User { get; set; } 
}
