using AutoMapper;
using TestTask.Services;
using TestTask.ServicesContracts;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddServices();
builder.Services.AddControllers();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
      app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    { 
        
        serviceCollection.AddAutoMapper(typeof(Program));
        serviceCollection.AddTransient<IUploadService, UploadService>();

        return serviceCollection;
    }
}