using Hospital_Management.Extantions;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbServices(builder.Configuration);
builder.Services.AddIdentityDbServices();

var app = builder.Build();

app.ContextInitalize();

app.UseRouting();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.Use(async (context, next) =>
{
    if (!context.User.Identity.IsAuthenticated &&
        !context.Request.Path.StartsWithSegments("/account/login"))
    {
        context.Response.Redirect("/Account/Login");
        return;
    }

    await next();
});


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "/{controller=Home}/{action=Index}/{id?}"
    );
});

app.Run();