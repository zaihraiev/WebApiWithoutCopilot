using AutoMapper;
using ExperimentalApp.BusinessLogic.Interfaces;
using ExperimentalApp.BusinessLogic.Services;
using ExperimentalApp.Core.DTOs;
using ExperimentalApp.Core.Enums;
using ExperimentalApp.Core.Models;
using ExperimentalApp.Core.ViewModels;
using ExperimentalApp.DataAccessLayer.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExperimentalApp.UnitTests
{
    public class FilmsServiceTests
    {
        private Mock<IFilmsRepository> _filmsRepositoryMock;
        private IFilmsService _filmsService;
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddMaps(new[] {
                "ExperimentalApp.BusinessLogic"
             }));
            _mapper = configuration.CreateMapper();
            _filmsRepositoryMock = new Mock<IFilmsRepository>();
            _filmsService = new FilmsService(_filmsRepositoryMock.Object, _mapper);
        }

        [Test]
        public void GetFilmsAsync_WhenCalls_ReturnsCollectionOfFilmsResponseDto()
        {
            // Arrange 
            var filmsViewModel = new FilmsFilterViewModel
            {
                StoreId = 1,
                CategoryId = 1
            };

            _filmsRepositoryMock.Setup(f => f.GetFilmsAsync(filmsViewModel)).Returns(Task.FromResult(GetFakeFilms()));

            // Act
            var result = _filmsService.GetFilmsAsync(filmsViewModel);

            // Asserts
            Assert.IsInstanceOf<IEnumerable<FilmResponseDTO>>(result.Result);
            Assert.IsTrue(result.Result.Any());
        }

        [Test]
        public void DeleteFilmByIdAsync_WhenFailed_ReturnsFalse()
        {
            // Arrange
            var id = 1;

            _filmsRepositoryMock.Setup(f => f.DeleteFilmByIdAsync(id)).Returns(Task.FromResult(false));

            // Act
            var result = _filmsService.DeleteFilmByIdAsync(id);

            // Asserts
            Assert.IsFalse(result.Result);
        }

        [Test]
        public void DeleteFilmByIdAsync_WhenSuccess_ReturnsTrue()
        {
            // Arrange
            var id = 1;

            _filmsRepositoryMock.Setup(f => f.DeleteFilmByIdAsync(id)).Returns(Task.FromResult(true));

            // Act
            var result = _filmsService.DeleteFilmByIdAsync(id);

            // Asserts
            Assert.IsTrue(result.Result);
        }


        [Test]
        public void AddFilmAsync_WhenCallsAndAllRight_ReturnsTrue()
        {
            // Arrange
            var viewModel = new FilmAddViewModel
            {
                Title = "test",
                CategoriesIds = new List<int> { 1, 2 },
                ActorsIds = new List<int> { 1, 2 },
                LanguageId = 1
            };

            _filmsRepositoryMock.Setup(r => r.GetFilmCategoriesByIdsAsync(It.IsAny<List<int>>()))
                .Returns(Task.FromResult(new List<Category> { new Category { CategoryId = 1 }, new Category { CategoryId = 2 } }));

            _filmsRepositoryMock.Setup(r => r.GetFilmActorsByIdsAsync(It.IsAny<List<int>>()))
                .Returns(Task.FromResult(new List<Actor> { new Actor { ActorId = 1 }, new Actor { ActorId = 2 } }));

            _filmsRepositoryMock.Setup(r => r.AddFilmAsync(It.IsAny<Film>()))
                .ReturnsAsync(true);

            // Act
            var result = _filmsService.AddFilmAsync(viewModel);

            // Assert
            Assert.True(result.Result);
            _filmsRepositoryMock.Verify(r => r.GetFilmCategoriesByIdsAsync(
                It.Is<List<int>>(categories => categories.SequenceEqual(viewModel.CategoriesIds))),
                Times.Once);

            _filmsRepositoryMock.Verify(r => r.GetFilmActorsByIdsAsync(
                It.Is<List<int>>(actors => actors.SequenceEqual(viewModel.ActorsIds))),
                Times.Once);

            _filmsRepositoryMock.Verify(r => r.AddFilmAsync(
                It.Is<Film>(film =>
                    film.Title == viewModel.Title &&
                    film.LanguageId == viewModel.LanguageId &&
                    film.FilmCategories.Count == viewModel.CategoriesIds.Count &&
                    film.FilmActors.Count == viewModel.ActorsIds.Count)),
                Times.Once);
        }

        [Test]
        public void AddFilmAsync_NullChecksAndDefaultValuesAssignment()
        {
            // Arrange
            var filmAddViewModel = new FilmAddViewModel
            {
                Title = "Test Film",
                CategoriesIds = new List<int> { 1, 2 },
                ActorsIds = new List<int> { 1, 2 },
                LanguageId = 1,
                RentalDuration = null,
                RentalRate = null,
                ReplacementCost = null,
                Rating = null
            };

            _filmsRepositoryMock.Setup(r => r.GetFilmCategoriesByIdsAsync(It.IsAny<List<int>>()))
                .Returns(Task.FromResult(new List<Category>()));

            _filmsRepositoryMock.Setup(r => r.GetFilmActorsByIdsAsync(It.IsAny<List<int>>()))
                .Returns(Task.FromResult(new List<Actor>()));

            // Act
            var result = _filmsService.AddFilmAsync(filmAddViewModel);

            // Asserts
            _filmsRepositoryMock.Verify(r => r.AddFilmAsync(It.Is<Film>(f =>
                f.RentalDuration == default(int) &&
                f.RentalRate == default(decimal) &&
                f.ReplacementCost == default(decimal) &&
                f.Rating == default(MPAA_Rating)
                )), Times.Once);
        }

        private IEnumerable<Film> GetFakeFilms()
        {
            return new List<Film>
            {
                new Film
                {
                    FilmId = 1,
                    FilmCategories = new List<FilmCategory> { new FilmCategory { CategoryId = 1 } }
                },
                new Film
                {
                    FilmId = 2,
                    FilmCategories = new List<FilmCategory> { new FilmCategory { CategoryId = 2 } }
                }
            };
        }
    }
}
