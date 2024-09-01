namespace praca.Entities
{
    public class RentRequest
    { 
        public int CarId { get; set; }
        public string UserId { get; set; }

        public DateTime StartDate { get; set; } // Data rozpoczęcia wynajmu
        public DateTime EndDate { get; set; }   // Data zakończenia wynajmu
    }
}
