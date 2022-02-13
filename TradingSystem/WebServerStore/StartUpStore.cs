using Application.Store;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcModule.Services.Enterprise;
using WebServerStore.Services;

namespace WebServerStore;
//TODO clean up
public class StartUpStore
{
    public static void Main(string[] args)
    {
        Console.WriteLine("StartUp Arguments:");
        foreach (var arg in args)
        {
            Console.WriteLine($"\t{arg}");
        }

        var storeId = Convert.ToInt64(args[0]);

        var httpHandler = new HttpClientHandler();
        httpHandler.ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        
        var client = new EnterpriseService.EnterpriseServiceClient(
            GrpcChannel.ForAddress("https://localhost:7046/grpc",
            new GrpcChannelOptions {HttpHandler = httpHandler}));

        var application = new StoreApplication(client, storeId);
        var builder = WebApplication.CreateBuilder(args);

        var store = new StoreEnterpriseDTO();
        try
        {
            store = application.GetStore();
        }
        catch (RpcException e)
        {
            Console.WriteLine(
                $"{e.GetType()}: Grpc client can't connect to EnterpriseService. Please make sure that WebServerEnterprise is running");
            Environment.Exit(0);
        }

        builder.Configuration["ServerInfo:EnterpriseName"] = store.Enterprise.EnterpriseName;
        builder.Configuration["ServerInfo:StoreName"] = store.StoreName;
        builder.Configuration["ServerInfo:StoreLocation"] = store.Location;

        // Add services to the container.
        builder.Services.AddGrpc();
        builder.Services.AddControllersWithViews().AddNewtonsoftJson();
        builder.Services.AddSingleton<IStoreApplication>(application);
        builder.Services.AddSingleton<ICashDeskConnector>(application);

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