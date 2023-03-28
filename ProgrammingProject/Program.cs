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


builder.Services.AddControllersWithViews();

var app = builder.Build();

//Seed data.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        //SeedData.Initialize(services);
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
