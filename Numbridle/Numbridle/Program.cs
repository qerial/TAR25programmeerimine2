using Microsoft.EntityFrameworkCore;
using Numbridle.Data;
using Numbridle.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddDbContext<GameDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("GameDatabase")));

builder.Services.AddScoped<IGameService, GameService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();

app.Run();