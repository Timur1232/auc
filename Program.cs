using Microsoft.EntityFrameworkCore;
using App;
using App.Services;
using App.Models;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<AuctionDbContext>(opt => {
    var conn_string = config.GetConnectionString("db_conn");
    if (builder.Environment.IsDevelopment()) {
        opt.UseSqlite(conn_string);
    } else {
        G.Todo("Add server db (like postgres or mysql) for production");
    }
});

builder.Services.AddScoped<JwtTokenProvider>();
builder.Services.AddScoped<PasswordHasher>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:8080")
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseStaticFiles();

app.MapControllers();

app.Run();
