using praca.Entities;

public class Rental
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public string UserId { get; set; }
    public DateTime StartDate { get; set; }  // Data rozpoczęcia wynajmu
    public DateTime EndDate { get; set; }
    public bool IsReturned { get; set; } 

    public Car Car { get; set; }
    public ApplicationUser User { get; set; } 
}
