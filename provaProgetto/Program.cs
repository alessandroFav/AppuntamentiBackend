using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using provaProgetto;
using provaProgetto.Dipendenze;
using provaProgetto.Middlewares;
using provaProgetto.Models;
using System.Configuration;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorTemplating();

//var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]);

//builder.Services.AddAuthentication(x =>
//{
//    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(x =>
//{
//    x.RequireHttpsMetadata= false;
//    x.SaveToken = true;
//    x.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuerSigningKey= true,
//        IssuerSigningKey = new SymmetricSecurityKey(key),
//        ValidIssuers = new string[] { builder.Configuration["Jwt:Issuer"] },
//        ValidAudiences = new string[] { builder.Configuration["Jwt:Issuer"] },
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//    };
//});
builder.Services.AddScoped<IGestioneUtente, GestioneUtente>();
builder.Services.AddScoped<IJwtManager, JwtManager>();
builder.Services.AddScoped<IGestioneDati, GestioneDati>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.UseJwtMiddleware();
app.UseParamsMiddleware();
app.Run();

