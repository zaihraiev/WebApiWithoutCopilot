using AutoMapper;
using ExperimentalApp.BusinessLogic.Interfaces;
using ExperimentalApp.Core.DTOs;
using ExperimentalApp.Core.Enums;
using ExperimentalApp.Core.Models;
using ExperimentalApp.Core.ViewModels;
using ExperimentalApp.DataAccessLayer.Interfaces;

namespace ExperimentalApp.BusinessLogic.Services
{
    /// <summary>
    /// Represents service for working with films.
    /// </summary>
    public class FilmsService : IFilmsService
    {
        private readonly IFilmsRepository _filmsRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Film`s service constructor with params
        /// </summary>
        /// <param name="filmsRepository">Repository to communicate with database</param>
        /// <param name="mapper">Mapper for mapping models from database to data transfer objects</param>
        public FilmsService(IFilmsRepository filmsRepository, IMapper mapper)
        {
            _filmsRepository = filmsRepository;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteFilmByIdAsync(int filmId)
        {
            return await _filmsRepository.DeleteFilmByIdAsync(filmId);
        }

        /// <inheritdoc/>
        public async Task<bool> AddFilmAsync(FilmAddViewModel filmAddViewModel)
        {
            var categories = await _filmsRepository.GetFilmCategoriesByIdsAsync(filmAddViewModel.CategoriesIds);
            var actors = await _filmsRepository.GetFilmActorsByIdsAsync(filmAddViewModel.ActorsIds);
            var filmCategories = SetupFilmCategories(categories);
            var filmActors = SetupFilmActors(actors);

            var film = new Film
            {
                Title = filmAddViewModel.Title,
                LanguageId = filmAddViewModel.LanguageId,
                FilmCategories = filmCategories,
                FilmActors = filmActors,
                Description = filmAddViewModel.Description,
                ReleaseYear = filmAddViewModel.ReleaseYear,
                RentalDuration = filmAddViewModel.RentalDuration.GetValueOrDefault(),
                RentalRate = filmAddViewModel.RentalRate.GetValueOrDefault(),
                Length = filmAddViewModel.Length,
                ReplacementCost = filmAddViewModel.ReplacementCost.GetValueOrDefault(),
                Rating = filmAddViewModel.Rating.GetValueOrDefault(),
                LastUpdate = DateTime.Now
            };

            return await _filmsRepository.AddFilmAsync(film);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<FilmResponseDTO>> GetFilmsAsync(FilmsFilterViewModel filmsFilterViewModel)
        {
            var films = await _filmsRepository.GetFilmsAsync(filmsFilterViewModel);
            var responseFilms = _mapper.Map<List<FilmResponseDTO>>(films);

            return responseFilms;
        }

        /// <summary>
        /// Setups film categories for film that will be created
        /// </summary>
        /// <param name="categories">Categories to setup</param>
        /// <returns>Collection of film categories type</returns>
        private List<FilmCategory> SetupFilmCategories(List<Category> categories)
        {
            var filmCategories = new List<FilmCategory>();

            foreach (var category in categories)
            {
                filmCategories.Add(new FilmCategory
                {
                    Category = category
                });
            }

            return filmCategories;
        }

        /// <summary>
        /// Setups film actors for film that will be created
        /// </summary>
        /// <param name="actors">Actors to setup</param>
        /// <returns>Collection of film actors type</returns>
        private List<FilmActor> SetupFilmActors(List<Actor> actors)
        {
            var filmActors = new List<FilmActor>();

            foreach (var actor in actors)
            {
                filmActors.Add(new FilmActor
                {
                    Actor = actor
                });
            }

            return filmActors;
        }
    }
}
