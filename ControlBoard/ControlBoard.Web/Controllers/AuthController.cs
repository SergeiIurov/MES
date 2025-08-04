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
public class AuthController(UserManager<ApplicationUser?> userManager, SignInManager<ApplicationUser?> signInManager)
    : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
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
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
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
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
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


    //Получение списка пользователей с ролями
    [HttpGet("Users")]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IEnumerable<UserInfo>> GetUsers()
    {
        List<UserInfo> users = new List<UserInfo>();
        List<ApplicationUser?> appUsers = userManager.Users.ToList();
        appUsers.ForEach(u =>
        {
            var claim = userManager.GetClaimsAsync(u).Result.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            users.Add(new UserInfo()
            {
                Name = u.UserName,
                Role = MesRoles.TryParse(claim.Value, out MesRoles role) ? role : MesRoles.User
            });
        });
        return users;
    }

    //Смена пароля
    [HttpPost("ChangePassword")]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult> ChangePassworkd(UserInfo userInfo, string newPassword)
    {
        ApplicationUser? appUser = await userManager.FindByNameAsync(userInfo.Name);
        if (appUser != null)
        {
            var result = await userManager.ChangePasswordAsync(appUser, userInfo.Password, newPassword);
            if (result.Succeeded)
            {
                return Ok();
            }
        }

        return BadRequest();

    }
}