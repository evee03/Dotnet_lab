using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Laboratorium_7.Data;
using Laboratorium_7.Areas.Identity.Data;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDbContext<ChinookDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ChinookConnection")
    ?? throw new InvalidOperationException("Connection string 'ChinookConnection' not found.")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddLocalization();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

var supportedCultures = new[] { new CultureInfo("en-US") };

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = supportedCultures,
    FallBackToParentCultures = false
});

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "orderslist",
    pattern: "orders",
    defaults: new { controller = "Home", action = "MyOrders" }
);

app.MapControllerRoute(
    name: "order",
    pattern: "orders/{id}",
    defaults: new { controller = "Home", action = "OrderDetails" }
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ChinookDbContext>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    if (context.Customers.Any())
    {
        var firstCustomerEmail = context.Customers.OrderBy(x => x.CustomerId).First().Email;

        if (await userManager.FindByEmailAsync(firstCustomerEmail) == null)
        {
            foreach (var item in context.Customers)
            {
                var user = new ApplicationUser
                {
                    UserName = item.Email,
                    NormalizedUserName = item.Email.ToUpper(),
                    Email = item.Email,
                    NormalizedEmail = item.Email.ToUpper(),
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    CustomerId = item.CustomerId
                };

                await userManager.CreateAsync(user, "P@ssw0rd");
            }
        }
    }
}

app.Run();
