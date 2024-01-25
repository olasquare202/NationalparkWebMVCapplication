using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NationalParkAPI;
using NationalParkAPI.Data;
using NationalParkAPI.ParkyMapper;
using NationalParkAPI.Repository;
using NationalParkAPI.Repository.IRepository;
using System.Reflection;
//using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
//Add Connection String
var configuration = builder.Configuration;
//How to configure cors
//builder.Services.AddCors("CORSPOLICY", builder => builder.AllowAnyMethod().AllowAnyHeader().AllowCredential);
var MyAllowCors = "Cors";
builder.Services.AddCors(options =>
{
    options.AddPolicy("CORSPOLICY", options => options.AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed((hosts) => true));
    options.AddPolicy(name: MyAllowCors, policy => { policy.WithOrigins("https://localhost:7262", "http://localhost:5226").AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed((hosts) => true); });

});
builder.Services.AddDbContext<ApplicationDbContext>(options
    => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

//Register the Repository Services
builder.Services.AddScoped<INationalParkRepository, NationalParkRepository>();
builder.Services.AddScoped<ITrailRepository, TrailRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddAutoMapper(typeof(ParkyMappings));
builder.Services.AddApiVersioning(options => 
{
    options.AssumeDefaultVersionWhenUnspecified = true;//if you dont specify your API version, it will load default version for you
    options.DefaultApiVersion = new ApiVersion(1, 0);//Current Version
    options.ReportApiVersions = true;//this report what is the current Api version
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));
//builder.Services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
var appSettingsSection = configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
var appSettings = appSettingsSection.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appSettings.Secret);

builder.Services.AddAuthentication(j =>
{
    j.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    j.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    j.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            //ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            //ValidAudience = builder.Configuration["JWT:ValidAudience"],
            //ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
            // IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Secret"]))
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

//.AddJwtBearer(b =>
//{
//    b.RequireHttpsMetadata = false;
//    b.SaveToken = true;
//    b.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
//    {
//        ValidateIssuerSigningKey = true,
//        //IssuerSigningKey = new SymmetricSecurityKey(key)
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Secret"]))
//    };
//});

//configure d middleware here "SwaggerGenerator" using Swashbukles
builder.Services.AddSwaggerGen(options =>
{
    //Add Swagger documentation here i.e open API specification
    options.SwaggerDoc("NationalParkOpenAPISpecNP",
        new Microsoft.OpenApi.Models.OpenApiInfo()
        {
            Title = "NationalPark API",
            Version = "v1",
            Description = "Best Practices Method of writing code",
            Contact = new Microsoft.OpenApi.Models.OpenApiContact()
            {
                Email = "olaoluwaesan.dev@gmail.com",
                Name = "Check My Website",
                Url = new Uri("https://olasquare202.github.io/Boostrap-V5-Project-with-SASS/")
            },
            License = new Microsoft.OpenApi.Models.OpenApiLicense()
            {
                Name = "MIT LICENSE",
                Url = new Uri("https://en.wikipedia.org/wiki/MIT License")
            }
        });





    options.SwaggerDoc("NationalParkOpenAPISpecTrails",
        new Microsoft.OpenApi.Models.OpenApiInfo()
        {
            Title = "NationalPark API Trails",
            Version = "v2",
            Description = "Best Practices Method of writing code",
            Contact = new Microsoft.OpenApi.Models.OpenApiContact()
            {
                Email = "olaoluwaesan.dev@gmail.com",
                Name = "Check My Website",
                Url = new Uri("https://olasquare202.github.io/Boostrap-V5-Project-with-SASS/")
            },
            License = new Microsoft.OpenApi.Models.OpenApiLicense()
            {
                Name = "MIT LICENSE",
                Url = new Uri("https://en.wikipedia.org/wiki/MIT License")
            }
        });
var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
var cmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
options.IncludeXmlComments(cmlCommentsFullPath);
});



builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Application name", Version = "v1", Description = "" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        //follow these steps in Authorization Swagger UI
        Description = "Enter 'Bearer' give space then paste the bearer Token.\r\n\r\n" +
        "Example: \"Bearer eyjoo354bdjkhvq83esno\""
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new String[]{ }
     }
});
});
//Add another Swagger documentation here i.e open API specification(i.e multiple Swagger document)


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //add Swashbukle to d request pipeline
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        //configure middleware
        options.SwaggerEndpoint("/swagger/NationalParkOpenAPISpecNP/swagger.json", "NationalPark API");
        options.SwaggerEndpoint("/swagger/NationalParkOpenAPISpecTrails/swagger.json", "Trails API");
        options.RoutePrefix = "";
    });
   
}

app.UseApiVersioning();
app.UseHttpsRedirection();
app.UseCors(MyAllowCors);
app.UseCors("CORSPOLICY");
//Note: Another way to configure Cors
//app.UseCors(x => x
    //.AllowAnyOrigin()
    //.AllowAnyMethod()
    //.AllowAnyHeader();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
