using Microsoft.AspNetCore.Mvc;

namespace WebServerStore.Controllers;

public class StoreServerConfig
{
    public const string KeyValue = "ServerInfo";
    public string EnterpriseName { get; set; }
    public string StoreName { get; set; }
    public string StoreLocation { get; set; }
}

[ApiController]
[Route("config")]
public class ServerConfigController : ControllerBase
{
    private readonly ILogger<ServerConfigController> _logger;
    private readonly IConfiguration _configuration;

    public ServerConfigController(ILogger<ServerConfigController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }
    
    [HttpGet]
    public StoreServerConfig Get()
    {
        var info = new StoreServerConfig();
        _configuration.GetSection(StoreServerConfig.KeyValue).Bind(info);
        _logger.LogInformation("get ServerConfig");
        return info;
    }
}