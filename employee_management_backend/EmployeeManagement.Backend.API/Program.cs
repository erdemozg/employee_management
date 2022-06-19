using EmployeeManagement.Backend.DatabaseContext;
using EmployeeManagement.Backend.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var allowedOriginsPolicyName = "allowedOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(
    options => options.AddPolicy(allowedOriginsPolicyName, 
        builder => builder.WithOrigins("*")
            .AllowAnyHeader()
            .AllowAnyMethod()
        )
    );

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<FormOptions>(o => {
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});

builder.Services.AddDbContext<EmployeeManagementContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("EmployeeManagementContext")));

builder.Services.AddScoped<IEmployeeManagementContext, EmployeeManagementContext>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<IEmployeeManagementContext>();
    context.Database.EnsureCreated();
}

//app.UseHttpsRedirection();

app.UseCors(allowedOriginsPolicyName);

app.UseAuthorization();

app.MapControllers();

app.Run();
