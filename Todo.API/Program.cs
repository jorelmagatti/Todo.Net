using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using Todo.Services.Interfaces;
using Todo.Services.Services;
using Todo.Data.Contexts;
using System.Globalization;

namespace Todo.API
{
    public class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            });
            builder.Services.AddCors();
            builder.Services.AddSingleton(typeof(ILogger), typeof(Logger<IServiceScope>));
            IConfiguration Configuration = builder.Configuration;
            builder.Services.AddSingleton<IConfiguration>(Configuration);

            builder.Services.AddDbContext<TodoSqlServerContext>(options => options.UseSqlServer(Configuration.GetConnectionString("TodoContext")));


            var hash = Configuration["Jwt:Key"];
            var key = Encoding.ASCII.GetBytes(hash);
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(2);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            builder.Services.AddMvc().AddSessionStateTempDataProvider();
            builder.Services.AddSession();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Todo.API",
                    Version = "v1",
                    Contact = new OpenApiContact() { Name = "Jorel Magatti", Email = "jorel_dev@hotmail.com" },
                    Description = "Uma API rest .Net 8 para Todo List",
                    License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://mit-license.org/") },
                    TermsOfService = new Uri("https://www.google.com")
                });
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "JWT Authorization header using the Bearer scheme.",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                c.AddSecurityDefinition("Bearer", securityScheme);
                var securityRequirement = new OpenApiSecurityRequirement
            {
                { securityScheme, new[] { "Bearer" } }
            };

                c.AddSecurityRequirement(securityRequirement);
            });
            builder.Services.AddMemoryCache();

            builder.Services.Scan(scan => scan
                .FromAssemblyOf<IServiceScoped>()
                    .AddClasses(classes => classes.AssignableTo<IServiceScoped>())
                        .AsSelf()
                        .WithScopedLifetime());

            var app = builder.Build();

            var cultureInfo = CultureInfo.CurrentCulture.Clone() as CultureInfo;
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            app.UseAuthentication();
            app.UseSession();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}


