namespace Data;

public class IDataFactory
{
    private static IData _dataaccess = null!;

    public static IData GetInstance()
    {
        if (_dataaccess == null)
        {
            _dataaccess = new Data();
        }

        return _dataaccess;
    }
}