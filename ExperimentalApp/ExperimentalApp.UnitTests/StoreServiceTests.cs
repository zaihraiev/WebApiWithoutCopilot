using AutoMapper;
using ExperimentalApp.BusinessLogic.Interfaces;
using ExperimentalApp.BusinessLogic.Services;
using ExperimentalApp.Core.DTOs;
using ExperimentalApp.Core.Models;
using ExperimentalApp.Core.Models.Identity;
using ExperimentalApp.Core.ViewModels;
using ExperimentalApp.DataAccessLayer.Interfaces;
using ExperimentalApp.UnitTests.Fakes;
using Moq;

namespace ExperimentalApp.UnitTests
{
    [TestFixture]
    public class StoreServiceTests
    {
        private Mock<IStoreRepository> _mockStoreRepository;
        private IStoreService _storeService;
        private IMapper _mapper;
        private MapperConfiguration _config;
        private Mock<FakeUserManager> _mockUserManager;

        [SetUp]
        public void SetUp()
        {
            _mockStoreRepository = new Mock<IStoreRepository>();
            _config = new MapperConfiguration(cfg => cfg.AddMaps(new[] {
                "ExperimentalApp.BusinessLogic"
             }));
            _mapper = _config.CreateMapper();
            _mockUserManager = new Mock<FakeUserManager>();

            _storeService = new StoreService(_mockStoreRepository.Object, _mapper, _mockUserManager.Object);
        }

        [Test]
        public void GetStoreByIdAsync_WhenCalls_ReturnsStoreObject()
        {
            // Arange 
            _mockStoreRepository.Setup(x => x.GetStoreByIdAsync(It.IsAny<int>())).ReturnsAsync(new Store());

            // Act
            var result = _storeService.GetStoreByIdAsync(1);

            // Assert
            Assert.IsInstanceOf<Store>(result.Result);
        }

        [Test]
        public void GetStoresListAsync_WhenCalls_ReturnsCollectionOfStores()
        {
            // Arrange
            _mockStoreRepository.Setup(s => s.GetStoresListAsync()).Returns(Task.FromResult(GetFakeStores()));

            // Act
            var result = _storeService.GetStoresListAsync();

            // Assert
            Assert.That(result.Result, Is.Not.Null);
            Assert.IsInstanceOf<List<StoreResponseDTO>>(result.Result);
        }

        [Test]
        public void AddStoreAsync_WhenSpecifiedUserIsNotFound_ReturnsFalse()
        {
            // Arrange
            var storeViewModel = new StoreViewModel
            {
                AddressId = 1,
                StaffId = "2"
            };

            // Act
            var result = _storeService.AddStoreAsync(storeViewModel);

            // Arrange
            Assert.That(result.Result, Is.EqualTo(false));
            _mockStoreRepository.Verify(s => s.AddStoreAsync(It.IsAny<Store>()), Times.Never);
        }

        [Test]
        public void AddStoreAsync_WhenAddStoreAsyncFailed_ReturnsFalse()
        {
            // Arrange
            var storeViewModel = new StoreViewModel
            {
                AddressId = 1
            };

            _mockStoreRepository.Setup(s => s.Complete()).Returns(Task.FromResult(false));

            // Act
            var result = _storeService.AddStoreAsync(storeViewModel);

            // Asserts
            Assert.That(result.Result, Is.EqualTo(false));
        }

        [Test]
        public void AddStoreAsync_WhenAddStoreAsyncSuccess_ReturnsTrue()
        {
            // Arrange
            var storeViewModel = new StoreViewModel
            {
                AddressId = 1
            };

            _mockStoreRepository.Setup(s => s.Complete()).Returns(Task.FromResult(true));

            // Act
            var result = _storeService.AddStoreAsync(storeViewModel);

            // Asserts
            Assert.That(result.Result, Is.EqualTo(true));
        }

        [Test]
        public void UpdateStoreAsync_WhenStoreIsNull_ReturnsFalse()
        {
            // Arrange
            var storeViewModel = new StoreUpdateViewModel
            {
                StoreId = 1,
                AddressId = 1
            };

            _mockStoreRepository.Setup(s => s.GetStoreByIdAsync(storeViewModel.StoreId)).ReturnsAsync((Store)null);
            _mockStoreRepository.Setup(s => s.Complete()).Returns(Task.FromResult(false));

            // Act
            var result = _storeService.UpdateStoreAsync(storeViewModel);

            // Arrange
            _mockStoreRepository.Verify(s => s.UpdateStoreAsync(It.IsAny<Store>()), Times.Never);
            Assert.IsFalse(result.Result);
        }

