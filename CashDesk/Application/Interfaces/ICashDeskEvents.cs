namespace CashDesk.Application.Interfaces;

public interface ICashDeskEvents
{
    /// <summary>
    /// Invoked when clicked <c> "Start Sale" </c> at cashbox terminal
    /// </summary>
    public event EventHandler? StartSale;

    /// <summary>
    /// Invoked when clicked <c> "Finish Sale" </c> at cashbox terminal
    /// </summary>
    public event EventHandler? FinishSale;

    /// <summary>
    /// Invoked when clicked <c> "Pay With Card" </c> at cashbox terminal
    /// </summary>
    public event EventHandler? PayWithCard;

    /// <summary>
    /// Invoked when clicked <c> "Pay With Cash" </c> at cashbox terminal
    /// </summary>
    public event EventHandler? PayWithCash;

    /// <summary>
    /// Invoked when clicked <c> "Disable Express Mode" </c> at cashbox terminal
    /// </summary>
    public event EventHandler? DisableExpressMode;

    /// <summary>
    /// Invoked when a new barcode is entered
    /// </summary>
    public event EventHandler<string>? AddItemToSale;

    /// <summary>
    /// This method is used to create a loop where the cashbox buttons are listened to and handled upon clicking
    /// </summary>
    Task StartListeningToTerminal();
}