namespace ExperimentalApp.Core.Models;

/// <summary>
/// Represents a class for working with SalesByFilmCategory
/// </summary>
public partial class SalesByFilmCategory
{
    /// <summary>
    /// Gets or sets Category
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// Gets or sets TotalSales
    /// </summary>
    public decimal? TotalSales { get; set; }
}
