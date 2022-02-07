using Application.Enterprise;

namespace Application.Store;

public class StoreEnterpriseDTO : StoreDTO
{
    protected EnterpriseDTO enterprise;

    public EnterpriseDTO Enterprise
    {
        get => enterprise;
        set => enterprise = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    public override string ToString()
    {
        return $"Id: {storeId}, Name: {storeName}, Loc: {location}, Enterprise: {enterprise.EnterpriseName}";
    }
}