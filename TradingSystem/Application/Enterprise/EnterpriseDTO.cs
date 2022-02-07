namespace Application.Enterprise;

public class EnterpriseDTO
{
    protected long enterpriseId;
    protected string enterpriseName = "";

    public long EnterpriseId
    {
        get => enterpriseId;
        set => enterpriseId = value;
    }

    public string EnterpriseName
    {
        get => enterpriseName;
        set => enterpriseName = value ?? throw new ArgumentNullException(nameof(value));
    }

    public override string ToString()
    {
        return $"Id: {enterpriseId}, Enterprise: {enterpriseName}";
    }
}