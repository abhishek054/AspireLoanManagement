using AspireLoanManagement.Utility.Config;
using AspireLoanManagement.Utility.Mapper;
using AspireLoanManagement.Business;
using AspireLoanManagement.Repository;
using AspireLoanManagement.Utility.Cache;
using AspireLoanManagement.Utility.Logger;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using AspireLoanManagement.Business.Models;
using AspireLoanManagement.Utility.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
builder.Services.AddScoped<ILoanRepository, LoanRepository>();
builder.Services.AddSingleton<IAspireCacheService, AspireCacheManager>();
builder.Services.AddSingleton<IAspireLogger, AspireLoggerManager>();
builder.Services.AddScoped<IValidator<LoanModelVM>, CreateLoanPayloadValidator>();
builder.Services.AddScoped<IValidator<RepaymentModelVM>, RepaymentModelValidator>();
builder.Services.AddDbContext<LoanDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("AspireDBConnectionString")));

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
