using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.Web.Controller;

[Route("[controller]/[action]")]
public class AccountController : Microsoft.AspNetCore.Mvc.Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
    {
        if(ModelState.IsValid == false)
            return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        
        ApplicationUser user = new ApplicationUser
        {
            UserName = registerDTO.Email,
            Email = registerDTO.Email,
            PhoneNumber =  registerDTO.Phone,
            PersonName = registerDTO.PersonName
        };
        IdentityResult result = await _userManager.CreateAsync(user);
        if(result.Succeeded) 
            return Created();

        foreach (IdentityError error in result.Errors)
        {
            ModelState.AddModelError(error.Code, error.Description);
        }

        return BadRequest(result.Errors.Select(e => e.Description));
    }
}