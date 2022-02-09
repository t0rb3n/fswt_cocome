using Application.Store;
using Grpc.AspNetCore.ClientFactory;
using Grpc.Enterprise.V1;
using Grpc.Net.Client;
using GrpcService.Services;

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

        var httpHandler = new HttpClientHandler();
        httpHandler.ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        var channel = GrpcChannel.ForAddress("https://localhost:7046/grpc",
            new GrpcChannelOptions {HttpHandler = httpHandler});
        var client = new EnterpriseService.EnterpriseServiceClient(channel);

        var application = new StoreApplication(Convert.ToInt64(args[0]), client);
        var builder = WebApplication.CreateBuilder(args);

        var store = new StoreEnterpriseDTO();
        try
        {
            store = application.GetStore();
        }
        catch (Grpc.Core.RpcException e)
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
        builder.Services.AddControllersWithViews();
        builder.Services.AddSingleton<IStoreApplication>(application);
        builder.Services.AddSingleton<ICashDeskConnector>(application);

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

        app.MapGrpcService<StoreGrpcService>();
        app.MapGet(
            "/grpc",
            () => "Communication with gRPC endpoints must be made through a gRPC client."
        );

        app.Run();
    }
}