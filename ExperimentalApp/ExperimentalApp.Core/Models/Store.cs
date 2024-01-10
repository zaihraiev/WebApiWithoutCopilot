using ExperimentalApp.Core.Models.Identity;

namespace ExperimentalApp.Core.Models;

/// <summary>
/// Represents a class for working with Store
/// </summary>
public partial class Store
{
    /// <summary>
    /// Gets or sets StoreId
    /// </summary>
    public int StoreId { get; set; }

    /// <summary>
    /// Gets or sets ManagerStaffId
    /// </summary>
    public string? ManagerStaffId { get; set; }

    /// <summary>
    /// Gets or sets AddressId
    /// </summary>
    public int AddressId { get; set; }

    /// <summary>
    /// Gets or sets LastUpdate date time
    /// </summary>
    public DateTime LastUpdate { get; set; }

    /// <summary>
    /// Gets or sets Address
    /// </summary>
    public virtual Address Address { get; set; } = null!;

    /// <summary>
    /// Gets or sets ManagerStaff
    /// </summary>
    public virtual ApplicationUser ManagerStaff { get; set; } = null!;
}
