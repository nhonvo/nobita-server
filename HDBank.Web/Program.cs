using HDBank.Core.Interfaces;
using HDBank.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
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
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
