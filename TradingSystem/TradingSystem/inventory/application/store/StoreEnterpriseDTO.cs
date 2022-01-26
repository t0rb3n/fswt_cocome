using TradingSystem.inventory.application.enterprise;

namespace TradingSystem.inventory.application.store;

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