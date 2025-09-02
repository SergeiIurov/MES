using ControlBoard.Web.Auth;
using ControlBoard.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace ControlBoard.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AuthController(
    AppDbContext context,
    UserManager<ApplicationUser?> userManager,
    SignInManager<ApplicationUser?> signInManager)
    : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult> Login(LoginInfo loginInfo)
    {
        ApplicationUser? user = await userManager.FindByNameAsync(loginInfo.UserName);

        var r = context.UserClaims.Join(context.Users, uc => uc.UserId, u => u.Id, (us, u) => new { us.ClaimType, us.ClaimValue, u.UserName, u.IsActive }).
            Where(info =>
                (info.ClaimValue.Equals("Admin") || info.ClaimValue.Equals("Operator"))
                && info.IsActive == true);
        Claim claim = null;
        if (user != null)
        {
            claim = (await userManager.GetClaimsAsync(user)).Where(c => c.Type == ClaimTypes.Role).FirstOrDefault();
            if (claim != null)
            {
                if (claim.Value != MesRoles.User.ToString() && r.Any() && user.UserName != AuthConstants.Superuser && user.UserName != r.First().UserName)
                {
                    return BadRequest(new
                    {
                        Message = $"В настоящее время активен другой пользователь с учетной записью 'Admin' или 'Operator': {string.Join(", ", r.Select(u => u.UserName
                    ))}"
                    });
                }
            }
        }

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

            user.IsActive = claim.Value is not null && claim.Value != MesRoles.User.ToString();
            user.MachineName = Environment.MachineName;
            user.ActiveUserName = Environment.UserName;
            context.Users.Update(user);
            await context.SaveChangesAsync();
            return Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
        }

        return Unauthorized();
    }

    [HttpPost("logout")]
    [AllowAnonymous]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult> LogOut(InputData data)
    {
        ApplicationUser user = await context.Users.FirstOrDefaultAsync(u => u.UserName == data.UserName);
        if (user is not null)
        {
            user.IsActive = false;
            user.MachineName = "";
            user.ActiveUserName = "";
            await context.SaveChangesAsync();
        }

        return Ok();
    }
}

public class InputData
{
    public string UserName { get; set; }
}