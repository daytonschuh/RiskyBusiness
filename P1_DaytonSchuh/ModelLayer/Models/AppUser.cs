using Microsoft.AspNetCore.Identity;
using ModelLayer.Models;
using System.ComponentModel.DataAnnotations;

namespace P1_DaytonSchuh
{
    public class AppUser : IdentityUser
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public byte[] ProfilePicture { get; set; }
        public int UsernameChangeLimit { get; set; } = 10;
        public int DefaultLocation { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
    }
}
