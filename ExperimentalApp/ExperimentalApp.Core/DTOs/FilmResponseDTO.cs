namespace ExperimentalApp.Core.DTOs
{
    /// <summary>
    /// Represnets film model that returns as response.
    /// </summary>
    public class FilmResponseDTO
    {
        /// <summary>
        /// Film id to return
        /// </summary>
        public int FilmId { get; set; }

        /// <summary>
        /// Film title to return
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Film description to return
        /// </summary>
        public string Description { get; set; } 

        /// <summary>
        /// Film release year to return
        /// </summary>
        public int ReleaseYear { get; set; }

        /// <summary>
        /// Film language to return
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Film length to return
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Film rating to return
        /// </summary>
        public string Rating { get;set; }

    }
}
