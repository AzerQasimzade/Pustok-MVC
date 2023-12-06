using Microsoft.AspNetCore.Identity;
using PustokBookStore.Utilities.Enums;

namespace PustokBookStore.Models
{
    public class AppUser:IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public GenderHelper  Gender { get; set; }
    }
}
