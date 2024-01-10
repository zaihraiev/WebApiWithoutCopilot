namespace ExperimentalApp.Core.Models;

/// <summary>
/// Represents a class for working with FilmList
/// </summary>
public partial class FilmList
{
    /// <summary>
    /// Gets or sets Fid
    /// </summary>
    public int? Fid { get; set; }

    /// <summary>
    /// Gets or sets Title
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets Description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets Category
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// Gets or sets Price
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// Gets or sets Length
    /// </summary>
    public short? Length { get; set; }

    /// <summary>
    /// Gets or sets Actors
    /// </summary>
    public string? Actors { get; set; }
}
