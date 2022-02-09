using Application.Enterprise;
using GrpcService.Services;

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
        
        var builder = WebApplication.CreateBuilder(args); 
        var application = new EnterpriseApplication(Convert.ToInt64(args[0]));
        
        var enterprise = application.GetEnterprise();
        builder.Configuration["ServerInfo:EnterpriseName"] = enterprise.EnterpriseName;
        
        // Add services to the container.
        builder.Services.AddGrpc();
        builder.Services.AddSingleton<IEnterpriseApplication>(application);
        builder.Services.AddControllersWithViews();

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
        app.MapGrpcService<EnterpriseGrpcService>();
        app.MapGet(
            "/grpc",
            () => "Communication with gRPC endpoints must be made through a gRPC client."
        );

        app.Run();
    }
}
