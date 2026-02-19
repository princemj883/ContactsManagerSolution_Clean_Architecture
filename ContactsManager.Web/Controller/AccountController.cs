using System.Security.Claims;
using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTO;
using ContactsManager.Core.Enums;
using ContactsManager.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.Web.Controller;

[Route("[controller]/[action]")]
[AllowAnonymous]
public class AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
                                RoleManager<ApplicationRole> roleManager, IJwtService jwtService) : Microsoft.AspNetCore.Mvc.Controller
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
            var authenticationResponse = jwtService.CreateJwtToken(user);
            user.RefreshToken = authenticationResponse.RefreshToken;
            user.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationDateTime;
            await userManager.UpdateAsync(user);
            return Ok(authenticationResponse);
        }

        foreach (IdentityError error in result.Errors)
        {
            ModelState.AddModelError(error.Code, error.Description);
        }

        return BadRequest(result.Errors.Select(e => e.Description));
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        
        var user = await userManager.FindByNameAsync(loginDTO.Email);

        if (user == null)
            return BadRequest("User not found in database");
        // Verify password against the user store (works regardless of authentication scheme)
        var passwordValid = await userManager.CheckPasswordAsync(user, loginDTO.Password);
        if (!passwordValid)
            return Unauthorized("Invalid email or password");

        var authenticationResponse = jwtService.CreateJwtToken(user);

        user.RefreshToken = authenticationResponse.RefreshToken;
        user.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationDateTime;
        await userManager.UpdateAsync(user);

        return Ok(authenticationResponse);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return Ok("Logged out successfully");
    }
    [HttpPost("generate-new-access-token")]
    [AllowAnonymous]
    public async Task<IActionResult> GenerateNewAccessToken([FromBody]TokenModel? tokenModel)
    {
        if(tokenModel == null)
            return BadRequest("Invalid client request");
        
        ClaimsPrincipal? principal = jwtService.GetPricipalFromJwtToken(tokenModel.Token);
        if(principal == null)
            return BadRequest("Invalid client request");

        string? email = principal.FindFirstValue(ClaimTypes.Email);
        ApplicationUser? applicationUser = await userManager.FindByEmailAsync(email);
        if(applicationUser == null || applicationUser.RefreshToken != tokenModel?.RefreshToken || applicationUser.RefreshTokenExpirationDateTime <= DateTime.UtcNow)
            return BadRequest("Invalid refresh token");
        
        AuthenticationResponse authenticationResponse = jwtService.CreateJwtToken(applicationUser);
        applicationUser.RefreshToken = authenticationResponse.RefreshToken;
        applicationUser.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationDateTime;
        await userManager.UpdateAsync(applicationUser);
        return Ok(authenticationResponse);
    }
}