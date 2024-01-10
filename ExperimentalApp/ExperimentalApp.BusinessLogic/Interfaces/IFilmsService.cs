using ExperimentalApp.Core.DTOs;
using ExperimentalApp.Core.ViewModels;

namespace ExperimentalApp.BusinessLogic.Interfaces
{
    /// <summary>
    /// Represents service interface for working with films.
    /// </summary>
    public interface IFilmsService
    {
        /// <summary>
        /// Method for getting films by filter params
        /// </summary>
        /// <param name="filmsFilterViewModel">Params that uses for filtering films</param>
        /// <returns>Mapped filtered films collection.</returns>
        public Task<IEnumerable<FilmResponseDTO>> GetFilmsAsync(FilmsFilterViewModel filmsFilterViewModel);

        /// <summary>
        /// Method for deleting film by id
        /// </summary>
        /// <param name="filmId">Film id to delete</param>
        /// <returns>Boolean result of executed operation</returns>
        public Task<bool> DeleteFilmByIdAsync(int filmId);

        /// <summary>
        /// Method for creating new film by using values from viewmodel
        /// </summary>
        /// <param name="filmAddViewModel">Values for new film creation</param>
        /// <returns>Boolean result of execution</returns>
        public Task<bool> AddFilmAsync(FilmAddViewModel filmAddViewModel);
    }
}
