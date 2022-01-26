namespace TradingSystem.inventory.application.enterprise;

public class ReportDTO
{
    protected string reportText = "";

    public string ReportText
    {
        get => reportText;
        set => reportText = value ?? throw new ArgumentNullException(nameof(value));
    }
}