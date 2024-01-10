namespace ExperimentalApp.Core.Models;

/// <summary>
/// Represents a class for working with Inventory
/// </summary>
public partial class Inventory
{
    /// <summary>
    /// Gets or sets InventoryId
    /// </summary>
    public int InventoryId { get; set; }

    /// <summary>
    /// Gets or sets FilmId
    /// </summary>
    public int FilmId { get; set; }

    /// <summary>
    /// Gets or sets StoreId
    /// </summary>
    public int StoreId { get; set; }

    /// <summary>
    /// Gets or sets LastUpdate date time
    /// </summary>
    public DateTime LastUpdate { get; set; }

    /// <summary>
    /// Gets or sets Film
    /// </summary>
    public virtual Film Film { get; set; } = null!;

    /// <summary>
    /// Gets or sets the collection of Rentals
    /// </summary>
    public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}
