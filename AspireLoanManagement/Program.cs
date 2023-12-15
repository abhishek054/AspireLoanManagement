using AspireLoanManagement.Utility.Config;
using AspireLoanManagement.Utility.Mapper;
using AspireLoanManagement.Repository;
using AspireLoanManagement.Utility.Cache;
using AspireLoanManagement.Utility.Logger;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using AspireLoanManagement.Business.Models;
using AspireLoanManagement.Utility.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AspireLoanManagement.Business.Loan;
using AspireLoanManagement.Business.Authentication;
using System.Security.Claims;
using AspireLoanManagement.Repository.Loan;
using AspireLoanManagement.Repository.Repayment;
using AspireLoanManagement.Business.Repayment;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<AspireConfigModel>(builder.Configuration.GetSection("AspireConfig"));
builder.Services.AddSingleton<IAspireConfigService, AspireConfigService>();
builder.Services.AddMemoryCache();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});
builder.Services.AddScoped<ILoanService, LoanService>();
builder.Services.AddScoped<IRepaymentService, RepaymentService>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();
builder.Services.AddScoped<IRepaymentRepository, RepaymentRepository>();
builder.Services.AddSingleton<IAspireCacheService, AspireCacheManager>();
builder.Services.AddSingleton<IAspireLogger, AspireLoggerManager>();
builder.Services.AddScoped<IValidator<LoanModelVM>, CreateLoanPayloadValidator>();
builder.Services.AddScoped<IValidator<RepaymentModelVM>, RepaymentModelValidator>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("LoanManagerPolicy", policy =>
        policy.RequireClaim(ClaimTypes.Role, "LoanManager"));
    options.AddPolicy("LoanCustomerPolicy", policy =>
        policy.RequireClaim(ClaimTypes.Role, "LoanCustomer"));
});

builder.Services.AddDbContext<AspireDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("AspireDBConnectionString")));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-secret-key")),
        ValidIssuer = "your-issuer",
        ValidAudience = "your-audience"
    };
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Aspire Loan Management API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
