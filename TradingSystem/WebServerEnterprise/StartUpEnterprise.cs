using Application.Enterprise;
using Data.Exceptions;
using WebServerEnterprise.Services;

namespace WebServerEnterprise;
//TODO clean up
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
        
        var builder = WebApplication.CreateBuilder(args); 
        var application = new EnterpriseApplication(enterpriseId);

        var enterprise = new EnterpriseDTO();
        
        try
        {
            enterprise = application.GetEnterprise();
        }
        catch (DatabaseNotAvailableException e)
        {
            Console.WriteLine(e);
            Environment.Exit(0);
        }
        
        builder.Configuration["ServerInfo:EnterpriseName"] = enterprise.EnterpriseName;
        
        // Add services to the container.
        builder.Services.AddGrpc();
        builder.Services.AddSingleton<IEnterpriseApplication>(application);
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
