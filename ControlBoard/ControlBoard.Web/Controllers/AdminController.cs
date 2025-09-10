using ControlBoard.Web.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace ControlBoard.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AdminController(
        UserManager<ApplicationUser?> userManager,
        AppDbContext context) : ControllerBase
    {
        [HttpPost("CreateUser")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> CreateUser(UserInfo userInfo)
        {
            try
            {
                ApplicationUser? user = await userManager.FindByNameAsync(userInfo.Name);
                if (user == null)
                {
                    user = new ApplicationUser { UserName = userInfo.Name, ActiveUserName = "_", MachineName = "_" };
                    var result = await userManager.CreateAsync(user, userInfo.Password);
                    if (result.Succeeded)
                    {
                        await userManager.AddClaimsAsync(user, [
                            new Claim(ClaimTypes.Name, userInfo.Name),
                        new Claim(ClaimTypes.Role, userInfo.Role.ToString())
                        ]);
                        return Ok(userInfo);
                    }
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();

            }
        }

        [HttpDelete("DeleteUser/{name}")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteUser(string name)
        {
            ApplicationUser? user = await userManager.FindByNameAsync(name);
            if (user != null)
            {
                await userManager.DeleteAsync(user);
                return Ok(new { Name = name });
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
                    Role = MesRoles.TryParse(claim.Value, out MesRoles role) ? role : MesRoles.User,
                    IsActive = u.IsActive
                });
            });
            return users;
        }

        //Смена пароля
        [HttpPut("ChangePassword/{newPassword}")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> ChangePassword(UserInfo userInfo, string newPassword)
        {
            ApplicationUser? appUser = await userManager.FindByNameAsync(userInfo.Name);
            if (appUser != null)
            {
                var result = await userManager.ChangePasswordAsync(appUser, userInfo.Password, newPassword);
                if (result.Succeeded)
                {
                    return Ok();
                }

                return BadRequest(result);
            }

            return BadRequest();
        }

        //Смена роли
        [HttpPut("ChangeRole/{newRole}")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> ChangeRole(UserInfo userInfo, MesRoles newRole)
        {
            ApplicationUser? appUser = await userManager.FindByNameAsync(userInfo.Name);
            if (appUser != null)
            {
                Claim from = new Claim(ClaimTypes.Role, userInfo.Role.ToString());
                Claim to = new Claim(ClaimTypes.Role, newRole.ToString());
                var result = await userManager.ReplaceClaimAsync(appUser, from, to);
                if (result.Succeeded)
                {
                    return Ok();
                }
            }

            return BadRequest();
        }

        //Смена активности
        [HttpPut("ResetActivity")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> ResetActivity(UserInfo userInfo)
        {
            try
            {

                ApplicationUser? appUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == userInfo.Name);
                if (appUser != null)
                {
                    appUser.IsActive = false;
                    await context.SaveChangesAsync();

                    return Ok(appUser);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ModelState);
                throw;
            }

            return NotFound();
        }
    }
}