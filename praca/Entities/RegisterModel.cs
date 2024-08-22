using System.ComponentModel.DataAnnotations;

namespace praca.Entities
{
   
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string DriverLicenseNumber { get; set; }

        public DateTime MembershipDate { get; set; } = DateTime.Now;
    }
}