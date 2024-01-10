using ExperimentalApp.Core.Models;
using ExperimentalApp.Core.ViewModels;

namespace ExperimentalApp.DataAccessLayer.Interfaces
{
    /// <summary>
    /// Represents repository interface for working with films
    /// </summary>
    public interface IFilmsRepository
    {
        /// <summary>
        /// Represents method for getting filtered films for database.
        /// </summary>
        /// <param name="filmsFilterViewModel">Filter params</param>
        /// <returns>Filtered films from database</returns>
        public Task<IEnumerable<Film>> GetFilmsAsync(FilmsFilterViewModel filmsFilterViewModel);

        /// <summary>
        /// Represents method for deleting film by id. Includes deleting dependencies such as actors and categories
        /// </summary>
        /// <param name="filmId">Film id to search and delete</param>
        /// <returns>Boolean result of executed operation</returns>
        public Task<bool> DeleteFilmByIdAsync(int filmId);

        /// <summary>
        /// Represents method for adding new film into database
        /// </summary>
        /// <param name="film">Film that should be added</param>
        /// <returns>Boolean result of execution</returns>
        public Task<bool> AddFilmAsync(Film film);

        /// <summary>
        /// Represents method for getting actors by theirs ids
        /// </summary>
        /// <param name="actorsIds">Actor ids to search</param>
        /// <returns>Founded actors by ids</returns>
        public Task<List<Actor>> GetFilmActorsByIdsAsync(List<int> actorsIds);

        /// <summary>
        /// Represents method for getting categories by theirs ids
        /// </summary>
        /// <param name="categoriesIds">Category ids to search</param>
        /// <returns>Founded categories by ids</returns>
        public Task<List<Category>> GetFilmCategoriesByIdsAsync(List<int> categoriesIds);
    }
}
