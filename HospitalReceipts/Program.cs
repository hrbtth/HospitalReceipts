using HospitalReceipts.Data;
using HospitalReceipts.Models;
using HospitalReceipts.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



// --- Database setup ---
var dbPath = Path.Combine(builder.Environment.ContentRootPath, "Data", "hospital_receipts.db");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

// --- Add services ---
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<AuthService>();
// Use Singleton (or Scoped if you want per-session with Server-side Blazor)
//builder.Services.AddSingleton<AuthService>();

// Force kestrel to use fixed port
//builder.WebHost.UseUrls("http://localhost:5000");

var app = builder.Build();

// --- Configure the HTTP request pipeline ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

//  Add seeding logic for Admin user
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (!db.Users.Any())
    {
        db.Users.Add(new AppUser
        {
            UserName = "Admin",
            Password = "", // blank password
            Privilege = "ADMIN"
        });
        db.SaveChanges();
    }
}


app.Run();
