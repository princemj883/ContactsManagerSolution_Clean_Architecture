using System.ComponentModel.DataAnnotations;

namespace ContactsManager.Core.DTO;

public class LoginDTO
{
    [Required(ErrorMessage = "Email cannot be blank")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string? Email { get; set; }
    
    [Required(ErrorMessage = "Password cannot be blank")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
}