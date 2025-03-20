using DotnetAuthentication.Context;
using DotnetAuthentication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Mail;
using System.Net;
using System.Web;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//veritaban� ba�lant�s� ve ApplicationDbContext ba�lant�s�n� yapt�k.
builder.Services.AddDbContext<ApplicationDbContext>(options=>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});


//Identity k�t�phanesini dependenciy injection ettik
builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false; 
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 1;

    options.User.RequireUniqueEmail = true;
    

    options.SignIn.RequireConfirmedEmail=true;
    options.Lockout.MaxFailedAccessAttempts = 3;  //ka� kere yanl�� girdi�inde kilitleyece�i
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(30);    //ne akdar s�re kilitleyece�i

})
  .AddEntityFrameworkStores<ApplicationDbContext>()
  .AddDefaultTokenProviders();





var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


