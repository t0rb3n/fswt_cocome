namespace Application.Enterprise;

/// <summary>
/// Class <c>EnterpriseDTO</c> is used as a data transfer object for transferring data from the database.
/// It is used in the Enterprise and Store application to handle the business logic.
/// A DTO can be either a copy of the persisted data for further processing, or
/// to modify or add new data in the persistence layer.
/// </summary>
public class EnterpriseDTO
{
    protected long enterpriseId;
    protected string enterpriseName;

    /// <summary>
    /// This constructor initializes the new EnterpriseDTO with default values.
    /// <para>EnterpriseDTO objects with Id = -1 means that it does not contain any data.</para>
    /// </summary>
    public EnterpriseDTO()
    {
        enterpriseId = -1;
        enterpriseName = "";
    }
    
    /// <summary>
    /// Provides get and set methods for EnterpriseId property.
    /// </summary>
    /// <value>Property <c>EnterpriseId</c> represents a unique identifier for Enterprise objects.</value>
    public long EnterpriseId
    {
        get => enterpriseId;
        set => enterpriseId = value;
    }

    /// <summary>
    /// Provides get and set methods for EnterpriseName property.
    /// </summary>
    /// <value>Property <c>EnterpriseName</c> represents the name of the Enterprise objects.</value>
    /// <exception cref="ArgumentNullException">If set EnterpriseName with null.</exception>
    public string EnterpriseName
    {
        get => enterpriseName;
        set => enterpriseName = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Reports a EnterpriseDTO properties as a string.
    /// </summary>
    /// <returns>A string with the enterprise properties enterpriseId and enterpriseName.</returns>
    public override string ToString()
    {
        return $"Id: {enterpriseId}, Enterprise: {enterpriseName}";
    }
}
