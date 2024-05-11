using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TransactionStore.Models.DBModel.TransactionStore;
using TransactionStore.Services.ApiService.Transaction;
using TransactionStore.Services.ViewService.Transaction;

var builder = WebApplication.CreateBuilder(args);

IConfiguration Configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register EF Core
string connStr = Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TransactionStoreEntities>(options =>
            options.UseSqlServer(connStr));

// Register services (DI)
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ITransactionApiService, TransactionApiService>();

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

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapAreaControllerRoute(
        name: "Api",
        areaName: "api",
        pattern: "api/v1/{controller}/{action}/{id?}",
        defaults: new { area = "api" }
   );
});

app.Run();
