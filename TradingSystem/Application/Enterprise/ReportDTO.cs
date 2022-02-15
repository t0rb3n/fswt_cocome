namespace Application.Enterprise;

/// <summary>
/// Class <c>ReportDTO</c> is used for encapsulating report information in simple text format.
/// </summary>
public class ReportDTO
{
    protected string reportText;

    public ReportDTO()
    {
        reportText = "";
    }

    public string ReportText
    {
        get => reportText;
        set => reportText = value ?? throw new ArgumentNullException(nameof(value));
    }
}
