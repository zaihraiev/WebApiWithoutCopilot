using AutoMapper;
using ExperimentalApp.BusinessLogic.Interfaces;
using ExperimentalApp.Controllers;
using ExperimentalApp.Core.DTOs;
using ExperimentalApp.Core.ViewModels;
using ExperimentalApp.UnitTests.Fakes;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ExperimentalApp.UnitTests
{
    [TestFixture]
    public class InventoryControllerTests
    {
        private InventoryController _controller;
        private Mock<IStoreService> _storeService;
        private Mock<FakeUserManager> _mockUserManager;

        [SetUp]
        public void SetUp()
        {
            _storeService = new Mock<IStoreService>();
            _mockUserManager = new Mock<FakeUserManager>();

            _controller = new InventoryController(_storeService.Object, _mockUserManager.Object);
        }

        [Test]
        public void GetStoresList_WhenCalls_ReturnsCollectionOfStores()
        {
            // Arrange 
            _storeService.Setup(s => s.GetStoresListAsync()).Returns(Task.FromResult(GetFakeStores()));

            // Act
            var result = _controller.GetStoresList();

            // Asserts
            Assert.That(result, Is.Not.Null);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public void AddStore_WhenModelStateIsInvalid_ReturnsBadRequest()
        {
            // Arrange
            var storeViewModel = new StoreViewModel
            {
                AddressId = 0
            };

            // Act
            var result = _controller.AddStore(storeViewModel);

            // Asserts
            Assert.IsInstanceOf<BadRequestResult>(result.Result);
            _storeService.Verify(s => s.AddStoreAsync(storeViewModel), Times.Never);
        }

        [Test]
        public void AddStore_WhenAddStoreAsyncFailed_ReturnsBadRequest()
        {
            // Arrange
            var storeViewModel = new StoreViewModel
            {
                AddressId = 1
            };
            _storeService.Setup(s => s.AddStoreAsync(storeViewModel)).ReturnsAsync(false);

            // Act
            var result = _controller.AddStore(storeViewModel);

            // Asserts
            Assert.IsInstanceOf<BadRequestResult>(result.Result);
            _storeService.Verify(s => s.AddStoreAsync(storeViewModel), Times.Once);
        }

        [Test]
        public void AddStore_WhenAddStoreAsyncSuccess_ReturnsOkResult()
        {
            // Arrange
            var storeViewModel = new StoreViewModel
            {
                AddressId = 1
            };
            _storeService.Setup(s => s.AddStoreAsync(storeViewModel)).ReturnsAsync(true);

            // Act
            var result = _controller.AddStore(storeViewModel);

            // Asserts
            Assert.IsInstanceOf<OkResult>(result.Result);
            _storeService.Verify(s => s.AddStoreAsync(storeViewModel), Times.Once);
        }

        [Test]
        public void UpdateStore_WhenModelStateIsInvalid_ReturnsBadRequest()
        {
            // Arrange
            var storeViewModel = new StoreUpdateViewModel
            {
                StaffId = "1"
            };

            // Act
            var result = _controller.UpdateStore(storeViewModel);

            // Asserts
            Assert.IsInstanceOf<BadRequestResult>(result.Result);
            _storeService.Verify(s => s.UpdateStoreAsync(storeViewModel), Times.Never);
        }

        [Test]
        public void UpdateStore_WhenUpdateStoreAsyncIsFailed_ReturnsBadRequest()
        {
            // Arrange
            var storeViewModel = new StoreUpdateViewModel
            {
                StoreId = 1,
            };
            _storeService.Setup(s => s.UpdateStoreAsync(storeViewModel)).ReturnsAsync(false);

            // Act
            var result = _controller.UpdateStore(storeViewModel);

            // Asserts
            Assert.IsInstanceOf<BadRequestResult>(result.Result);
            _storeService.Verify(s => s.UpdateStoreAsync(storeViewModel), Times.Once);
        }

        [Test]
        public void UpdateStore_WhenUpdateStoreAsyncSuccess_ReturnsOkResult()
        {
            // Arrange
            var storeViewModel = new StoreUpdateViewModel
            {
                StoreId = 1,
            };
            _storeService.Setup(s => s.UpdateStoreAsync(storeViewModel)).ReturnsAsync(true);

            // Act
            var result = _controller.UpdateStore(storeViewModel);

            // Asserts
            Assert.IsInstanceOf<OkResult>(result.Result);
            _storeService.Verify(s => s.UpdateStoreAsync(storeViewModel), Times.Once);
        }

        [Test]
        public void DeleteStore_WhenResultIsFalse_ReturnsBadRequest()
        {
            // Arrange
            var storeId = 1;
            _storeService.Setup(s => s.DeleteStoreAsync(storeId)).ReturnsAsync(false);

            // Act
            var result = _controller.DeleteStore(storeId);

            // Asserts
            Assert.IsInstanceOf<BadRequestResult>(result.Result);
        }

        [Test]
        public void DeleteStore_WhenResultIsTrue_ReturnsOkResult()
        {
            // Arrange
            var storeId = 1;
            _storeService.Setup(s => s.DeleteStoreAsync(storeId)).ReturnsAsync(true);

            // Act
            var result = _controller.DeleteStore(storeId);

            // Asserts
            Assert.IsInstanceOf<OkResult>(result.Result);
        }

        private IEnumerable<StoreResponseDTO> GetFakeStores()
        {
            return new List<StoreResponseDTO>
            {
                new StoreResponseDTO
                {
                    StoreId = 1,
                    ManagerStaffId = "1",
                    Address = new Core.Models.Address
                    {
                        AddressId = 1,
                        CityId = 1,
                    },
                    LastUpdate = DateTime.Now
                },
                                new StoreResponseDTO
                {
                    StoreId = 2,
                    ManagerStaffId = "2",
                    Address = new Core.Models.Address
                    {
                        AddressId = 2,
                        CityId = 2,
                    },
                    LastUpdate = DateTime.Now
                }
            };
        }
    }
}
