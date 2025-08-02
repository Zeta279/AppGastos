using AppGastos.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add API Controller support
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        });

// Database context
builder.Services.AddDbContext<AppGastosContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
