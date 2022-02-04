namespace Enterprise.Application;

public class StoreDTO
{
    protected long storeId;
    protected string storeName = "";
    protected string location = "";

    public long StoreId
    {
        get => storeId;
        set => storeId = value;
    }

    public string StoreName
    {
        get => storeName;
        set => storeName = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Location
    {
        get => location;
        set => location = value ?? throw new ArgumentNullException(nameof(value));
    }

    public override string ToString()
    {
        return $"Id: {storeId}, Store: {storeName}, Location: {location}";
    }
}