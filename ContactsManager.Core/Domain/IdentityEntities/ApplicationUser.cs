using Microsoft.AspNetCore.Identity;

namespace ContactsManager.Core.Domain.IdentityEntities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string? PersonName { get; set; }
    public string? RefreshToken { get; set; } = string.Empty;
    
    public DateTime RefreshTokenExpirationDateTime { get; set; }
    
}