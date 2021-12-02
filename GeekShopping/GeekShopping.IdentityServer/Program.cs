using GeekShopping.IdentityServer.Configuration;
using GeekShopping.IdentityServer.Initializer;
using GeekShopping.IdentityServer.Model;
using GeekShopping.IdentityServer.Model.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connection = "Server=localhost;DataBase=geek_shopping_identity_server;Uid=root;Pwd=#Helpdesk2013;Port=3306";

builder.Services.AddDbContext<MySQLContext>(op => op
    .UseMySql(connection,
    new MySqlServerVersion(
        new Version(8, 0, 25)
    )));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<MySQLContext>()
    .AddDefaultTokenProviders();

var builderConfiguration = builder.Services.AddIdentityServer(op =>
{
    op.Events.RaiseErrorEvents = true;
    op.Events.RaiseInformationEvents = true;
    op.Events.RaiseFailureEvents = true;
    op.Events.RaiseSuccessEvents = true;
    op.EmitStaticAudienceClaim = true;
}).AddInMemoryIdentityResources(IdentityConfiguration.IdentityResources)
.AddInMemoryApiScopes(IdentityConfiguration.ApiScopes)
.AddInMemoryClients(IdentityConfiguration.Clients)
.AddAspNetIdentity<ApplicationUser>();

builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builderConfiguration.AddDeveloperSigningCredential();

var app = builder.Build();

var scope = app.Services.CreateScope();
var dbInitializer = scope.ServiceProvider.GetService<IDbInitializer>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();
dbInitializer.Initialize();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
