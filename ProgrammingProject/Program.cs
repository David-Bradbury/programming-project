using Microsoft.EntityFrameworkCore;
using ProgrammingProject.Data;



var builder = WebApplication.CreateBuilder(args);


//Add services to the container
builder.Services.AddDbContext<EasyWalkContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(EasyWalkContext)));
    //Enable lazy loading
    options.UseLazyLoadingProxies();
});


// Store session data (In memory).
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.IsEssential = true;
});




builder.Services.AddControllersWithViews();

var app = builder.Build();

//Seed data.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        SeedData.Initialize(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}


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

app.UseSession();

// Wade(no date) Set X-FRAME-OPTIONS in ASP.NET Core, .NET Core Tutorials.
// Available at: https://dotnetcoretutorials.com/set-x-frame-options-asp-net-core/ (Accessed: 21 May 2023). 
app.Use(async (context, next) =>
	{
		context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
		await next();
	});

// Anuraj (1 Nov 2021) Implementing Content Security Policy (CSP) in ASP.NET Core.
// Available at: https://dotnetthoughts.net/implementing-content-security-policy-in-aspnetcore/ (Accessed: 22 May 2023).
//app.Use(async (context, next) =>
//{
//    context.Response.Headers.Add("Content-Security-Policy", "default-src 'self';");
//    await next();
//});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
