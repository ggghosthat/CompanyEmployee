using System.ComponentModel.DataAnnotations;
namespace Entities.DTO;
public class UserForRegistrationDto
{
    public string FirstName {get; set;}
    public string Lastname {get; set;}
    [Required(ErrorMessage= "Username is required" )]
    public string UserName {get; set;}
    [Required(ErrorMessage= "Password is required")]
    public string Password {get; set;}
    public string Email {get; set;}
    public string Phone {get; set;}
    public ICollection<string> Roles {get; set;}
}
