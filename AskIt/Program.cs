using ATMCompass.Core.Configuration;
using ATMCompass.Core.Mappings;
using ATMCompass.Extensions;
using ATMCompass.Insfrastructure.Data;
using ATMCompass.Core.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var AllowSpecificOrigins = "_allowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

var appSettings = new AppSettings();
builder.Configuration.Bind(appSettings);

builder.Services.ConfigureCorsPolicy(AllowSpecificOrigins);

builder.Services.AddDbContext<ATMCompassDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.SetupAuthentication(appSettings);

builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfiles)));

builder.Services.RegisterServices(appSettings);

builder.Services.RegisterRepositories();

builder.Services.AddControllers();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.ConfigureSwagger();

// build app

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

UpdateDatabase(app);

SeedData(app, appSettings);

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(AllowSpecificOrigins);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

static void UpdateDatabase(IApplicationBuilder app)
{
    using IServiceScope serviceScope = app.ApplicationServices
        .GetRequiredService<IServiceScopeFactory>()
        .CreateScope();

    using ATMCompassDbContext context = serviceScope.ServiceProvider.GetService<ATMCompassDbContext>();

    context.Database.Migrate();
}
static async void SeedData(IApplicationBuilder app, AppSettings appSettings)
{
    using IServiceScope serviceScope = app.ApplicationServices
    .GetRequiredService<IServiceScopeFactory>()
    .CreateScope();

    var userManager = serviceScope.ServiceProvider.GetService<UserManager<IdentityUser>>();
    var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

    await DbHelper.SeedAdminRole(roleManager);
    await DbHelper.SeedAdminUser(userManager, appSettings.AdminCredentials);
}