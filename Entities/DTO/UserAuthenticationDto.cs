using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.DTO;
public class UserAuthenticationDto
{
    [Required(ErrorMessage = "User name is required")]
    public string Username {get; set;}

    [Required(ErrorMessage = "User password is required")]
    public string Password {get; set;}
}
