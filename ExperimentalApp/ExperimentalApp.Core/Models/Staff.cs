namespace ExperimentalApp.Core.Models;

/// <summary>
/// Represents a class for working with Staff
/// </summary>
public partial class Staff
{
    /// <summary>
    /// Gets or sets StaffId
    /// </summary>
    public int StaffId { get; set; }

    /// <summary>
    /// Gets or sets FirstName
    /// </summary>
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// Gets or sets LastName
    /// </summary>
    public string LastName { get; set; } = null!;

    /// <summary>
    /// Gets or sets AddressId
    /// </summary>
    public int AddressId { get; set; }

    /// <summary>
    /// Gets or sets Email
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets StoreId
    /// </summary>
    public int StoreId { get; set; }

    /// <summary>
    /// Gets or sets Active
    /// </summary>
    public bool? Active { get; set; }

    /// <summary>
    /// Gets or sets Username
    /// </summary>
    public string Username { get; set; } = null!;

    /// <summary>
    /// Gets or sets Password
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Gets or sets LastUpdate date time
    /// </summary>
    public DateTime LastUpdate { get; set; }

    /// <summary>
    /// Gets or sets Picture
    /// </summary>
    public byte[]? Picture { get; set; }

    /// <summary>
    /// Gets or sets Address
    /// </summary>
    public virtual Address Address { get; set; } = null!;

    /// <summary>
    /// Gets or sets the collection of Payments
    /// </summary>
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    /// <summary>
    /// Gets or sets the collection of Rentals
    /// </summary>
    public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}
