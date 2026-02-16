using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTO;
using ContactsManager.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.Web.Controller;

[Route("[controller]/[action]")]
[AllowAnonymous]
public class AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
                                RoleManager<ApplicationRole> roleManager) : Microsoft.AspNetCore.Mvc.Controller
{
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
        IdentityResult result = await userManager.CreateAsync(user, registerDTO.Password);
        if (result.Succeeded)
        {
            if (registerDTO.UserType == UserTypeOptions.Admin)
            {
                //Create Admin Role
                if (await roleManager.FindByNameAsync(UserTypeOptions.Admin.ToString()) is null)
                {
                    ApplicationRole applicationRole = new ApplicationRole()
                    {
                        Name = UserTypeOptions.Admin.ToString() 
                    };
                    await roleManager.CreateAsync(applicationRole);
                }
                // Add new user to Admin Role
                await userManager.AddToRoleAsync(user, UserTypeOptions.Admin.ToString());
            }   
            
            else
            {
                // Add new user to User Role
                await userManager.AddToRoleAsync(user, UserTypeOptions.User.ToString());
            }
            await signInManager.SignInAsync(user, isPersistent: false);
            return Created();
        }

        foreach (IdentityError error in result.Errors)
        {
            ModelState.AddModelError(error.Code, error.Description);
        }

        return BadRequest(result.Errors.Select(e => e.Description));
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

        var result = await signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, isPersistent: false,
            lockoutOnFailure: false);
        if(result.Succeeded)
            return Ok("Login successful");
        return Unauthorized("Invalid email or password");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return Ok("Logged out successfully");
    }
}