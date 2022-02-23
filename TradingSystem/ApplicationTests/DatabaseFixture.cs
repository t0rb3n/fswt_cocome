using System;
using System.IO;
using System.Net.Http;
using Application.Enterprise;
using Application.Store;
using Data;
using Grpc.Net.Client;
using GrpcModule.Services.Enterprise;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebServerEnterprise.Services;
using Xunit;

namespace ApplicationTests;

public class DatabaseFixture : IDisposable
{
    private const string ConnectionString = 
        "host=localhost:5433;database=test;username=dummy;password=dummy123";
    private readonly WebApplication _webApplication;
  
    public DatabaseFixture()
    {
        Context = new DatabaseContext(ConnectionString);
        
        Context.Database.EnsureDeleted();
        Context.Database.EnsureCreated();
        
        var testData = Path.GetFullPath(
            Path.Combine(
                AppContext.BaseDirectory, 
                "../../../../../database/db_content.sql")
        );
        
        // Inserts all test data from db_content.sql
        Context.Database.ExecuteSqlRaw(File.ReadAllText(testData));

        EnterpriseApplication = new EnterpriseApplication(1);
        EnterpriseApplicationFailure = new EnterpriseApplication(-1);

        // Creates Grpc enterprise service test server.
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseUrls("https://localhost:3000");
        builder.Services.AddSingleton<IEnterpriseApplication>(EnterpriseApplication);
        builder.Services.AddGrpc();
    
        _webApplication = builder.Build();
        _webApplication.UseHttpsRedirection();
        _webApplication.UseRouting();
        _webApplication.MapGrpcService<EnterpriseGrpcService>();
        _webApplication.MapGet(
            "/",
            () => "Communication with gRPC endpoints must be made through a gRPC client."
        );
        _webApplication.StartAsync();

        var httpHandler = new HttpClientHandler();
        httpHandler.ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        var channel = GrpcChannel.ForAddress("https://localhost:3000/", new GrpcChannelOptions{HttpHandler = httpHandler});
        var client = new EnterpriseService.EnterpriseServiceClient(channel);
        
        StoreApplication = new StoreApplication(client, 1);
        StoreApplicationFailure = new StoreApplication(client, -1);
    }

    public void Dispose()
    {
        Context.Database.CloseConnection();
        Context.Dispose();
        _webApplication.StopAsync();
        _webApplication.DisposeAsync();
    }

    public DatabaseContext Context { get; }
    public EnterpriseApplication EnterpriseApplication { get; }
    public EnterpriseApplication EnterpriseApplicationFailure { get; }
    public StoreApplication StoreApplication { get; }
    public StoreApplication StoreApplicationFailure { get; }
}

[CollectionDefinition("ApplicationTestCollection")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture> {}