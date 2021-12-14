using GeekShopping.Web.Services;
using GeekShopping.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient<IProductService, ProductService>(op => op.BaseAddress = new Uri("https://localhost:4440"));
builder.Services.AddHttpClient<ICartService, CartService>(op => op.BaseAddress = new Uri("https://localhost:4445"));
//builder.Services.AddHttpClient<ICouponService, CouponService>(op => op.BaseAddress = new Uri("https://localhost:4450"));
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc"; //Open Id Connection
}).AddCookie("Cookies", config => config.ExpireTimeSpan = TimeSpan.FromMinutes(10))
.AddOpenIdConnect("oidc", options =>
{
    options.Authority = "https://localhost:4435";
    options.GetClaimsFromUserInfoEndpoint = true;
    options.ClientId = "geek_shopping"; //ApiScope genérico da classe IdentityConfiguration
    options.ClientSecret = "my_super_secret"; //Configurado também no IdentityConfiguration
    options.ResponseType = "code";
    options.ClaimActions.MapJsonKey("role", "role", "role");
    options.ClaimActions.MapJsonKey("sub", "sub", "sub");
    options.TokenValidationParameters.NameClaimType = "name";
    options.TokenValidationParameters.RoleClaimType = "role";
    options.Scope.Add("geek_shopping"); //Do ApiScope também
    options.SaveTokens = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
