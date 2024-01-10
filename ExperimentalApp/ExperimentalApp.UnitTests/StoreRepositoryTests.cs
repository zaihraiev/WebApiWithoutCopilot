using ExperimentalApp.Core.DTOs;
using ExperimentalApp.Core.Models;
using ExperimentalApp.DataAccessLayer.DBContext;
using ExperimentalApp.DataAccessLayer.Interfaces;
using ExperimentalApp.DataAccessLayer.Repositories;
using ExperimentalApp.UnitTests.Fakes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using MockQueryable.Moq;
using Moq;
using Moq.EntityFrameworkCore;

namespace ExperimentalApp.UnitTests
{
    [TestFixture]
    public class StoreRepositoryTests
    {
        private Mock<DvdRentalContext> _context;
        private IStoreRepository _storeRepository;

        [SetUp]
        public void SetUp()
        {
            _context = GetFakeDbContext();
            _storeRepository = new StoreRepository(_context.Object);
        }

        [Test]
        public void GetStoreByIdAsync_WhenCalls_ReturnsStoreObject()
        {
            // Arange 
            _context.Setup(x => x.Stores).ReturnsDbSet(GetFakeStores().BuildMock());

            // Act
            var result = _storeRepository.GetStoreByIdAsync(2);

            // Assert
            Assert.That(result.Result.StoreId, Is.EqualTo(2));
        }

        [Test]
        public void GetStoresListAsync_WhenCalls_ReturnsCollectionOfStores()
        {
            // Arrange
            _context.Setup(x => x.Stores).ReturnsDbSet(GetFakeStores().BuildMock());

            // Act
            var result = _storeRepository.GetStoresListAsync();

            // Assert
            Assert.That(result.Result, Is.Not.Null);
            Assert.IsInstanceOf<List<Store>>(result.Result);
        }

        [Test]
        public void AddStoreAsync_WhenStoreWithAddressIsExists_Returns()
        {
            // Arrange
            var store = new Store
            {
                AddressId = 1
            };

            // Act
            _storeRepository.AddStoreAsync(store);

            // Asserts
            _context.Verify(c => c.Stores.AddAsync(store, default), Times.Never);
        }

        [Test]
        public void AddStoreAsync_WhenAllRight_AddsStore()
        {
            // Arrange
            var store = new Store
            {
                AddressId = 3
            };

            // Act
            _storeRepository.AddStoreAsync(store);

            // Asserts
            _context.Verify(c => c.Stores.AddAsync(store, default), Times.Once);
        }

        [Test]
        public void UpdateStoreAsync_WhenAllRight_UpdatesStore()
        {
            // Arrange
            var store = new Store
            {
                AddressId = 3
            };

            // Act
            _storeRepository.UpdateStoreAsync(store);

            // Asserts
            _context.Verify(c => c.Update(store), Times.Once);
        }

        private IEnumerable<Store> GetFakeStores()
        {
            return new List<Store>
            {
                new Store
                {
                    StoreId = 1,
                    AddressId = 1,
                    LastUpdate = DateTime.Now ,
                    Address = new Address {Address1 = "test"}
                },
                new Store
                {
                    StoreId = 2,
                    AddressId = 2,
                    LastUpdate = DateTime.Now ,
                    Address = new Address {Address1 = "test2"}
                }
            };
        }

        private Mock<DvdRentalContext> GetFakeDbContext()
        {
            var storeList = new List<Store>
            {
                new Store
                {
                    StoreId = 1,
                    AddressId = 1,
                },
                new Store
                {
                    StoreId = 2,
                    AddressId = 2,
                },
            }.AsQueryable();

            var dbContextMock = new Mock<DvdRentalContext>();

            var dbSetMock = new Mock<DbSet<Store>>();

            dbSetMock.As<IAsyncEnumerable<Store>>()
            .Setup(x => x.GetAsyncEnumerator(default))
                .Returns(new FakeAsyncEnumerator<Store>(storeList.GetEnumerator()));
            dbSetMock.As<IQueryable<Store>>()
            .Setup(m => m.Provider)
                .Returns(new FakeAsyncQueryProvider<Store>(storeList.Provider));
            dbSetMock.As<IQueryable<Store>>()
                .Setup(m => m.Expression).Returns(storeList.Expression);
            dbSetMock.As<IQueryable<Store>>()
                .Setup(m => m.ElementType).Returns(storeList.ElementType);
            dbSetMock.As<IQueryable<Store>>()
                .Setup(m => m.GetEnumerator()).Returns(storeList.GetEnumerator());

            dbContextMock.Setup(db => db.Stores).ReturnsDbSet(dbSetMock.Object);
            
            return dbContextMock;
        }

        private InternalEntityEntry GetInternalEntityEntry(Store testObject)
        {
            return new InternalEntityEntry(
                new Mock<IStateManager>().Object,
                new RuntimeEntityType(
                    name: nameof(Store), type: typeof(Store), sharedClrType: false, model: new(),
                    baseType: null, discriminatorProperty: null, changeTrackingStrategy: ChangeTrackingStrategy.Snapshot,
                    indexerPropertyInfo: null, propertyBag: false,
                    discriminatorValue: null),
                testObject);
        }
    }
}
