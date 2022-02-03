using System.Transactions;
using Tecan.Sila2.Discovery;

namespace CashDesk;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // adds a GRPC service to be used for injection
        services.AddGrpcClient<EnterpriseRpc.EnterpriseRpcClient>(options =>
        {
            options.Address = new Uri("https://localhost:5001");
        });
        
        //services.AddHostedService()
    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
    }
}