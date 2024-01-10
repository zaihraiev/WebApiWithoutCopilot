namespace ExperimentalApp.Core.Models;

/// <summary>
/// Represents a class for working with Country
/// </summary>
public partial class Country
{
    /// <summary>
    /// Gets or sets CountryId
    /// </summary>
    public int CountryId { get; set; }

    /// <summary>
    /// Gets or sets Country1
    /// </summary>
    public string Country1 { get; set; } = null!;

    /// <summary>
    /// Gets or sets LastUpdate date time
    /// </summary>
    public DateTime LastUpdate { get; set; }

    /// <summary>
    /// Gets or sets the collection of Cities
    /// </summary>
    public virtual ICollection<City> Cities { get; set; } = new List<City>();
}
