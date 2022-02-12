using System.Security.Cryptography;

namespace CashDesk;

public class CashDeskEventHandler : IHostedService
{
    private CashDeskEventPublisher _cdep;
    private readonly CashDesk _cashDesk;

    private readonly ILogger _logger;
    private readonly IHostApplicationLifetime _appLifetime;

    public CashDeskEventHandler(
        ILogger<CashDeskEventHandler> logger,
        IHostApplicationLifetime appLifetime,
        CashDesk cashDesk,
        CashDeskEventPublisher cdep)
    {
        _logger = logger;
        _appLifetime = appLifetime;
        _cashDesk = cashDesk;
        _cdep = cdep;

        RegisterHandler();
    }

    private void RegisterHandler()
    {
        _cdep.StartSale += StartSaleHandler;
        _cdep.FinishSale += FinishSaleHandler;
        _cdep.DisableExpressMode += DisableExpressModeHandler;
        _cdep.PayWithCard += PayWithCardHandler;
        _cdep.PayWithCash += PayWithCashHandler;

        _cdep.AddItemToSale += AddItemToSaleHandler;
    }

    /*
     * Hosted Service Methods
     */

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _appLifetime.ApplicationStarted.Register(() =>
        {
            Task.Run(async () =>
            {
                try
                {
                    await _cdep.StartListeningToTerminal();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unhandled exception!");
                }
                finally
                {
                    {
                        _appLifetime.StopApplication();
                    }
                }
            });
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    
    /*
     *  Handler Methods
     */

    private void StartSaleHandler(object sender, EventArgs e)
    {
        _cashDesk.StartSale();
    }

    private void AddItemToSaleHandler(object sender, string barcode)
    {
        _cashDesk.AddItemToSale(barcode);
    }

    private void FinishSaleHandler(object sender, EventArgs e)
    {
        _cashDesk.FinishSale();
    }

    private void DisableExpressModeHandler(object sender, EventArgs e)
    {
        _cashDesk.DisableExpressMode();
    }

    private void PayWithCardHandler(object sender, EventArgs e)
    {
        _cashDesk.PayWithCard();
    }

    private void PayWithCashHandler(object sender, EventArgs e)
    {
        _cashDesk.PayWithCash();
    }
}