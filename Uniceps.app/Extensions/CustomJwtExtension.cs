using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Uniceps.app.Extensions
{
    public static class CustomJwtExtension
    {
        public static void AddCustomJwtAuth(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = configurationManager["JWT:Issuer"],
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configurationManager["JWT:SecretKey"]!))
                };
                o.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        // أولاً جرّب تقرأ من الـ Authorization header
                        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                        {
                            context.Token = authHeader.Substring("Bearer ".Length).Trim();
                        }
                        // إذا ما في Authorization header، جرّب تقرأ من الكوكي
                        else if (context.Request.Cookies.ContainsKey("auth"))
                        {
                            context.Token = context.Request.Cookies["auth"];
                        }

                        return Task.CompletedTask;
                    }
                };

            });
        }
        public static void AddCustomSwaggerGenAuth(this IServiceCollection services)
        {
            services.AddSwaggerGen(o =>
            {
                o.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "Uniceps API",
                    Description = "Docs For Using Uniceps api",
                    Contact = new OpenApiContact()
                    {
                        Email = "yazan.ash.doonaas@gmail.com",
                        Name = "ANACONDA",

                    }
                });
                o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT Here"
                });
                o.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type= ReferenceType.SecurityScheme,
                                Id="Bearer"
                            },
                             Name = "Bearer",
                             In = ParameterLocation.Header,

                        },
                         new List<string>()
                    }
                });
            });
        }
    }
}
