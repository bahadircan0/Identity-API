using DotnetAuthentication.Context;
using DotnetAuthentication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAuthentication.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public sealed class UserRolesController(ApplicationDbContext context, UserManager<AppUser> userManager) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> Create(Guid userId,string roleName,CancellationToken cancellationToken)
        {
            //1.yöntem
            //AppUserRole appUserRole = new()
            //{
            //    RoleId = roleId,
            //    UserId = userId
            //};

            //await context.UserRoles.AddAsync(appUserRole);

            //await context.SaveChangesAsync(cancellationToken);



            //2.yöntem
            AppUser? user = await userManager.FindByIdAsync(userId.ToString());

            if (user is null)
            {
                return BadRequest(new { Message = "Kullanıcı bulunamadı" });
            }

            IdentityResult result= await userManager.AddToRoleAsync(user, roleName);

            if (!result.Succeeded) {
                return BadRequest(result.Errors.Select(s => s.Description));


            }

            return NoContent();
        }


    }
}
