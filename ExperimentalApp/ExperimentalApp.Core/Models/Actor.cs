namespace ExperimentalApp.Core.Models;

/// <summary>
/// Represents a class for working with actor
/// </summary>
public partial class Actor
{
    /// <summary>
    /// Gets or sets ActorId
    /// </summary>
    public int ActorId { get; set; }

    /// <summary>
    /// Gets or sets FirstName of the actor
    /// </summary>
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// Gets or sets LastName of the actor
    /// </summary>
    public string LastName { get; set; } = null!;

    /// <summary>
    /// Gets or sets LastUpdate date and time of the actor
    /// </summary>
    public DateTime LastUpdate { get; set; }

    /// <summary>
    /// Gets or sets the collection of films associated with the actor
    /// </summary>
    public virtual ICollection<FilmActor> FilmActors { get; set; } = new List<FilmActor>();
}
