namespace ExperimentalApp.Core.Models;

/// <summary>
/// Represents a class for working with FilmCategory
/// </summary>
public partial class FilmCategory
{
    /// <summary>
    /// Gets or sets FilmId
    /// </summary>
    public int FilmId { get; set; }

    /// <summary>
    /// Gets or sets CategoryId
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Gets or sets LastUpdate date time
    /// </summary>
    public DateTime LastUpdate { get; set; }

    /// <summary>
    /// Gets or sets Category
    /// </summary>
    public virtual Category Category { get; set; } = null!;

    /// <summary>
    /// Gets or sets Film
    /// </summary>
    public virtual Film Film { get; set; } = null!;
}
