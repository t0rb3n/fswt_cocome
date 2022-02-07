namespace Application.Enterprise;

public class ReportDTO
{
    protected string reportText = "";

    public string ReportText
    {
        get => reportText;
        set => reportText = value ?? throw new ArgumentNullException(nameof(value));
    }
}