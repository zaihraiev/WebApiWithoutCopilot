using ExperimentalApp.Core.Enums;
using NpgsqlTypes;

namespace ExperimentalApp.Core.Models;

/// <summary>
/// Represents a class for working with Film
/// </summary>
public partial class Film
{
    /// <summary>
    /// Gets or sets FilmId
    /// </summary>
    public int FilmId { get; set; }

    /// <summary>
    /// Gets or sets Title
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Gets or sets Description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets ReleaseYear
    /// </summary>
    public int? ReleaseYear { get; set; }

    /// <summary>
    /// Gets or sets LanguageId
    /// </summary>
    public int LanguageId { get; set; }

    /// <summary>
    /// Gets or sets RentalDuration
    /// </summary>
    public short RentalDuration { get; set; }

    /// <summary>
    /// Gets or sets RentalRate
    /// </summary>
    public decimal RentalRate { get; set; }

    /// <summary>
    /// Gets or sets Length
    /// </summary>
    public short? Length { get; set; }

    /// <summary>
    /// Gets or sets ReplacementCost
    /// </summary>
    public decimal ReplacementCost { get; set; }

    /// <summary>
    /// Gets or sets LastUpdate date time
    /// </summary>
    public DateTime LastUpdate { get; set; }

    /// <summary>
    /// Gets or sets the array of SpecialFeatures
    /// </summary>
    public string[]? SpecialFeatures { get; set; }

    /// <summary>
    /// Gets or sets Fulltext
    /// </summary>
    public NpgsqlTsVector Fulltext { get; set; } = null!;

    /// <summary>
    /// Gets or sets the collection of FilmActors
    /// </summary>
    public virtual ICollection<FilmActor> FilmActors { get; set; } = new List<FilmActor>();

    /// <summary>
    /// Gets or sets the collection of FilmCategories
    /// </summary>
    public virtual ICollection<FilmCategory> FilmCategories { get; set; } = new List<FilmCategory>();

    /// <summary>
    /// Gets or sets the collection of Inventories
    /// </summary>
    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    /// <summary>
    /// Gets or sets Language
    /// </summary>
    public virtual Language Language { get; set; } = null!;

    /// <summary>
    /// Gets or sets Rating
    /// </summary>
    public MPAA_Rating Rating { get; set; }
}
