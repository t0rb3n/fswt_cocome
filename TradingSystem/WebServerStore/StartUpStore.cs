using Store.Application;

namespace WebServerStore;

public class StartUpStore
{
    public static void Main(string[] args)
    {
        Console.WriteLine("StartUp Arguments:");
        foreach (var arg in args)
        {
            Console.WriteLine($"\t{arg}");
        }
        
        var application = new StoreApplication(Convert.ToInt64(args[0]));
        var builder = WebApplication.CreateBuilder(args);

        var store = application.GetStore();
        builder.Configuration["ServerInfo:EnterpriseName"] = store.Enterprise.EnterpriseName;
        builder.Configuration["ServerInfo:StoreName"] = store.StoreName;
        builder.Configuration["ServerInfo:StoreLocation"] = store.Location;
        
        // Add services to the container.
        builder.Services.AddControllersWithViews().AddNewtonsoftJson();
        builder.Services.AddSingleton<IStoreApplication>(application);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();


        app.MapControllerRoute(
            name: "default",
            pattern: "{controller}/{action=Index}/{id?}");

        app.MapFallbackToFile("index.html");

        app.Run();
    }
}
