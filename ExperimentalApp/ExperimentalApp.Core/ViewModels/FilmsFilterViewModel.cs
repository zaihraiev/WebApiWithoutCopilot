using ExperimentalApp.Core.Enums;

namespace ExperimentalApp.Core.ViewModels
{
    /// <summary>
    /// Represents view model for filtering films
    /// </summary>
    public class FilmsFilterViewModel
    {
        /// <summary>
        /// Gets or sets store id
        /// </summary>
        public int StoreId { get; set; }

        /// <summary>
        /// Gets or sets title
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets rating
        /// </summary>
        public MPAA_Rating? Rating { get; set; }

        /// <summary>
        /// Gets or sets category id
        public int? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets description
        /// </summary>
        public string? Description { get; set; }
    }
}
