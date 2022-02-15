namespace Application.Enterprise;

/// <summary>
/// Class <c>StoreDTO</c> is used as a data transfer object for transferring data from the database.
/// It is used in the Enterprise and Store application to handle the business logic.
/// A DTO can be either a copy of the persisted data for further processing, or
/// to modify or add new data in the persistence layer.
/// </summary>
public class StoreDTO
{
    protected long storeId;
    protected string storeName;
    protected string location;

    /// <summary>
    /// This constructor initializes the new StoreDTO with default values.
    /// <para>StoreDTO objects with Id = -1 means that it does not contain any data.</para>
    /// </summary>
    public StoreDTO()
    {
        storeId = -1;
        storeName = "";
        location = "";
    }

    /// <summary>
    /// Provides get and set methods for StoreId property.
    /// </summary>
    /// <value>Property <c>StoreId</c> represents a unique identifier for Store objects.</value>
    public long StoreId
    {
        get => storeId;
        set => storeId = value;
    }

    /// <summary>
    /// Provides get and set methods for StoreName property.
    /// </summary>
    /// <value>Property <c>StoreName</c> represents the name of the Store objects.</value>
    /// <exception cref="ArgumentNullException">If set StoreName with null.</exception>
    public string StoreName
    {
        get => storeName;
        set => storeName = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Provides get and set methods for Location property.
    /// </summary>
    /// <value>Property <c>Location</c> represents the location of the Store objects.</value>
    /// <exception cref="ArgumentNullException">If set Location with null.</exception>
    public string Location
    {
        get => location;
        set => location = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Reports a StoreDTO properties as a string.
    /// </summary>
    /// <returns>A string with the store properties storeId, storeName and location.</returns>
    public override string ToString()
    {
        return $"Id: {storeId}, Store: {storeName}, Location: {location}";
    }
}
