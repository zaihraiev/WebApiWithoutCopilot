using ExperimentalApp.Core.Enums;

namespace ExperimentalApp.Core.ViewModels
{
    /// <summary>
    /// Represents view model for new film creation
    /// </summary>
    public class FilmAddViewModel
    {
        /// <summary>
        /// Sets title for film
        /// </summary>
        public string Title { get; set; }  

        /// <summary>
        /// Sets language id for film
        /// </summary>
        public int LanguageId { get; set; }

        /// <summary>
        /// Sets film categories ids
        /// </summary>
        public List<int> CategoriesIds { get; set; }

        /// <summary>
        /// Sets film actors for film
        /// </summary>
        public List<int> ActorsIds { get; set; }

        /// <summary>
        /// Sets description for film
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Sets release year for film
        /// </summary>
        public int? ReleaseYear { get; set; }

        /// <summary>
        /// Sets rental duration for film
        /// </summary>
        public short? RentalDuration { get; set; }

        /// <summary>
        /// Sets rental rate for film
        /// </summary>
        public decimal? RentalRate { get; set; }

        /// <summary>
        /// Sets length for film
        /// </summary>
        public short? Length { get; set; }

        /// <summary>
        /// Sets replacement cost for film
        /// </summary>
        public decimal? ReplacementCost { get; set; }

        /// <summary>
        /// Sets rating for film
        /// </summary>
        public MPAA_Rating? Rating { get; set; }
    }
}
