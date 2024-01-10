namespace ExperimentalApp.Core.Models;

/// <summary>
/// Represents a class for working with Customer
/// </summary>
public partial class Customer
{
    /// <summary>
    /// Gets or sets CustomerId
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Gets or sets StoreId
    /// </summary>
    public int StoreId { get; set; }

    /// <summary>
    /// Gets or sets FirstName
    /// </summary>
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// Gets or sets LastName
    /// </summary>
    public string LastName { get; set; } = null!;

    /// <summary>
    /// Gets or sets Email
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets AddressId
    /// </summary>
    public int AddressId { get; set; }

    /// <summary>
    /// Gets or sets Activebool
    /// </summary>
    public bool? Activebool { get; set; }

    /// <summary>
    /// Gets or sets CreateDate date
    /// </summary>
    public DateOnly CreateDate { get; set; }

    /// <summary>
    /// Gets or sets LastUpdate date time
    /// </summary>
    public DateTime? LastUpdate { get; set; }

    /// <summary>
    /// Gets or sets Active
    /// </summary>
    public int? Active { get; set; }

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
