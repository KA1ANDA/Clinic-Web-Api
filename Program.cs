
using ClinicWebApi.Auth;
using ClinicWebApi.EmailSend;
using ClinicWebApi.Filters;
using ClinicWebApi.Packages;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace ClinicWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IPKG_TK_PATIENTS, PKG_TK_PATIENTS>();
            builder.Services.AddScoped<IPKG_TK_AUTHENTICATION , PKG_TK_AUTHENTICATION>();
            builder.Services.AddScoped<IJwtManager , JwtManager>();
            builder.Services.AddScoped<IPKG_TK_DOCTORS , PKG_TK_DOCTORS>();
            builder.Services.AddScoped<IPKG_TK_DOCTOR_SPECIALIZATIONS, PKG_TK_DOCTOR_SPECIALIZATIONS>();
            builder.Services.AddScoped<IPKG_TK_PATIENT_ACTIVATION_CODES, PKG_TK_PATIENT_ACTIVATION_CODES>();
            builder.Services.AddScoped<IPKG_TK_BOOKING, PKG_TK_BOOKING>();


            

            builder.Services.AddSingleton<IEmailSender, EmailSender>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllCors", config =>
                {
                    config.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            builder.Services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });


            builder.Services.AddScoped<GlobalExceptionFilter>();
            builder.Services.AddControllers(config =>
            {
                config.Filters.AddService(typeof(GlobalExceptionFilter));
            });

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                var key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });





            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAllCors");
            app.UseCors("AllowAll");
            app.UseCors("AllowSpecificOrigin");

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
