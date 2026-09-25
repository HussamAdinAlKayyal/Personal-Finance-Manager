using Finance.Api;
using Finance.Api.Endpoints;
using Finance.Api.Extensions;
using Finance.Api.Requests;
using Finance.Api.Validators;
using Finance.Application;
using Finance.Infrastructure;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<IValidator<CreateFundRequest>, CreateFundRequestValidator>();
builder.Services.AddScoped<IValidator<UpdateFundRequest>, UpdateFundRequestValidator>();
builder.Services.AddScoped<IValidator<RegistrationRequest>, RegistrationRequestValidator>();
builder.Services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
builder.Services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    IConfigurationSection section = builder.Configuration.GetRequiredSection("Jwt");
    options.TokenValidationParameters = new()
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidAudience = section["Audience"],
        ValidIssuer = section["Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(section["Key"]!)),
    };
});
builder.Services.AddAuthorization();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
var app = builder.Build();
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
app.MigrateDb();
app.SeedDb(builder.Configuration);
app.MapControllers();
app.MapCategoryEndpoints();
app.MapCurrencyEndpoints();
app.MapGet("/", () => "Hello World!");
app.Run();
