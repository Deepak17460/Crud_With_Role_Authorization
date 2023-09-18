using System.ComponentModel.DataAnnotations;

namespace RoleBaseAuthorization.Models.Authentication
{
    public class UserRegister
    {
        [Required(ErrorMessage = "User Name is required")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
