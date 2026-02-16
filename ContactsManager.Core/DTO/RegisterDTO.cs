using System.ComponentModel.DataAnnotations;
using ContactsManager.Core.Enums;

namespace ContactsManager.Core.DTO;

public class RegisterDTO
{
    [Required(ErrorMessage = "Name cannot be blank")]
    public string PersonName { get; set; }
    
    [Required(ErrorMessage = "Email cannot be blank")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Phone cannot be blank")]
    [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number must contain only digits")]
    public string Phone { get; set; }
    
    [Required(ErrorMessage = "Password cannot be blank")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "ConfirmPassword cannot be blank")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }

    public UserTypeOptions UserType { get; set; } = UserTypeOptions.User;
}