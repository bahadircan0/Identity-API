using DotnetAuthentication.Dtos;
using DotnetAuthentication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace DotnetAuthentication.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public sealed class AuthController(UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager) : ControllerBase
    {

        //Kullanıcı ekleme
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto request,CancellationToken cancellationToken)
        {

            AppUser appUser = new()
            {
                Email = request.Email,
                UserName = request.UserName,
                FirstName = request.FirstName,
                Lastname = request.LastName,  

            };

            
            IdentityResult reslt=await userManager.CreateAsync(appUser,request.Password);
            if (!reslt.Succeeded)
            {
                return BadRequest(reslt.Errors.Select(s=>s.Description));
            }



            return NoContent();
        }



        //Şifre değiştirme
        [HttpPost]

        public async Task<IActionResult> ChangePassword(CahngePasswordDto request, CancellationToken cancellationToken)
        {
            AppUser? appUser = await userManager.FindByIdAsync(request.Id.ToString());

            if(appUser is null)
            {
                return BadRequest(new { Message = "" });
            }
            IdentityResult result= await userManager.ChangePasswordAsync(appUser, request.CurrentPassword, request.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(s => s.Description));
            }

            return NoContent();
        }


        //Şifremi unuttum seçeneği ile şifre değiştirme
        [HttpGet]
        public async Task<IActionResult> ForgetPassword(string email, CancellationToken cancellationToken)
        {


            AppUser ? appUser=await userManager.FindByEmailAsync(email);

            if (appUser is null)
            {
                return BadRequest(new { Message = "" });
            }

            string token= await userManager.GeneratePasswordResetTokenAsync(appUser);

            return Ok(new {Token=token});


        }

        //Şifremi unuttum seçeneği ile şifre değiştirme
        [HttpPost]
        public async Task<IActionResult> ChangePasswordUsingToken(ChangePasswordUsingDto request, CancellationToken cancellationToken)
        {
            AppUser? appUser = await userManager.FindByEmailAsync(request.Email);

            if (appUser is null)
            {
                return BadRequest(new { Message = "Kullanıcı bulunamadı" });
            }
            IdentityResult result = await userManager.ResetPasswordAsync(appUser,request.Token,request.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(s => s.Description));
            }

            return NoContent();


        }



        //Giriş yapma 
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto request,CancellationToken cancellationToken)
        {
            AppUser? appUser = await userManager.Users.FirstOrDefaultAsync(p =>
            p.Email == request.UserNameOrEmail || 
            p.UserName == request.UserNameOrEmail, cancellationToken);


            if (appUser is null)
            {
                return BadRequest(new { Message = "Kullanıcı bulunamadı" });
            }

           bool result= await userManager.CheckPasswordAsync(appUser, request.Password);
            if (!result) return BadRequest(new { Message = "Şifre yanlış" });



            return Ok(new { Token = "Token" });
        }



        //Login olma ama signInManager kullanarak,Yani fazla yanlış giriş yaptığında 3 dk hesabını kilitlemek gibi.
        [HttpPost]
        public async Task<IActionResult> LoginWithSignInManager(LoginDto request, CancellationToken cancellationToken)
        {
            AppUser? appUser = await userManager.Users.FirstOrDefaultAsync(p =>
            p.Email == request.UserNameOrEmail ||
            p.UserName == request.UserNameOrEmail, cancellationToken);


            if (appUser is null)
            {
                return BadRequest(new { Message = "Kullanıcı bulunamadı" });
            }

            SignInResult result= await signInManager.CheckPasswordSignInAsync(appUser, request.Password, true);

            if (result.IsLockedOut)
            {
               TimeSpan? timeSpan= appUser.LockoutEnd-DateTime.Now;

                if(timeSpan is not null)
                {
                    return StatusCode(500,
                        $"Şifrenizi 3 kere yanlış girdiğiniz için kullanıcınız" +
                        $" {timeSpan.Value.TotalSeconds} saniye girişe yasaklanmıştır.");
                }
                else
                {
                    return StatusCode(500,
                        "Şifrenizi 3 kere yanlış girdiğiniz için kullanıcınız 30 saniye girişe yasaklanmıştır.");
                }
            }

            if (!result.Succeeded)
            {
                return StatusCode(500, "Şifreniz yanlış");
            }

            if (result.IsNotAllowed)
            {
                return StatusCode(500,"Mail adresiniz onaylı değil");
            }
           
            return Ok(new { Token = "Token" });

        }


    }
}
