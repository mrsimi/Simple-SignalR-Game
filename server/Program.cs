using server.Models;
using Microsoft.EntityFrameworkCore;
using server.Hubs;

var builder = WebApplication.CreateBuilder(args);

string allowedUrl = builder.Configuration["AllowUrls"];
string connectionStrings = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(op => op.UseSqlite(connectionStrings), ServiceLifetime.Scoped);

// Add services to the container.
builder.Services.AddScoped<AppManager>();
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddCors(o => o.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithOrigins(allowedUrl);
    })
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//app.UseAuthorization();

app.UseCors("AllowAllOrigins");


// app.MapRazorPages();
// app.MapHub<GameHub>("/gameHub");
app.UseEndpoints(endpoints => {
    endpoints.MapHub<GameHub>("/gamehub");
});

app.Run();
