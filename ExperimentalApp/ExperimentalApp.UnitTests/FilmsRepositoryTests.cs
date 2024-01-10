using AutoMapper;
using ExperimentalApp.BusinessLogic.Interfaces;
using ExperimentalApp.BusinessLogic.Services;
using ExperimentalApp.Core.Enums;
using ExperimentalApp.Core.Models;
using ExperimentalApp.Core.ViewModels;
using ExperimentalApp.DataAccessLayer.DBContext;
using ExperimentalApp.DataAccessLayer.Interfaces;
using ExperimentalApp.DataAccessLayer.Repositories;
using ExperimentalApp.UnitTests.Fakes;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExperimentalApp.UnitTests
{
    [TestFixture]
    public class FilmsRepositoryTests
    {
        private Mock<DvdRentalContext> _context;
        private IFilmsRepository _filmsRepository;

        [SetUp]
        public void SetUp()
        {
            _context = GetFakeDbContext();
            _filmsRepository = new FilmsRepository(_context.Object);
        }

        [Test]
        public void GetFilmsAsync_WhenFilterOnlyWithStoreId_ReturnsLastTenFilms()
        {
            // Arrange
            var filmsViewModel = new FilmsFilterViewModel
            {
                StoreId = 1
            };

            // Act
            var result = _filmsRepository.GetFilmsAsync(filmsViewModel);

            // Asserts
            Assert.That(result.Result.Count(), Is.LessThanOrEqualTo(10));
        }

        [Test]
        public void GetFilmsAsync_WhenTitleAssigned_ReturnsFilmsThatContainsTitle()
        {
            // Arrange
            var filmsViewModel = new FilmsFilterViewModel
            {
                StoreId = 1,
                Title = "Title 1"
            };

            // Act
            var result = _filmsRepository.GetFilmsAsync(filmsViewModel);

            // Asserts
            Assert.IsTrue(result.Result.All(f => f.Title.ToLower().Contains(filmsViewModel.Title.ToLower())));
        }

        [Test]
        public void GetFilmsAsync_WhenDescriptionAssigned_ReturnsFilmsThatContainsDescription()
        {
            // Arrange
            var filmsViewModel = new FilmsFilterViewModel
            {
                StoreId = 1,
                Description = "desc 1"
            };

            // Act
            var result = _filmsRepository.GetFilmsAsync(filmsViewModel);

            // Asserts
            Assert.IsTrue(result.Result.All(f => f.Description.ToLower().Contains(filmsViewModel.Description.ToLower())));
        }

        [Test]
        public void GetFilmsAsync_WhenCategoryIdAssigned_ReturnsFilmsWithSpecifiedCategoryId()
        {
            // Arrange
            var filmsViewModel = new FilmsFilterViewModel
            {
                StoreId = 1,
                CategoryId = 1
            };

            // Act
            var result = _filmsRepository.GetFilmsAsync(filmsViewModel);

            // Asserts
            Assert.IsTrue(result.Result.All(f => f.FilmCategories.All(c => c.CategoryId == filmsViewModel.CategoryId)));
        }

        [TestCase(MPAA_Rating.G)]
        [TestCase(MPAA_Rating.PG)]
        [TestCase(MPAA_Rating.R)]
        [TestCase(MPAA_Rating.PG13)]
        [TestCase(MPAA_Rating.NC17)]
        public void GetFilmsAsync_WhenRatingAssigned_ReturnsFilmsWithRating(MPAA_Rating mPAA_Rating)
        {
            // Arrange
            var filmsViewModel = new FilmsFilterViewModel
            {
                StoreId = 1,
                Rating = mPAA_Rating
            };

            // Act
            var result = _filmsRepository.GetFilmsAsync(filmsViewModel);

            // Asserts
            Assert.IsTrue(result.Result.All(f => f.Rating == filmsViewModel.Rating));
        }

        [Test]
        public void GetFilmsAsync_WhenAllParamsSpecified_ReturnsNeededFilms()
        {
            // Arrange
            var filmsViewModel = new FilmsFilterViewModel
            {
                StoreId = 1,
                Title = "Title 1",
                Description = "desc 1",
                CategoryId = 1,
                Rating = MPAA_Rating.G
            };

            // Act
            var result = _filmsRepository.GetFilmsAsync(filmsViewModel);

            // Asserts
            Assert.IsTrue(result.Result.All(f => f.Title.ToLower().Contains(filmsViewModel.Title.ToLower())));
            Assert.IsTrue(result.Result.All(f => f.Description.ToLower().Contains(filmsViewModel.Description.ToLower())));
            Assert.IsTrue(result.Result.All(f => f.FilmCategories.All(c => c.CategoryId == filmsViewModel.CategoryId)));
            Assert.IsTrue(result.Result.All(f => f.Rating == filmsViewModel.Rating));
        }

        [Test]
        public void DeleteFilmByIdAsync_WhenFilmIsNotFound_ReturnsFalse()
        {
            // Arrange
            var id = 10;

            // Act
            var result = _filmsRepository.DeleteFilmByIdAsync(id);

            // Asserts
            Assert.IsFalse(result.Result);
            _context.Verify(c => c.FilmCategories.RemoveRange(It.IsAny<IQueryable<FilmCategory>>()), Times.Never);
            _context.Verify(c => c.FilmActors.RemoveRange(It.IsAny<IQueryable<FilmActor>>()), Times.Never);
            _context.Verify(c => c.Films.Remove(It.IsAny<Film>()), Times.Never);
        }

        [Test]
        public void DeleteFilmByIdAsync_WhenFilmIsFoundAndSuccessfullyDeleted_ReturnsTrue()
        {
            // Arrange
            var id = 1;

            var film = new Film
            {
                FilmId = 1,
                FilmCategories = new List<FilmCategory> { new FilmCategory { CategoryId = 1 } },
                FilmActors = new List<FilmActor> { new FilmActor { ActorId = 1 } },
                Description = "Some desc 1",
                Title = "Some Title 1",
                Rating = MPAA_Rating.G,
                Language = new Language { Name = "en" },
            };

            _context.Setup(c => c.Films.FindAsync(id)).ReturnsAsync(film);
            _context.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = _filmsRepository.DeleteFilmByIdAsync(id);

            // Asserts
            Assert.IsTrue(result.Result);
            _context.Verify(c => c.FilmCategories.RemoveRange(It.IsAny<IQueryable<FilmCategory>>()), Times.Once);
            _context.Verify(c => c.FilmActors.RemoveRange(It.IsAny<IQueryable<FilmActor>>()), Times.Once);
            _context.Verify(c => c.Films.Remove(It.IsAny<Film>()), Times.Once);
        }

        [Test]
        public void DeleteFilmByIdAsync_WhenFilmIsFoundAndFailedDeleting_ReturnsFalse()
        {
            // Arrange
            var id = 1;

            var film = new Film
            {
                FilmId = 1,
                FilmCategories = new List<FilmCategory> { new FilmCategory { CategoryId = 1 } },
                FilmActors = new List<FilmActor> { new FilmActor { ActorId = 1 } },
                Description = "Some desc 1",
                Title = "Some Title 1",
                Rating = MPAA_Rating.G,
                Language = new Language { Name = "en" },
            };

            _context.Setup(c => c.Films.FindAsync(id)).ReturnsAsync(film);

            // Act
            var result = _filmsRepository.DeleteFilmByIdAsync(id);

            // Asserts
            Assert.IsFalse(result.Result);
            _context.Verify(c => c.FilmCategories.RemoveRange(It.IsAny<IQueryable<FilmCategory>>()), Times.Once);
            _context.Verify(c => c.FilmActors.RemoveRange(It.IsAny<IQueryable<FilmActor>>()), Times.Once);
            _context.Verify(c => c.Films.Remove(It.IsAny<Film>()), Times.Once);
        }

        [Test]
        public void GetFilmActorsByIdsAsync_WhenCalled_ReturnsActorsFoundedByIds() 
        {
            // Arrange
            var ids = new List<int> { 1 };

            // Act
            var result = _filmsRepository.GetFilmActorsByIdsAsync(ids);

            // Asserts
            Assert.IsNotNull(result.Result);
            Assert.IsTrue(result.Result.All(a => ids.Contains(a.ActorId)));
        }

        [Test]
        public void GetFilmCategoriesByIdsAsync_WhenCalled_ReturnsCategoriesFoundedByIds()
        {
            // Arrange
            var ids = new List<int> { 1 };

            // Act
            var result = _filmsRepository.GetFilmCategoriesByIdsAsync(ids);

            // Asserts
            Assert.IsNotNull(result.Result);
            Assert.IsTrue(result.Result.All(a => ids.Contains(a.CategoryId)));
        }

        [Test]
        public void AddFilmAsync_WhenCalledAndFilmAddedProperly_ReturnsTrue()
        {
            // Arrange
            var film = new Film
            {
                FilmCategories = new List<FilmCategory> { new FilmCategory { CategoryId = 1 } },
                FilmActors = new List<FilmActor> { new FilmActor { ActorId = 1 } },
                Description = "Some desc 1",
                Title = "Some Title 1",
                Rating = MPAA_Rating.G,
                LanguageId = 1,
            };

            _context.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            // Act
            var result = _filmsRepository.AddFilmAsync(film);

            // Asserts
            Assert.IsTrue(result.Result);
        }

        [Test]
        public void AddFilmAsync_WhenCalledAndFilmNotAdded_ReturnsFalse()
        {
            // Arrange
            var film = new Film
            {
                FilmCategories = new List<FilmCategory> { new FilmCategory { CategoryId = 1 } },
                FilmActors = new List<FilmActor> { new FilmActor { ActorId = 1 } },
                Description = "Some desc 1",
                Title = "Some Title 1",
                Rating = MPAA_Rating.G,
                LanguageId = 1,
            };

            _context.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(0));

            // Act
            var result = _filmsRepository.AddFilmAsync(film);

            // Asserts
            Assert.IsFalse(result.Result);
        }

        private Mock<DvdRentalContext> GetFakeDbContext()
        {
            var filmsList = new List<Film>
            {
                new Film
                {
                    FilmId = 1,
                    FilmCategories = new List<FilmCategory> { new FilmCategory { CategoryId = 1 } },
                    Description = "Some desc 1",
                    Title = "Some Title 1",
                    Rating = MPAA_Rating.G,
                    Language = new Language { Name = "en" },
                },
                new Film
                {
                    FilmId = 2,
                    FilmCategories = new List<FilmCategory> { new FilmCategory { CategoryId = 2 } },
                    Description = "Some desc 2",
                    Title = "Some Title 2",
                    Rating = MPAA_Rating.PG,
                    Language = new Language { Name = "ua" },
                },
                new Film
                {
                    FilmId = 3,
                    FilmCategories = new List<FilmCategory> { new FilmCategory { CategoryId = 1 } },
                    Description = "Some desc 1",
                    Title = "Some Title 1",
                    Rating = MPAA_Rating.NC17,
                    Language = new Language { Name = "ee" },
                },
                new Film
                {
                    FilmId = 4,
                    FilmCategories = new List<FilmCategory> { new FilmCategory { CategoryId = 2 } },
                    Description = "Some desc 2",
                    Title = "Some Title 2",
                    Rating = MPAA_Rating.R,
                    Language = new Language { Name = "sp" },
                },
                new Film
                {
                    FilmId = 5,
                    FilmCategories = new List<FilmCategory> { new FilmCategory { CategoryId = 1 } },
                    Description = "Some desc 5",
                    Title = "Some Title 5",
                    Rating = MPAA_Rating.PG13,
                    Language = new Language { Name = "ee" },
                },
                new Film
                {
                    FilmId = 6,
                    FilmCategories = new List<FilmCategory> { new FilmCategory { CategoryId = 1 } },
                    Description = "Some desc 5",
                    Title = "Some Title 5",
                    Rating = MPAA_Rating.PG13,
                    Language = new Language { Name = "ee" },
                },
                new Film
                {
                    FilmId = 7,
                    FilmCategories = new List<FilmCategory> { new FilmCategory { CategoryId = 1 } },
                    Description = "Some desc 5",
                    Title = "Some Title 5",
                    Rating = MPAA_Rating.PG13,
                    Language = new Language { Name = "ee" },
                },
                new Film
                {
                    FilmId = 8,
                    FilmCategories = new List<FilmCategory> { new FilmCategory { CategoryId = 1 } },
                    Description = "Some desc 5",
                    Title = "Some Title 5",
                    Rating = MPAA_Rating.PG13,
                    Language = new Language { Name = "ee" },
                },
                new Film
                {
                    FilmId = 9,
                    FilmCategories = new List<FilmCategory> { new FilmCategory { CategoryId = 1 } },
                    Description = "Some desc 5",
                    Title = "Some Title 5",
                    Rating = MPAA_Rating.PG13,
                    Language = new Language { Name = "ee" },
                },
                new Film
                {
                    FilmId = 10,
                    FilmCategories = new List<FilmCategory> { new FilmCategory { CategoryId = 1 } },
                    Description = "Some desc 5",
                    Title = "Some Title 5",
                    Rating = MPAA_Rating.PG13,
                    Language = new Language { Name = "ee" },
                },
                new Film
                {
                    FilmId = 11,
                    FilmCategories = new List<FilmCategory> { new FilmCategory { CategoryId = 1 } },
                    Description = "Some desc 5",
                    Title = "Some Title 5",
                    Rating = MPAA_Rating.PG13,
                    Language = new Language { Name = "ee" },
                },
                new Film
                {
                    FilmId = 12,
                    FilmCategories = new List<FilmCategory> { new FilmCategory { CategoryId = 1 } },
                    Description = "Some desc 5",
                    Title = "Some Title 5",
                    Rating = MPAA_Rating.PG13,
                    Language = new Language { Name = "ee" },
                },
                new Film
                {
                    FilmId = 13,
                    FilmCategories = new List<FilmCategory> { new FilmCategory { CategoryId = 1 } },
                    Description = "Some desc 5",
                    Title = "Some Title 5",
                    Rating = MPAA_Rating.PG13,
                    Language = new Language { Name = "ee" },
                },
                new Film
                {
                    FilmId = 14,
                    FilmCategories = new List<FilmCategory> { new FilmCategory { CategoryId = 1 } },
                    Description = "Some desc 5",
                    Title = "Some Title 5",
                    Rating = MPAA_Rating.PG13,
                    Language = new Language { Name = "ee" },
                },
                new Film
                {
                    FilmId = 15,
                    FilmCategories = new List<FilmCategory> { new FilmCategory { CategoryId = 1 } },
                    Description = "Some desc 5",
                    Title = "Some Title 5",
                    Rating = MPAA_Rating.PG13,
                    Language = new Language { Name = "ee" },
                },

            }.AsQueryable();


            var invetories = new List<Inventory>
            {
                new Inventory
                {
                    StoreId = 1,
                    Film = filmsList.First(),
                },
                new Inventory
                {
                    StoreId = 2,
                    Film = filmsList.Skip(1).First(),
                },
                new Inventory
                {
                    StoreId = 1,
                    Film = filmsList.Skip(2).First(),
                },
                new Inventory
                {
                    StoreId = 2,
                    Film = filmsList.Skip(3).First(),
                },
                new Inventory
                {
                    StoreId = 1,
                    Film = filmsList.Skip(4).First(),
                },
                new Inventory
                {
                    StoreId = 1,
                    Film = filmsList.Skip(5).First(),
                },
                new Inventory
                {
                    StoreId = 1,
                    Film = filmsList.Skip(6).First(),
                },
                new Inventory
                {
                    StoreId = 1,
                    Film = filmsList.Skip(7).First(),
                },
                new Inventory
                {
                    StoreId = 1,
                    Film = filmsList.Skip(8).First(),
                },
                new Inventory
                {
                    StoreId = 1,
                    Film = filmsList.Skip(9).First(),
                },
                new Inventory
                {
                    StoreId = 1,
                    Film = filmsList.Skip(10).First(),
                },
                new Inventory
                {
                    StoreId = 1,
                    Film = filmsList.Skip(11).First(),
                },
                new Inventory
                {
                    StoreId = 1,
                    Film = filmsList.Skip(12).First(),
                },
                new Inventory
                {
                    StoreId = 1,
                    Film = filmsList.Skip(13).First(),
                },
                new Inventory
                {
                    StoreId = 1,
                    Film = filmsList.Skip(14).First(),
                }
            }.AsQueryable();

            var filmActors = new List<FilmActor>
            {
                new FilmActor
                {
                    FilmId = 1
                },
                new FilmActor
                {
                    FilmId = 1
                }
            }.AsQueryable();

            var filmCategories= new List<FilmCategory>
            {
                new FilmCategory
                {
                    FilmId = 1
                },
                new FilmCategory
                {
                    FilmId = 1
                }
            }.AsQueryable();

            var actors = new List<Actor>
            {
                new Actor
                {
                    ActorId = 1
                },
                new Actor
                {
                    ActorId = 2
                }
            }.AsQueryable();

            var categories = new List<Category>
            {
                new Category
                {
                    CategoryId = 1
                },
                new Category
                {
                    CategoryId = 2
                }
            }.AsQueryable();

            var dbContextMock = new Mock<DvdRentalContext>();

            var dbSetMockFilms = new Mock<DbSet<Film>>();

            dbSetMockFilms.As<IAsyncEnumerable<Film>>()
            .Setup(x => x.GetAsyncEnumerator(default))
                .Returns(new FakeAsyncEnumerator<Film>(filmsList.GetEnumerator()));
            dbSetMockFilms.As<IQueryable<Film>>()
            .Setup(m => m.Provider)
                .Returns(new FakeAsyncQueryProvider<Film>(filmsList.Provider));
            dbSetMockFilms.As<IQueryable<Film>>()
                .Setup(m => m.Expression).Returns(filmsList.Expression);
            dbSetMockFilms.As<IQueryable<Film>>()
                .Setup(m => m.ElementType).Returns(filmsList.ElementType);
            dbSetMockFilms.As<IQueryable<Film>>()
                .Setup(m => m.GetEnumerator()).Returns(filmsList.GetEnumerator());

            dbContextMock.Setup(db => db.Films).ReturnsDbSet(dbSetMockFilms.Object);

            var dbSetMockInventories = new Mock<DbSet<Inventory>>();

            dbSetMockInventories.As<IAsyncEnumerable<Inventory>>()
                .Setup(x => x.GetAsyncEnumerator(default))
                .Returns(new FakeAsyncEnumerator<Inventory>(invetories.GetEnumerator()));
            dbSetMockInventories.As<IQueryable<Inventory>>()
                .Setup(m => m.Provider)
                .Returns(new FakeAsyncQueryProvider<Inventory>(invetories.Provider));
            dbSetMockInventories.As<IQueryable<Inventory>>()
                .Setup(m => m.Expression).Returns(invetories.Expression);
            dbSetMockInventories.As<IQueryable<Inventory>>()
                .Setup(m => m.ElementType).Returns(invetories.ElementType);
            dbSetMockInventories.As<IQueryable<Inventory>>()
                .Setup(m => m.GetEnumerator()).Returns(invetories.GetEnumerator());

            dbContextMock.Setup(db => db.Inventories).ReturnsDbSet(dbSetMockInventories.Object);

            var dbSetMockFilmActors = new Mock<DbSet<FilmActor>>();

            dbSetMockFilmActors.As<IAsyncEnumerable<FilmActor>>()
                .Setup(x => x.GetAsyncEnumerator(default))
                .Returns(new FakeAsyncEnumerator<FilmActor>(filmActors.GetEnumerator()));
            dbSetMockFilmActors.As<IQueryable<FilmActor>>()
                .Setup(m => m.Provider)
                .Returns(new FakeAsyncQueryProvider<FilmActor>(filmActors.Provider));
            dbSetMockFilmActors.As<IQueryable<FilmActor>>()
                .Setup(m => m.Expression).Returns(filmActors.Expression);
            dbSetMockFilmActors.As<IQueryable<FilmActor>>()
                .Setup(m => m.ElementType).Returns(filmActors.ElementType);
            dbSetMockFilmActors.As<IQueryable<FilmActor>>()
                .Setup(m => m.GetEnumerator()).Returns(filmActors.GetEnumerator());

            dbContextMock.Setup(db => db.FilmActors).ReturnsDbSet(dbSetMockFilmActors.Object);

            var dbSetMockFilmCategories = new Mock<DbSet<FilmCategory>>();

            dbSetMockFilmCategories.As<IAsyncEnumerable<FilmCategory>>()
                .Setup(x => x.GetAsyncEnumerator(default))
                .Returns(new FakeAsyncEnumerator<FilmCategory>(filmCategories.GetEnumerator()));
            dbSetMockFilmCategories.As<IQueryable<FilmCategory>>()
                .Setup(m => m.Provider)
                .Returns(new FakeAsyncQueryProvider<FilmCategory>(filmCategories.Provider));
            dbSetMockFilmCategories.As<IQueryable<FilmCategory>>()
                .Setup(m => m.Expression).Returns(filmCategories.Expression);
            dbSetMockFilmCategories.As<IQueryable<FilmCategory>>()
                .Setup(m => m.ElementType).Returns(filmCategories.ElementType);
            dbSetMockFilmCategories.As<IQueryable<FilmCategory>>()
                .Setup(m => m.GetEnumerator()).Returns(filmCategories.GetEnumerator());

            dbContextMock.Setup(db => db.FilmCategories).ReturnsDbSet(dbSetMockFilmCategories.Object);

            var dbSetMockCategories = new Mock<DbSet<Category>>();

            dbSetMockCategories.As<IAsyncEnumerable<Category>>()
            .Setup(x => x.GetAsyncEnumerator(default))
                .Returns(new FakeAsyncEnumerator<Category>(categories.GetEnumerator()));
            dbSetMockCategories.As<IQueryable<Category>>()
            .Setup(m => m.Provider)
                .Returns(new FakeAsyncQueryProvider<Category>(categories.Provider));
            dbSetMockCategories.As<IQueryable<Category>>()
                .Setup(m => m.Expression).Returns(categories.Expression);
            dbSetMockCategories.As<IQueryable<Category>>()
                .Setup(m => m.ElementType).Returns(categories.ElementType);
            dbSetMockCategories.As<IQueryable<Category>>()
                .Setup(m => m.GetEnumerator()).Returns(categories.GetEnumerator());

            dbContextMock.Setup(db => db.Categories).ReturnsDbSet(dbSetMockCategories.Object);

            var dbSetMockActors = new Mock<DbSet<Actor>>();

            dbSetMockActors.As<IAsyncEnumerable<Actor>>()
            .Setup(x => x.GetAsyncEnumerator(default))
                .Returns(new FakeAsyncEnumerator<Actor>(actors.GetEnumerator()));
            dbSetMockActors.As<IQueryable<Actor>>()
            .Setup(m => m.Provider)
                .Returns(new FakeAsyncQueryProvider<Actor>(actors.Provider));
            dbSetMockActors.As<IQueryable<Actor>>()
                .Setup(m => m.Expression).Returns(actors.Expression);
            dbSetMockActors.As<IQueryable<Actor>>()
                .Setup(m => m.ElementType).Returns(actors.ElementType);
            dbSetMockActors.As<IQueryable<Actor>>()
                .Setup(m => m.GetEnumerator()).Returns(actors.GetEnumerator());

            dbContextMock.Setup(db => db.Actors).ReturnsDbSet(dbSetMockActors.Object);

            return dbContextMock;
        }
    }
}
