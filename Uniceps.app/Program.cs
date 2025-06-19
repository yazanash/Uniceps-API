using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Uniceps.app.DTOs.ExerciseDtos;
using Uniceps.app.DTOs.MuscleGroupDtos;
using Uniceps.app.Extensions;
using Uniceps.app.HostBuilder;
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
//builder.Services.AddMediatR(cfg =>
//    cfg.RegisterServicesFromAssemblyContaining<Lib>());
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddCustomSwaggerGenAuth();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();

app.Run();
