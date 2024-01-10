namespace ExperimentalApp.Core.Models;

/// <summary>
/// Represents a class for working with Rental
/// </summary>
public partial class Rental
{
    /// <summary>
    /// Gets or sets RentalId
    /// </summary>
    public int RentalId { get; set; }

    /// <summary>
    /// Gets or sets RentalDate date time
    /// </summary>
    public DateTime RentalDate { get; set; }

    /// <summary>
    /// Gets or sets InventoryId
    /// </summary>
    public int InventoryId { get; set; }

    /// <summary>
    /// Gets or sets CustomerId
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Gets or sets ReturnDate
    /// </summary>
    public DateTime? ReturnDate { get; set; }

    /// <summary>
    /// Gets or sets StaffId
    /// </summary>
    public int StaffId { get; set; }

    /// <summary>
    /// Gets or sets LastUpdate date time
    /// </summary>
    public DateTime LastUpdate { get; set; }

    /// <summary>
    /// Gets or sets Customer
    /// </summary>
    public virtual Customer Customer { get; set; } = null!;

    /// <summary>
    /// Gets or sets Inventory
    /// </summary>
    public virtual Inventory Inventory { get; set; } = null!;

    /// <summary>
    /// Gets or sets the collection of Payments
    /// </summary>
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    /// <summary>
    /// Gets or sets Staff
    /// </summary>
    public virtual Staff Staff { get; set; } = null!;
}
