using PFA_Gestion_Laureats.Models;
using Microsoft.EntityFrameworkCore;
using PFA_Gestion_Laureats.SignalR;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR(o =>
{
    o.EnableDetailedErrors = true;
});
builder.Services.AddSession(
    opt =>
    {
        opt.IdleTimeout = TimeSpan.FromMinutes(1200);
        //  opt.Cookie.HttpOnly = true;
    }
    );

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<MyContext>(opt =>
{
    opt.UseSqlServer(@"Data Source=.\SQLEXPRESS;Encrypt=false;initial catalog=Gestion_Laureats;Integrated Security = true");
});


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
app.UseSession();
app.UseAuthorization();
app.MapHub<ChatHub>("/chatHub");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}");

app.Run();