        [Test]
        public void UpdateStoreAsync_WhenSpecifiedStaffIsNotFound_ReturnsFalse()
        {
            // Arrange
            var storeViewModel = new StoreUpdateViewModel
            {
                StoreId = 1,
                AddressId = 1,
                StaffId = "1"
            };

            _mockStoreRepository.Setup(s => s.GetStoreByIdAsync(storeViewModel.StoreId)).ReturnsAsync((Store)null);
            _mockUserManager.Setup(u => u.FindByIdAsync(storeViewModel.StaffId)).ReturnsAsync((ApplicationUser)null);
            _mockStoreRepository.Setup(s => s.Complete()).Returns(Task.FromResult(false));

            // Act
            var result = _storeService.UpdateStoreAsync(storeViewModel);

            // Asserts
            _mockStoreRepository.Verify(s => s.UpdateStoreAsync(It.IsAny<Store>()), Times.Never);
            Assert.That(result.Result, Is.EqualTo(false));
        }

        [Test]
        public void UpdateStoreAsync_WhenAddressIdIsCorrect_SetsStoreAddressIdFromVM()
        {
            // Arrange
            var storeViewModel = new StoreUpdateViewModel
            {
                StoreId = 1,
                AddressId = 1,
                StaffId = "1"
            };

            var store = new Store
            {
                AddressId = 2
            };

            _mockStoreRepository.Setup(s => s.GetStoreByIdAsync(storeViewModel.StoreId)).ReturnsAsync(store);
            _mockUserManager.Setup(u => u.FindByIdAsync(storeViewModel.StaffId)).ReturnsAsync(new ApplicationUser());

            // Act
            var result = _storeService.UpdateStoreAsync(storeViewModel);

            // Asserts
            Assert.That(store.AddressId, Is.EqualTo(storeViewModel.AddressId));
        }

        [Test]
        public void UpdateStoreAsync_WhenUpdateStoreAsyncFailed_ReturnsFalse()
        {
            // Arrange
            var storeViewModel = new StoreUpdateViewModel
            {
                StoreId = 1,
                AddressId = 1,
                StaffId = "1"
            };

            _mockStoreRepository.Setup(s => s.GetStoreByIdAsync(storeViewModel.StoreId)).ReturnsAsync(new Store());
            _mockUserManager.Setup(u => u.FindByIdAsync(storeViewModel.StaffId)).ReturnsAsync(new ApplicationUser());
            _mockStoreRepository.Setup(s => s.Complete()).Returns(Task.FromResult(false));

            // Act
            var result = _storeService.UpdateStoreAsync(storeViewModel);

            // Asserts
            _mockStoreRepository.Verify(s => s.UpdateStoreAsync(It.IsAny<Store>()), Times.Once);
            Assert.IsFalse(result.Result);
        }

        [Test]
        public void UpdateStoreAsync_WhenUpdateStoreAsyncSuccess_ReturnsTrue()
        {
            // Arrange
            var storeViewModel = new StoreUpdateViewModel
            {
                StoreId = 1,
                AddressId = 1,
                StaffId = "1"
            };

            _mockStoreRepository.Setup(s => s.GetStoreByIdAsync(storeViewModel.StoreId)).ReturnsAsync(new Store());
            _mockUserManager.Setup(u => u.FindByIdAsync(storeViewModel.StaffId)).ReturnsAsync(new ApplicationUser());
            _mockStoreRepository.Setup(s => s.Complete()).Returns(Task.FromResult(true));

            // Act
            var result = _storeService.UpdateStoreAsync(storeViewModel);

            // Asserts
            _mockStoreRepository.Verify(s => s.UpdateStoreAsync(It.IsAny<Store>()), Times.Once);
            Assert.IsTrue(result.Result);
        }

