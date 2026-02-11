using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO;


/// <summary>
/// Represents the DTO class  that contains the person details to update
/// </summary>
public class PersonUpdateRequest
{
    [Required(ErrorMessage = "PersonId is required")]
    public Guid PersonId { get; set; }
    
    [Required(ErrorMessage = "Person Name cannot be empty.")]
    public string? PersonName { get; set; }
    
    [Required(ErrorMessage = "Email cannot be empty.")]
    [EmailAddress(ErrorMessage = "Invalid Email Address.")]
    public string? Email { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    
    public GenderOptions? Gender { get; set; }
    
    public Guid? CountryId { get; set; }
    
    public string? Address { get; set; }
    
    public bool ReceiveNewsLetter { get; set; }

    public Person ToPerson()
    {
        return new Person()
        {
            PersonId = PersonId, PersonName = PersonName, Email = Email, DateOfBirth = DateOfBirth, Gender = Gender.ToString(),
            CountryId = CountryId, Address = Address, ReceiveNewsLetter = ReceiveNewsLetter
        };
    }
}