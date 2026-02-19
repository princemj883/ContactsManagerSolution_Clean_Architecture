using System.Security.Claims;
using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTO;

namespace ContactsManager.Core.ServiceContracts;

public interface IJwtService
{
    AuthenticationResponse CreateJwtToken(ApplicationUser user);
    ClaimsPrincipal? GetPricipalFromJwtToken(string? token);
}