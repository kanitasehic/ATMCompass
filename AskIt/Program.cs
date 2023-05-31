using ATMCompass.Core.Mappings;
using ATMCompass.Extensions;
using ATMCompass.Insfrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var AllowSpecificOrigins = "_allowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureCorsPolicy(AllowSpecificOrigins);

builder.Services.AddDbContext<ATMCompassDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfiles)));

builder.Services.RegisterServices();

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