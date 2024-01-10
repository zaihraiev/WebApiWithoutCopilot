using ExperimentalApp.BusinessLogic.Interfaces;
using ExperimentalApp.Controllers;
using ExperimentalApp.Core.ViewModels;
using ExperimentalApp.UnitTests.Fakes;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExperimentalApp.UnitTests
{
    [TestFixture]
    public class FilmsControllerTests
    {
        private Mock<IFilmsService> _filmsServiceMock;
        private FilmsController _controller;

        [SetUp]
        public void SetUp()
        {
            _filmsServiceMock = new Mock<IFilmsService>();
            _controller = new FilmsController(_filmsServiceMock.Object);
        }

        [Test]
        public void GetFilms_WhenModelStateIsInvalid_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var viewModel = new FilmsFilterViewModel
            {
                CategoryId = 1
            };

            // Act
            var result = _controller.GetFilms(viewModel);

            // Asserts
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
            _filmsServiceMock.Verify(f => f.GetFilmsAsync(viewModel), Times.Never);
        }

        [Test]
        public void GetFilms_WhenModelStateIsValid_ReturnsOkObjectResult()
        {
            // Arrange
            var viewModel = new FilmsFilterViewModel
            {
                StoreId = 1,
                CategoryId = 1
            };

            // Act
            var result = _controller.GetFilms(viewModel);

            // Asserts
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            _filmsServiceMock.Verify(f => f.GetFilmsAsync(viewModel), Times.Once);
        }

        [Test]
        public void DeleteFilm_WhenResultIsFailed_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var id = 1;

            _filmsServiceMock.Setup(f => f.DeleteFilmByIdAsync(id)).Returns(Task.FromResult(false));

            // Act
            var result = _controller.DeleteFilm(id);

            // Asserts
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }

        [Test]
        public void DeleteFilm_WhenResultIsSuccess_ReturnsOktObjectResult()
        {
            // Arrange
            var id = 1;

            _filmsServiceMock.Setup(f => f.DeleteFilmByIdAsync(id)).Returns(Task.FromResult(true));

            // Act
            var result = _controller.DeleteFilm(id);

            // Asserts
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public void AddFilm_WhenModelStateIsInvalid_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var viewModel = new FilmAddViewModel
            {
                Title = "test"
            };

            // Act
            var result = _controller.AddFilm(viewModel);

            // Asserts
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
            _filmsServiceMock.Verify(f => f.AddFilmAsync(viewModel), Times.Never);
        }

        [Test]
        public void AddFilm_WhenModelStateIsValidAndAddFilmAsyncFailed_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var viewModel = new FilmAddViewModel
            {
                Title = "test",
                CategoriesIds = new List<int> { 1, 2 },
                ActorsIds = new List<int> { 1, 2 },
                LanguageId = 1
            };

            _filmsServiceMock.Setup(f => f.AddFilmAsync(viewModel)).Returns(Task.FromResult(false));

            // Act
            var result = _controller.AddFilm(viewModel);

            // Asserts
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
            _filmsServiceMock.Verify(f => f.AddFilmAsync(viewModel), Times.Once);
        }

        [Test]
        public void AddFilm_WhenModelStateIsValidAndAddFilmAsyncSuccess_ReturnsOkObjectResult()
        {
            // Arrange
            var viewModel = new FilmAddViewModel
            {
                Title = "test",
                CategoriesIds = new List<int> { 1, 2 },
                ActorsIds = new List<int> { 1, 2 },
                LanguageId = 1
            };

            _filmsServiceMock.Setup(f => f.AddFilmAsync(viewModel)).Returns(Task.FromResult(true));

            // Act
            var result = _controller.AddFilm(viewModel);

            // Asserts
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            _filmsServiceMock.Verify(f => f.AddFilmAsync(viewModel), Times.Once);
        }
    }
}
