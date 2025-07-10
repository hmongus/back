using System.Reflection;
using System.Text;
using System.Text.Json; 
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using workstation_back_end.Bookings.Application.BookingCommandService;
using workstation_back_end.Bookings.Application.BookingQueryService;
using workstation_back_end.Bookings.Domain;
using workstation_back_end.Bookings.Domain.Models.Commands;
using workstation_back_end.Bookings.Domain.Models.Validators;
using workstation_back_end.Bookings.Domain.Services;
using workstation_back_end.Bookings.Infrastructure;
using workstation_back_end.Experience.Application.ExperienceCommandServices;
using workstation_back_end.Experience.Domain;
using workstation_back_end.Experience.Domain.Models.Validators;
using workstation_back_end.Experience.Domain.Services;
using workstation_back_end.Experience.Infraestructure;
using workstation_back_end.Favorites.Application.FavoriteCommandService;
using workstation_back_end.Favorites.Application.FavoriteQueryService;
using workstation_back_end.Favorites.Domain;
using workstation_back_end.Experience.Domain.Models.Queries; 
using workstation_back_end.Favorites.Domain.Models.Validators;
using workstation_back_end.Favorites.Domain.Services;
using workstation_back_end.Favorites.Infrastructure;
using workstation_back_end.Inquiry.Application.CommandServices;
using workstation_back_end.Inquiry.Application.QueryServices;
using workstation_back_end.Inquiry.Domain.Models.Commands;
using workstation_back_end.Inquiry.Domain.Services;
using workstation_back_end.Inquiry.Domain.Services.Models.Validators;
using workstation_back_end.Inquiry.Domain.Services.Services;
using workstation_back_end.Inquiry.Infraestructure;
using workstation_back_end.Reviews.Application.ReviewCommandService;
using workstation_back_end.Reviews.Application.ReviewQueryService;
using workstation_back_end.Reviews.Domain;
using workstation_back_end.Reviews.Domain.Models.Commands;
using workstation_back_end.Reviews.Domain.Models.Validators;
using workstation_back_end.Reviews.Domain.Services;
using workstation_back_end.Reviews.Infrastructure;
using workstation_back_end.Security.Application.SecurityCommandServices;
using workstation_back_end.Security.Application.TokenServices;
using workstation_back_end.Security.Domain.Services;
using workstation_back_end.Shared.Domain.Repositories;
using workstation_back_end.Shared.Infraestructure.Persistence.Configuration;
using workstation_back_end.Shared.Infraestructure.Persistence.Repositories;
using workstation_back_end.Users.Application.CommandServices;
using workstation_back_end.Users.Application.QueryServices;
using workstation_back_end.Users.Domain;
using workstation_back_end.Users.Domain.Models.Validadors;
using workstation_back_end.Users.Domain.Services;
using workstation_back_end.Users.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "TripMatch API",
        Description = "API to manage travel experiences"
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));


    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa tu token aquí con el formato: Bearer {token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter());
}).AddJsonOptions(options =>
{

    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});
// -------------------------------------------------------------------------


// === JWT Auth ===
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = builder.Configuration["Jwt:Key"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!))
        };
    });

builder.Services.AddAuthorization();

// === CORS ===
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("https://trip-match-2025.web.app", 
                    "http://localhost:5173",          
                    "http://localhost:8080")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

// Base de datos
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (connectionString == null)
    throw new Exception("Falta la cadena de conexión a la base de datos.");

builder.Services.AddDbContext<TripMatchContext>(options =>
{
    options.UseMySQL(connectionString)
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors();
});



// Inyección de dependencias (Shared)
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IExperienceRepository, ExperienceRepository>();
builder.Services.AddScoped<IExperienceCommandService, ExperienceCommandService>();
builder.Services.AddScoped<IExperienceQueryService, ExperienceQueryService>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateExperienceCommandValidator>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>();

builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingCommandService, BookingCommandService>();
builder.Services.AddScoped<IBookingQueryService, BookingQueryService>();
builder.Services.AddScoped<IValidator<CreateBookingCommand>, CreateBookingCommandValidator>();

builder.Services.AddScoped<IInquiryCommandService, InquiryCommandService>();
builder.Services.AddScoped<IInquiryQueryService, InquiryQueryService>();
builder.Services.AddScoped<IInquiryRepository, InquiryRepository>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateInquiryCommandValidator>();

builder.Services.AddScoped<IResponseCommandService, ResponseCommandService>();
builder.Services.AddScoped<IResponseQueryService, ResponseQueryService>();
builder.Services.AddScoped<IResponseRepository, ResponseRepository>();
builder.Services.AddScoped<IValidator<CreateResponseCommand>, CreateResponseCommandValidator>();

builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IReviewCommandService, ReviewCommandService>();
builder.Services.AddScoped<IReviewQueryService, ReviewQueryService>();
builder.Services.AddScoped<IValidator<CreateReviewCommand>, CreateReviewCommandValidator>();


builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();
builder.Services.AddScoped<IFavoriteCommandService, FavoriteCommandService>();
builder.Services.AddScoped<IFavoriteQueryService, FavoriteQueryService>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateFavoriteCommandValidator>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
//builder.WebHost.UseUrls("http://localhost:5000");


builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(int.Parse(Environment.GetEnvironmentVariable("PORT") ?? "10000"));
});




// usual middleware and endpoint config here





var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TripMatchContext>();
    context.Database.EnsureCreated();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseCors("_myAllowSpecificOrigins");
app.UseAuthorization();
app.MapControllers();
app.Run();