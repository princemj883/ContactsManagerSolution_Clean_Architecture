using System.ComponentModel.DataAnnotations;

namespace Entities;

/// <summary>
/// Domain model for storing the Country
/// </summary>
public class Country
{
    [Key]
    public Guid CountryId { get; set; }
    public string? CountryName { get; set; }
}