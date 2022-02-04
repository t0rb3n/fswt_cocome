using Microsoft.AspNetCore.Mvc;

namespace TradingSystemService.Controllers;

public class EnterpriseServerConfig
{
    public const string KeyValue = "ServerInfo";
    public string EnterpriseName { get; set; }
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
    public EnterpriseServerConfig Get()
    {
        var info = new EnterpriseServerConfig();
        _configuration.GetSection(EnterpriseServerConfig.KeyValue).Bind(info);
        _logger.LogInformation("get ServerConfig");
        return info;
    }
}