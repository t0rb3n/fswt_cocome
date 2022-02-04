using Enterprise.Application;

namespace Enterprise;

public static class ConvertEntryObject
{
    public static EnterpriseDTO ToEnterpriseDTO(Data.Enterprise.Enterprise enterprise)
    {
        EnterpriseDTO result = new();
        result.EnterpriseId = enterprise.Id;
        result.EnterpriseName = enterprise.Name;
        return result;
    }
}