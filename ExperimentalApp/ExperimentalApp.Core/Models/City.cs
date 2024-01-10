namespace ExperimentalApp.Core.Models;

/// <summary>
/// Represents a class for working with City
/// </summary>
public partial class City
{
    /// <summary>
    /// Gets or sets CityId
    /// </summary>
    public int CityId { get; set; }

    /// <summary>
    /// Gets or sets City1
    /// </summary>
    public string City1 { get; set; } = null!;

    /// <summary>
    /// Gets or sets CountryId
    /// </summary>
    public int CountryId { get; set; }

    /// <summary>
    /// Gets or sets LastUpdate date time
    /// </summary>
    public DateTime LastUpdate { get; set; }

    /// <summary>
    /// Gets or sets the collection of Addresses
    /// </summary>
    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    /// <summary>
    /// Gets or sets Country
    /// </summary>
    public virtual Country Country { get; set; } = null!;
}
