using HDBank.API.Services;
using HDBank.Core.Interfaces;
using HDBank.Core.Mapper;
using HDBank.Core.Services;
using HDBank.Infrastructure.Data;
using HDBank.Infrastructure.Models;
using HDBank.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HDBank API",
        Version = "v1",
        Description = "API for HDBank",
        Contact = new OpenApiContact
        {
            Url = new Uri("https://google.com")
        }
    });
    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    options.AddSecurityDefinition("Bearer", securitySchema);

    var securityRequirement = new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { "Bearer" } }
                };

    options.AddSecurityRequirement(securityRequirement);
});
builder.Services.AddDbContext<HDBankDbContext>(o =>
{
    var connectionString = builder.Configuration.GetConnectionString("API");
    o.UseSqlServer(connectionString);
});

builder.Services.AddIdentity<AppUser, IdentityRole>(o =>
{
    o.SignIn.RequireConfirmedPhoneNumber = false;
    o.SignIn.RequireConfirmedAccount = false;
    o.SignIn.RequireConfirmedEmail = false;
    // User Settings
    //o.User.RequireUniqueEmail = false;
    // Password Settings
    o.Password.RequireNonAlphanumeric = false;
    o.Password.RequireUppercase = false;
    o.Password.RequireDigit = false;
    o.Password.RequiredLength = 0;
    o.Password.RequireLowercase = false;
    o.Password.RequiredUniqueChars = 0;
})
    .AddEntityFrameworkStores<HDBankDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddHttpClient("HDBank", httpClient =>
{
    var baseAddress = builder.Configuration["BaseUrl:HDBank"];
    httpClient.BaseAddress = new Uri(baseAddress);
    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("x-api-key", "hutech_hackathon@123456");
    httpClient.DefaultRequestHeaders.Add("x-api-key", "hutech_hackathon@123456");
});
builder.Services.AddHttpClient("Refresh", httpClient =>
{
    var baseAddress = builder.Configuration["BaseUrl:Refresh"];
    httpClient.BaseAddress = new Uri(baseAddress);
});

builder.Services.AddAutoMapper(typeof(MapperProfile));

builder.Services.AddScoped<IAppService, AppService>();
builder.Services.AddScoped<IAPIService, APIService>();
builder.Services.AddSingleton<IJwtManager, JwtManager>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}
app.UseSwagger();
app.UseSwaggerUI(o => {

    o.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
    o.RoutePrefix = string.Empty;
});
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
