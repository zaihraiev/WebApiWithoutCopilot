namespace ExperimentalApp.Core.Models;

/// <summary>
/// Represents a class for working with Language
/// </summary>
public partial class Language
{
    /// <summary>
    /// Gets or sets LanguageId
    /// </summary>
    public int LanguageId { get; set; }

    /// <summary>
    /// Gets or sets Name
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets LastUpdate date time
    /// </summary>
    public DateTime LastUpdate { get; set; }

    /// <summary>
    /// Gets or sets the collection of Films
    /// </summary>
    public virtual ICollection<Film> Films { get; set; } = new List<Film>();
}
