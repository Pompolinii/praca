namespace praca.Entities
{
    public class TopUpRequest
    {
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public string BlikCode { get; set; }
    }
}
