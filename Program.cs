using backend.Data;
using backend.Services.UserService;
using backend.Services.CarService;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer; // Allow use to protect a route.
using Microsoft.OpenApi.Models; // To use OpenApiSecurityScheme (Configure swagger so it can send jwt during test)
using Swashbuckle.AspNetCore.Filters;
//using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Connect to db
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers().AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddSwaggerGen(); // Configure Swager without sending Jwt

// Configure Swagger to Send Jwt if Needed
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

// Authenticate User Token
builder.Services.AddAuthentication().AddCookie(options =>
{
    // letting the app know that cookies ar involes in our request au authentication
    options.Cookie.Name = "Token";
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true, // checking only the siganture key
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value!))
    };
    // check if a request have the cookies "Token" , then take it to authenticate any route that
    // required authentication in our controller.
    // when we use the Authorise attribute on a route in our controller
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["Token"];
            return Task.CompletedTask;
        }
    };
});


// Allow our web api to share data(and Cookies) with the frontend: Configuring CORS (Cross-Origin Resource Sharing)
builder.Services.AddCors(p => p.AddPolicy("corspolicy", build =>
{
    //build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
    build.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
}));

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly); // Register Automapper

// Register our the Service Class the our Interface is uisng
builder.Services.AddScoped<IUserService, UserService>(); // Use this for testing: Everytime we use the interface IUserService it will use an instance of UserService class
// builder.Services.AddScoped<IUserService, ProdUserService>(); // Use this for Production: Everytime we use the interface IUserService it will use an instance of UserService class

builder.Services.AddScoped<ICarService, CarService>(); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("corspolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

