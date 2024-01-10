namespace ExperimentalApp.Core.Models;

/// <summary>
/// Represents a class for working with adress
/// </summary>
public partial class Address
{
    /// <summary>
    /// Gets or sets AddressId
    /// </summary>
    public int AddressId { get; set; }

    /// <summary>
    /// Gets or sets Address1
    /// </summary>
    public string Address1 { get; set; } = null!;

    /// <summary>
    /// Gets or sets Address2
    /// </summary>
    public string? Address2 { get; set; }

    /// <summary>
    /// Gets or sets District
    /// </summary>
    public string District { get; set; } = null!;

    /// <summary>
    /// Gets or sets CityId
    /// </summary>
    public int CityId { get; set; }

    /// <summary>
    /// Gets or sets PostalCode
    /// </summary>
    public string? PostalCode { get; set; }

    /// <summary>
    /// Gets or sets Phone
    /// </summary>
    public string Phone { get; set; } = null!;

    /// <summary>
    /// Gets or sets LastUpdate date time
    /// </summary>
    public DateTime LastUpdate { get; set; }

    /// <summary>
    /// Gets or sets City
    /// </summary>
    public virtual City City { get; set; } = null!;

    /// <summary>
    /// Gets or sets the collection of Customers
    /// </summary>
    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    /// <summary>
    /// Gets or sets the collection of Staff
    /// </summary>
    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();

    /// <summary>
    /// Gets or sets the collection of Stores
    /// </summary>
    public virtual ICollection<Store> Stores { get; set; } = new List<Store>();
}
