using Application.Enterprise;
using Data.Store;
using Microsoft.AspNetCore.Mvc;

namespace WebServerEnterprise.Controllers;

[ApiController]
[Route("[controller]")]

public class EnterpriseReportController : ControllerBase
{
    private readonly ILogger<EnterpriseReportController> _logger;
    private readonly IEnterpriseApplication _enterpriseApp;
    private readonly IReporting _enterpriseReporting;

    public EnterpriseReportController(ILogger<EnterpriseReportController> logger, IEnterpriseApplication enterpriseApp,
        IReporting enterpriseReporting)
    {
        _logger = logger;
        _enterpriseApp = enterpriseApp;
        _enterpriseReporting = enterpriseReporting;
    }

    [HttpGet("/report")]
    public IActionResult GetReport()
    {
        var enterprise = _enterpriseApp.GetEnterprise();

        return Ok(_enterpriseReporting.GetEnterpriseStockReport(enterprise.EnterpriseId));
    }

    [HttpGet("/statistics")]

    public IActionResult GetStatistics()
    {
        var enterprise = _enterpriseApp.GetEnterprise();
        return Ok(_enterpriseReporting.GetMeanTimeToDeliveryReport(enterprise.EnterpriseId));
    }

}