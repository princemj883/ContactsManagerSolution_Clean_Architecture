using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities;

public class Person
{
    [Key]
    public Guid PersonId { get; set; }
    
    [StringLength(40)]  //nvarchar(40)
    public string? PersonName { get; set; }
    
    [StringLength(40)] 
    public string? Email { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    
    [StringLength(10)] 
    public string? Gender { get; set; }
    
    //unique identifier
    public Guid? CountryId { get; set; }
    
    [ForeignKey("CountryId")]
    public virtual Country? Country { get; set; }
    
    [StringLength(200)] 
    public string? Address { get; set; }
    
    //bit
    public bool ReceiveNewsLetter { get; set; }
    
    public string? TIN { get; set; }
}