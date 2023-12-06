using PustokBookStore.Utilities.Enums;
using System.ComponentModel.DataAnnotations;

namespace PustokBookStore.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "UserName Or Email is required,Please Input UserName Or Email")]
        public string UserNameOrEmail { get; set; }
        [Required(ErrorMessage = "Password is required,Please Input Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IsRemembered { get; set; }
    }
}
