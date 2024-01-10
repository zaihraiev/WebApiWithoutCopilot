namespace ExperimentalApp.Core.Models;

/// <summary>
/// Represents a class for working with Category
/// </summary>
public partial class Category
{
    /// <summary>
    /// Gets or sets CategoryId
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Gets or sets Name
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets LastUpdate date time
    /// </summary>
    public DateTime LastUpdate { get; set; }

    /// <summary>
    /// Gets or sets the collection of FilmCategories
    /// </summary>
    public virtual ICollection<FilmCategory> FilmCategories { get; set; } = new List<FilmCategory>();
}
