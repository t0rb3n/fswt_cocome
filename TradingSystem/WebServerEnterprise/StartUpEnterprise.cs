using Application.Enterprise;
using Application.Exceptions;
using Data.Enterprise;
using Data.Exceptions;
using Data.Store;
using WebServerEnterprise.Services;

namespace WebServerEnterprise;
public class StartUpEnterprise
{
    private static void GetEnterpriseInfo(IServiceCollection service, IConfiguration config)
    {
        try
        {
            var app = service.BuildServiceProvider().GetService<EnterpriseApplication>();
            var appInfo = app!.GetEnterprise();
            config["ServerInfo:EnterpriseName"] = appInfo.EnterpriseName;
        }
        catch (DatabaseNotAvailableException e)
        {
            Console.WriteLine(e);
            Environment.Exit(1);
        }
        catch (EnterpriseException e)
        {
            Console.WriteLine(e);
            Environment.Exit(1);
        }
    }
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("The enterprise id is missing!");
            Environment.Exit(1);
        }

        var enterpriseId = Convert.ToInt64(args[0]);
        
        /*const string host = "localhost";
        const string database = "tradingsystem";
        const string username = "dummy";
        const string password = "dummy123";*/
        
        const string host = "ec2-54-155-194-191.eu-west-1.compute.amazonaws.com";
        const string database = "d6v10jgjrtfjnt";
        const string username = "mhxaavrkfwmegj";
        const string password = "fc1cc9bdc3a621aa753d50896e87f00d2420354242cbd92b20331bf6cc1e16a4";
        
        const string connectionString = $"host={host};database={database};username={username};password={password}";

        var builder = WebApplication.CreateBuilder(args);
        
        // Add services to the container.
        builder.Services.AddGrpc();
        builder.Services.AddControllersWithViews();
        builder.Services.AddSingleton<IEnterpriseQuery, EnterpriseQuery>();
        builder.Services.AddSingleton<IStoreQuery, StoreQuery>();
        builder.Services.AddSingleton<EnterpriseApplication>(provider =>
        {
            return new EnterpriseApplication(
                provider.GetRequiredService<IEnterpriseQuery>(),
                provider.GetRequiredService<IStoreQuery>(),
                provider.GetRequiredService<ILogger<EnterpriseApplication>>(),
                enterpriseId,
                connectionString);
        });
        builder.Services.AddSingleton<IEnterpriseApplication>(provider => 
            provider.GetRequiredService<EnterpriseApplication>());
        builder.Services.AddSingleton<IReporting>(provider => 
            provider.GetRequiredService<EnterpriseApplication>());

        GetEnterpriseInfo(builder.Services, builder.Configuration);

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
