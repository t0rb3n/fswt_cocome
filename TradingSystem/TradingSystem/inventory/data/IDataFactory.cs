namespace TradingSystem.inventory.data;

public class IDataFactory
{
    private static IData _dataaccess;

    public static IData GetInstance()
    {
        if (_dataaccess == null)
        {
            _dataaccess = new Data();
        }

        return _dataaccess;
    }
}