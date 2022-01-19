namespace TradingSystem.inventory.data;

public class IDataFactory
{
    private static IData dataaccess = null;
    
    public IDataFactory() {}

    public static IData getInstance()
    {
        if (dataaccess == null)
        {
            dataaccess = new Data();
        }

        return dataaccess;
    }
}