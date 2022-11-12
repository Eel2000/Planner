using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Planner.core.DTOs;
#nullable disable
public class UserDTO
{
    [Required(ErrorMessage ="Must specified the username"),
        EmailAddress(ErrorMessage ="The username must be a email address.")]
    public string Username { get; set; }

    [Required(ErrorMessage ="Must typ the password"), PasswordPropertyText]
    public string Password { get; set; }
}
