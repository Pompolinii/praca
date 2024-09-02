using System.ComponentModel.DataAnnotations;

namespace praca.Entities
{

    public class RegisterModel
    {
        [Required(ErrorMessage = "Email jest wymagany.")]
        [EmailAddress(ErrorMessage = "Niepoprawny format email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Hasło musi mieć przynajmniej 8 znaków.")]
        [MaxLength(100, ErrorMessage = "Hasło może mieć maksymalnie 100 znaków.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "Hasło musi zawierać przynajmniej jedną małą literę, jedną wielką literę oraz jedną cyfrę.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Pełna nazwa jest wymagana.")]
        [MinLength(5, ErrorMessage = "Pełna nazwa musi mieć przynajmniej 5 znaków.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Adres jest wymagany.")]
        [MinLength(5, ErrorMessage = "Adres musi mieć przynajmniej 5 znaków.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Data urodzenia jest wymagana.")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(RegisterModel), nameof(ValidateDateOfBirth))]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Numer prawa jazdy jest wymagany.")]
        [RegularExpression(@"^[A-Z0-9]{5,15}$", ErrorMessage = "Numer prawa jazdy może zawierać tylko wielkie litery oraz cyfry, i musi mieć długość od 5 do 15 znaków.")]

        public string DriverLicenseNumber { get; set; }

        public DateTime MembershipDate { get; set; } = DateTime.Now;

        // Metoda do walidacji daty urodzenia
        public static ValidationResult ValidateDateOfBirth(DateTime dateOfBirth, ValidationContext context)
        {
            int minimumAge = 18;
            if (dateOfBirth > DateTime.Now.AddYears(-minimumAge))
            {
                return new ValidationResult($"Użytkownik musi mieć przynajmniej {minimumAge} lat.");
            }

            return ValidationResult.Success;
        }
    }
}