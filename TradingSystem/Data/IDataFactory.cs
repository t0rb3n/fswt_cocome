namespace Data;

/// <summary>
/// Factory for creating the Data component
/// </summary>
public class IDataFactory
{
    private static IData _dataaccess = null!;

    /// <summary>
    /// Creates a data component if none has been created yet
    /// </summary>
    /// <returns>a Data component</returns>
    public static IData GetInstance()
    {
        if (_dataaccess == null)
        {
            _dataaccess = new Data();
        }
        return _dataaccess;
    }
}