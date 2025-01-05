using System.Text;
using Common;
using Common.Models;
using DataAccess;
using DataAccess.Context;
using DataAccess.Repositories.Implementations;
using DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Movie_Verse.Interfaces;
using Movie_Verse.JWT;
using Movie_Verse.Services;

namespace WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            
            Console.WriteLine("JWT Configuration:");
            Console.WriteLine($"- Issuer: {Configuration["JWTSettings:Issuer"]}");
            Console.WriteLine($"- Key: {Configuration["JWTSettings:Key"]}");
            Console.WriteLine($"- Audience: {Configuration["JWTSettings:Audience"]}");
            
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddAutoMapper(typeof(Startup));

            // Налаштування контексту БД
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            // Налаштування Identity
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddRoles<IdentityRole>();

            // Додаємо власні сервіси
            AddCustomServices(services, Configuration);
            
            services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp", policy =>
                {
                    policy.WithOrigins("http://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowReactApp");
            app.UseAuthentication();
            app.UseAuthorization();

            // Для обробки помилок
            app.UseMiddleware<ExceptionHandlingMiddleware>(); // Якщо є Middleware для обробки помилок

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void AddJwtAuthentication(IServiceCollection services, JWTSettings jwtSettings)
        {
            if (string.IsNullOrEmpty(jwtSettings.Key))
            {
                Console.WriteLine("JWT Key ne vkazano!");
            }

            Console.WriteLine($"Settings JWT:");
            Console.WriteLine($"- Issuer: {jwtSettings.Issuer}");
            Console.WriteLine($"- Audience: {jwtSettings.Audience}");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                    };
                });
        }

        private void AddDataAccessServices(IServiceCollection services, IConfiguration configuration)
        {
            // Використання PostgreSQL
            services.AddDbContext<ApplicationDbContext>(options =>  
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        }

        private void AddBusinessLogicServices(IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJWTService, JWTService>();
            // Додайте інші сервіси за необхідності
        }

        private void ConfigureJwtAndAutoMapper(IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JWTSettings").Get<JWTSettings>();

            if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.Key))
            {
                Console.WriteLine("JWTSettings або Key не заповнені!");
                throw new InvalidOperationException("JWTSettings або JWT Key не можуть бути null. Перевірте конфігурацію.");
            }
            else
            {
                Console.WriteLine($"JWT Key: {jwtSettings.Key}");
                Console.WriteLine($"JWT Issuer: {jwtSettings.Issuer}");
                Console.WriteLine($"JWT Audience: {jwtSettings.Audience}");
            }

            services.AddSingleton(jwtSettings);
            
            services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));
        }

        private void AddCustomServices(IServiceCollection services, IConfiguration configuration)
        {
            // Налаштовуємо JWT та AutoMapper
            ConfigureJwtAndAutoMapper(services, configuration);
            // Додаємо сервіси для доступу до даних
            AddDataAccessServices(services, configuration);
            // Додаємо бізнес-логіку
            AddBusinessLogicServices(services);

            // Налаштовуємо JWT аутентифікацію
            var jwtSettings = configuration.GetSection("JWTSettings").Get<JWTSettings>();
            AddJwtAuthentication(services, jwtSettings);
            
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<TMDBService>() ;
            
            services.AddHttpClient<TVService>();
            services.AddHttpClient<TMDBService>();
            services.AddTransient<TMDBService>();
        }
    }
}
