namespace ExperimentalApp.Core.Models;

/// <summary>
/// Represents a class for working with SalesByStore
/// </summary>
public partial class SalesByStore
{
    /// <summary>
    /// Gets or sets Store
    /// </summary>
    public string? Store { get; set; }

    /// <summary>
    /// Gets or sets Manager
    /// </summary>
    public string? Manager { get; set; }

    /// <summary>
    /// Gets or sets TotalSales
    /// </summary>
    public decimal? TotalSales { get; set; }
}
