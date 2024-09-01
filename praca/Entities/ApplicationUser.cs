using Microsoft.AspNetCore.Identity;
using System;

namespace praca.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string DriverLicenseNumber { get; set; }
        public DateTime MembershipDate { get; set; }
        public decimal Balance { get; set; }
    }
}