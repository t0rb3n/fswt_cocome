using Application.Enterprise;

namespace Application.Store;

/// <summary>
/// Class <c>StoreEnterpriseDTO</c> is used as a data transfer object for transferring data from the database.
/// It is used in the Enterprise and Store application to handle the business logic.
/// A DTO can be either a copy of the persisted data for further processing, or
/// to modify or add new data in the persistence layer.
/// </summary>
public class StoreEnterpriseDTO : StoreDTO
{
    protected EnterpriseDTO enterprise;

    /// <summary>
    /// This constructor initializes the new StoreEnterpriseDTO with default values.
    /// <para>StoreEnterpriseDTO objects with Id = -1 means that it does not contain any data.</para>
    /// </summary>
    public StoreEnterpriseDTO()
    {
        storeId = -1;
        storeName = "";
        location = "";
        enterprise = new EnterpriseDTO();
    }

    /// <summary>
    /// Provides get and set methods for Enterprise property.
    /// </summary>
    /// <value>Property <c>Enterprise</c> represents the enterprise belongs the Store object.</value>
    public EnterpriseDTO Enterprise
    {
        get => enterprise;
        set => enterprise = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    /// <summary>
    /// Reports a EnterpriseDTO properties as a string.
    /// </summary>
    /// <returns>A string with the properties storeId, storeName, location and enterprise.</returns>
    public override string ToString()
    {
        return $"Id: {storeId}, Name: {storeName}, Loc: {location}, Enterprise: {enterprise.EnterpriseName}";
    }
}
