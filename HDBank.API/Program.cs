using HDBank.Core.Interfaces;
using HDBank.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient("HDBank", httpClient =>
{
    var baseAddress = builder.Configuration["BaseUrl:HDBank"];
    httpClient.BaseAddress = new Uri(baseAddress);
    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("x-api-key", "hutech_hackathon@123456");
    httpClient.DefaultRequestHeaders.Add("x-api-key", "hutech_hackathon@123456");
});
builder.Services.AddScoped<IAPIService, APIService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
