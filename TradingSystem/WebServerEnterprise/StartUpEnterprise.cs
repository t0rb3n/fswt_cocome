using Application.Enterprise;
using Data.Enterprise;
using Data.Exceptions;
using Data.Store;
using WebServerEnterprise.Services;

namespace WebServerEnterprise;
public class StartUpEnterprise
{
    public static void Main(string[] args)
    {
        Console.WriteLine("StartUp Arguments:");
        foreach (var arg in args)
        {
            Console.WriteLine($"\t{arg}");
        }

        var enterpriseId = Convert.ToInt64(args[0]);

        var enterpriseLogger = LoggerFactory.Create(options => 
        {
            options.AddConsole();
            options.AddDebug();
        }).CreateLogger<EnterpriseApplication>();
        
        /*const string host = "localhost";
        const string database = "tradingsystem";
        const string username = "dummy";
        const string password = "dummy123";*/
        
        const string host = "ec2-54-155-194-191.eu-west-1.compute.amazonaws.com";
        const string database = "d6v10jgjrtfjnt";
        const string username = "mhxaavrkfwmegj";
        const string password = "fc1cc9bdc3a621aa753d50896e87f00d2420354242cbd92b20331bf6cc1e16a4";
        
        const string connectionString = $"host={host};database={database};username={username};password={password}";
        
        var application = new EnterpriseApplication
        (
            new EnterpriseQuery(), 
            new StoreQuery(), 
            enterpriseLogger, 
            enterpriseId, 
            connectionString
            );
        
        var builder = WebApplication.CreateBuilder(args); 
        
        try
        {
            var enterprise = application.GetEnterprise();
            builder.Configuration["ServerInfo:EnterpriseName"] = enterprise.EnterpriseName;
        }
        catch (DatabaseNotAvailableException e)
        {
            Console.WriteLine(e);
            Environment.Exit(0);
        }

        // Add services to the container.
        builder.Services.AddGrpc();
        builder.Services.AddSingleton<IEnterpriseApplication>(application);
        builder.Services.AddSingleton<IReporting>(application);
        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            // The default HSTS value is 30 days.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller}/{action=Index}/{id?}");

        app.MapFallbackToFile("index.html");
        
        // Add service for enterprise grpc server
        app.MapGrpcService<EnterpriseGrpcService>();
        app.MapGet(
            "/grpc",
            () => "Communication with gRPC endpoints must be made through a gRPC client."
        );
        
        // start asp.net enterprise web server
        app.Run();
    }
}
