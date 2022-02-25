using Application.Exceptions;
using Application.Store;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcModule.Services.Enterprise;
using WebServerStore.Services;

namespace WebServerStore;
public class StartUpStore
{
    private static void GetStoreInfo(IServiceCollection service, IConfiguration config)
    {
        try
        {
            var app = service.BuildServiceProvider().GetService<StoreApplication>();
            var storeInfo = app!.GetStore();
            config["ServerInfo:EnterpriseName"] = storeInfo.Enterprise.EnterpriseName;
            config["ServerInfo:StoreName"] = storeInfo.StoreName;
            config["ServerInfo:StoreLocation"] = storeInfo.Location;
        }
        catch (StoreException e)
        {
            Console.WriteLine(e);
            Environment.Exit(1);
        }
    }
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("The store id is missing!");
            Environment.Exit(1);
        }

        var storeId = Convert.ToInt64(args[0]);
        
        var builder = WebApplication.CreateBuilder(args);
        
        // Add services to the container.
        builder.Services.AddGrpc();
        builder.Services.AddControllersWithViews().AddNewtonsoftJson();
        
        builder.Services.AddGrpcClient<EnterpriseService.EnterpriseServiceClient>(options =>
        {
            options.Address = new Uri("https://localhost:7046/grpc");
        }).ConfigurePrimaryHttpMessageHandler(() =>
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            return handler;
        });
        
        builder.Services.AddSingleton<StoreApplication>(provider => new StoreApplication(
            provider.GetRequiredService<ILogger<StoreApplication>>(),
            provider.GetRequiredService<EnterpriseService.EnterpriseServiceClient>(),
            storeId));
        
        builder.Services.AddSingleton<IStoreApplication>(provider => 
            provider.GetRequiredService<StoreApplication>());
        builder.Services.AddSingleton<ICashDeskConnector>(provider => 
            provider.GetRequiredService<StoreApplication>());

        GetStoreInfo(builder.Services, builder.Configuration);
        
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

        // Add service for store grpc server
        app.MapGrpcService<StoreGrpcService>();
        app.MapGet(
            "/grpc",
            () => "Communication with gRPC endpoints must be made through a gRPC client."
        );

        // start asp.net store web server
        app.Run();
    }
}