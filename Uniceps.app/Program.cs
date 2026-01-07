using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Uniceps.app.DTOs.ExerciseDtos;
using Uniceps.app.DTOs.MuscleGroupDtos;
using Uniceps.app.Extensions;
using Uniceps.app.HostBuilder;
using Uniceps.app.Middleware;
using Uniceps.app.Services;
using Uniceps.app.Services.PaymentServices;
using Uniceps.Core.Services;
using Uniceps.Entityframework;
using Uniceps.Entityframework.DBContext;
using Uniceps.Entityframework.Models.AuthenticationModels;
using Uniceps.Entityframework.Models.RoutineModels;
using Uniceps.Entityframework.Services;
using Uniceps.Entityframework.Services.ExerciseServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(
    options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("default")));
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddIdentity<AppUser,IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddDataServices();
builder.Services.AddMappers();
builder.Services.AddSystemServices();
builder.Services.AddCustomJwtAuth(builder.Configuration);
builder.Services.AddFirebaseAdmin(builder.Configuration);
builder.Services.AddHostedService<NotificationWorker>();
//builder.Services.AddMediatR(cfg =>
//    cfg.RegisterServicesFromAssemblyContaining<Lib>());
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddCustomSwaggerGenAuth();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000",
                    "https://uniceps-admin.vercel.app",
                    "https://uniceps.trio-verse.com")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials(); 
        });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
}
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>(); 
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "حدث خطأ أثناء عمل Migration لقاعدة البيانات.");
    }
}
await DbInitializer.SeedRolesAndAdminAsync(app.Services);
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowFrontend");
app.UseAuthorization();
app.UseMiddleware<UserTypeMiddleware>();
app.MapControllers();

app.Run();