        [Test]
        public void UpdateStoreAsync_WhenAddressIdInViewModelNull_SetsSameIdentifier()
        {
            // Arrange
            var storeViewModel = new StoreUpdateViewModel
            {
                StoreId = 1,
                AddressId = null,
                StaffId = "1"
            };
            var existingStore = new Store { AddressId = 3 };
            _mockStoreRepository.Setup(s => s.GetStoreByIdAsync(storeViewModel.StoreId)).ReturnsAsync(existingStore);
            _mockUserManager.Setup(u => u.FindByIdAsync(storeViewModel.StaffId)).ReturnsAsync(new ApplicationUser());
            _mockStoreRepository.Setup(s => s.Complete()).Returns(Task.FromResult(true));

            // Act
            var result = _storeService.UpdateStoreAsync(storeViewModel);

            // Asserts
            _mockStoreRepository.Verify(s => s.UpdateStoreAsync(It.IsAny<Store>()), Times.Once);
            Assert.AreNotEqual(existingStore.AddressId, storeViewModel.AddressId);
            Assert.IsTrue(result.Result);
        }

        [Test]
        public void UpdateStoreAsync_WhenAddressIdInViewModelNotNull_SetsViewModelValue()
        {
            // Arrange
            var storeViewModel = new StoreUpdateViewModel
            {
                StoreId = 1,
                AddressId = 2,
                StaffId = "1"
            };
            var existingStore = new Store { AddressId = 3 };
            _mockStoreRepository.Setup(s => s.GetStoreByIdAsync(storeViewModel.StoreId)).ReturnsAsync(existingStore);
            _mockUserManager.Setup(u => u.FindByIdAsync(storeViewModel.StaffId)).ReturnsAsync(new ApplicationUser());
            _mockStoreRepository.Setup(s => s.Complete()).Returns(Task.FromResult(true));

            // Act
            var result = _storeService.UpdateStoreAsync(storeViewModel);

            // Asserts
            _mockStoreRepository.Verify(s => s.UpdateStoreAsync(It.IsAny<Store>()), Times.Once);
            Assert.AreEqual(existingStore.AddressId, storeViewModel.AddressId);
            Assert.IsTrue(result.Result);
        }

        [Test]
        public void DeleteStoreAsync_WhenStoreIsNull_ReturnsFalse()
        {
            // Arrange
            var storeId = 1;

            _mockStoreRepository.Setup(s => s.GetStoreByIdAsync(storeId)).ReturnsAsync((Store)null);

            // Act
            var result = _storeService.DeleteStoreAsync(storeId);

            // Asserts
            Assert.IsFalse(result.Result);
            _mockStoreRepository.Verify(s => s.DeleteStore(It.IsAny<Store>()), Times.Never);
        }

        [Test]
        public void DeleteStoreAsync_WhenStoreIsNotNullAndDeleteStoreFailed_ReturnsFalse()
        {
            // Arrange
            var storeId = 1;

            _mockStoreRepository.Setup(s => s.GetStoreByIdAsync(storeId)).ReturnsAsync(new Store());
            _mockStoreRepository.Setup(s => s.Complete()).ReturnsAsync(false);

            // Act
            var result = _storeService.DeleteStoreAsync(storeId);

            // Asserts
            Assert.IsFalse(result.Result);
            _mockStoreRepository.Verify(s => s.DeleteStore(It.IsAny<Store>()), Times.Once);
        }

        [Test]
        public void DeleteStoreAsync_WhenStoreIsNotNullAndDeleteStoreSuccess_ReturnsTrue()
        {
            // Arrange
            var storeId = 1;

            _mockStoreRepository.Setup(s => s.GetStoreByIdAsync(storeId)).ReturnsAsync(new Store());
            _mockStoreRepository.Setup(s => s.Complete()).ReturnsAsync(true);

            // Act
            var result = _storeService.DeleteStoreAsync(storeId);

            // Asserts
            Assert.IsTrue(result.Result);
            _mockStoreRepository.Verify(s => s.DeleteStore(It.IsAny<Store>()), Times.Once);
        }

        private IEnumerable<Store> GetFakeStores()
        {
            return new List<Store>
            {
                new Store
                {
                    StoreId = 1,
                    ManagerStaffId = "1",
                    Address = new Core.Models.Address
                    {
                        AddressId = 1,
                        CityId = 1,
                    },
                    LastUpdate = DateTime.Now,
                    AddressId = 1,
                },
                new Store
                {
                    StoreId = 2,
                    ManagerStaffId = "2",
                    Address = new Core.Models.Address
                    {
                        AddressId = 2,
                        CityId = 2,
                    },
                    LastUpdate = DateTime.Now,
                    AddressId = 2
                }
            };
        }    
    }
}
