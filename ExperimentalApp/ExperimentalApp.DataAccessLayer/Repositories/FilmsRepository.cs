using ExperimentalApp.Core.Models;
using ExperimentalApp.Core.ViewModels;
using ExperimentalApp.DataAccessLayer.DBContext;
using ExperimentalApp.DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExperimentalApp.DataAccessLayer.Repositories
{
    /// <summary>
    /// Represents repository for working with films in database.
    /// </summary>
    public class FilmsRepository : IFilmsRepository
    {
        private readonly DvdRentalContext _context;

        /// <summary>
        /// Constructor for films repository with params
        /// </summary>
        /// <param name="context">Data base context for working with entity</param>
        public FilmsRepository(DvdRentalContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteFilmByIdAsync(int filmId)
        {
            var filmToDelete = await _context.Films.FindAsync(filmId);

            if(filmToDelete == null)
            {
                return false;
            }

            var filmActorsToDelete = _context.FilmActors.Where(fa => fa.FilmId == filmId);
            _context.FilmActors.RemoveRange(filmActorsToDelete);

            var filmCategoriesToDelete = _context.FilmCategories.Where(fa => fa.FilmId == filmId);
            _context.FilmCategories.RemoveRange(filmCategoriesToDelete);

            _context.Films.Remove(filmToDelete);
            
            return await _context.SaveChangesAsync() > 0;
        }

        /// <inheritdoc/>
        public async Task<bool> AddFilmAsync(Film film)
        {
            await _context.Films.AddAsync(film);

            return await _context.SaveChangesAsync() > 0;
        }

        /// <inheritdoc/>
        public async Task<List<Actor>> GetFilmActorsByIdsAsync(List<int> actorsIds)
        {
            return await _context.Actors.Where(a => actorsIds.Contains(a.ActorId)).Distinct().ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<List<Category>> GetFilmCategoriesByIdsAsync(List<int> categoriesIds)
        {
            return await _context.Categories.Where(a => categoriesIds.Contains(a.CategoryId)).Distinct().ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Film>> GetFilmsAsync(FilmsFilterViewModel filmsFilterViewModel)
        {
            var baseQuery = _context.Inventories
                .Where(i => i.StoreId == filmsFilterViewModel.StoreId)
                .Include(i => i.Film)
                    .ThenInclude(f => f.FilmCategories)
                .Include(i => i.Film)
                    .ThenInclude(f => f.Language);

            var filmsQuery = baseQuery.Select(i => i.Film);

            if(filmsFilterViewModel.Title is null &&
               filmsFilterViewModel.Rating is null &&
               filmsFilterViewModel.CategoryId is null &&
               filmsFilterViewModel.Description is null)
            {
                return await filmsQuery.Distinct().Take(10).ToListAsync();
            }

            if (filmsFilterViewModel.Title is not null)
            {
                filmsQuery = filmsQuery.Where(f => f.Title.ToLower().Contains(filmsFilterViewModel.Title.ToLower()));
            }

            if (filmsFilterViewModel.CategoryId is not null)
            {
                filmsQuery = filmsQuery.Where(f => f.FilmCategories.Any(fc => fc.CategoryId == filmsFilterViewModel.CategoryId));
            }

            if (filmsFilterViewModel.Rating is not null)
            {
                filmsQuery = filmsQuery.Where(f => f.Rating == filmsFilterViewModel.Rating);
            }

            if(filmsFilterViewModel.Description is not null)
            {
                filmsQuery = filmsQuery.Where(f => f.Description!.ToLower().Contains(filmsFilterViewModel.Description.ToLower()));
            }

            var films = await filmsQuery.Distinct().ToListAsync();

            return films;
        }
    }
}
