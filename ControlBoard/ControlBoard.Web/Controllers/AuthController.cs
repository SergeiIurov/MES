using ControlBoard.Web.Auth;
using ControlBoard.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ControlBoard.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(UserManager<ApplicationUser?> userManager, SignInManager<ApplicationUser?> signInManager) : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult> Login(LoginInfo loginInfo)
    {
        ApplicationUser? user = await userManager.FindByNameAsync(loginInfo.UserName);
        if (user != null && (await signInManager.CheckPasswordSignInAsync(user, loginInfo.Password, true)).Succeeded)
        {

            var jwt = new JwtSecurityToken(
                AuthOptions.Issuer,
                AuthOptions.Audience,
                await userManager.GetClaimsAsync(user),
                expires: DateTime.MaxValue,
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256));

            var res = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
        }

        return Unauthorized();
    }

    [HttpPost("CreateUser")]
    [Authorize]
    public async Task<ActionResult> CreateUser(UserInfo userInfo)
    {

        ApplicationUser? user = await userManager.FindByNameAsync(userInfo.Name);
        if (user == null)
        {
            user = new ApplicationUser { UserName = userInfo.Name };
            var result = await userManager.CreateAsync(user, userInfo.Password);
            if (result.Succeeded)
            {
                await userManager.AddClaimsAsync(user, [
                    new Claim(ClaimTypes.Name, userInfo.Name),
                    new Claim(ClaimTypes.Role, userInfo.Role.ToString())
                ]);
                return Created();
            }
        }

        return BadRequest();
    }

    [HttpPost("DeleteUser")]
    [Authorize]
    public async Task<ActionResult> DeleteUser(string name)
    {
        ApplicationUser? user = await userManager.FindByNameAsync(name);
        if (user != null)
        {
            await userManager.DeleteAsync(user);
            return Ok();
        }

        return BadRequest();
    }
}