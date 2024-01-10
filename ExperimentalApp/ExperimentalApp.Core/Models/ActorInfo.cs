namespace ExperimentalApp.Core.Models;

/// <summary>
/// Represents a class for working with actor info
/// </summary>
public partial class ActorInfo
{
    /// <summary>
    /// Gets or sets ActorId
    /// </summary>
    public int? ActorId { get; set; }

    /// <summary>
    /// Gets or sets FirstName
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Gets or sets LastName
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Gets or sets FilmInfo
    /// </summary>
    public string? FilmInfo { get; set; }
}
