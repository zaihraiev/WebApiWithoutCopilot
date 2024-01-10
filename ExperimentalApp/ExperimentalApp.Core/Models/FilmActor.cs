namespace ExperimentalApp.Core.Models;

/// <summary>
/// Represents a class for working with FilmActor
/// </summary>
public partial class FilmActor
{
    /// <summary>
    /// Gets or sets ActorId
    /// </summary>
    public int ActorId { get; set; }

    /// <summary>
    /// Gets or sets FilmId
    /// </summary>
    public int FilmId { get; set; }

    /// <summary>
    /// Gets or sets LastUpdate date time
    /// </summary>
    public DateTime LastUpdate { get; set; }

    /// <summary>
    /// Gets or sets Actor
    /// </summary>
    public virtual Actor Actor { get; set; } = null!;

    /// <summary>
    /// Gets or sets Film
    /// </summary>
    public virtual Film Film { get; set; } = null!;
}
