using praca.Entities;

public class Rental
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public string UserId { get; set; } // Używamy stringa dla UserId
    public DateTime RentedAt { get; set; }
    public DateTime? ReturnedAt { get; set; } // Może być null, jeśli auto nie zostało jeszcze zwrócone
    public bool IsReturned { get; set; } // Dodajemy właściwość IsReturned

    public Car Car { get; set; }
    public ApplicationUser User { get; set; } // Używamy ApplicationUser jako modelu użytkownika
}
