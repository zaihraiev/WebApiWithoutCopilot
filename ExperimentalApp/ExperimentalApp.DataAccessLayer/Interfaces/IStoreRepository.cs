using ExperimentalApp.Core.Models;

namespace ExperimentalApp.DataAccessLayer.Interfaces
{
    /// <summary>
    /// Represents interface for working with store repository
    /// </summary>
    public interface IStoreRepository
    {
        /// <summary>
        /// The method that gets store by id from the data base by using db context.
        /// </summary>
        /// <param name="storeId">Store id for searching specific store</param>
        /// <returns>Store that founded by id</returns>
        public Task<Store> GetStoreByIdAsync(int storeId);

        /// <summary>
        /// The method that gets stores from the data base by using db context.
        /// </summary>
        /// <returns>Stores list</returns>
        public Task<IEnumerable<Store>> GetStoresListAsync();

        /// <summary>
        /// The method that adds new store.
        /// </summary>
        /// <param name="store">Store that should be added</param>
        public Task AddStoreAsync(Store store);

        /// <summary>
        /// Represents method that save changes.
        /// </summary>
        /// <returns>Boolean result of operation</returns>
        public Task<bool> Complete();

        /// <summary>
        /// Represents method for updating store in database
        /// </summary>
        /// <param name="store">Store to update</param>
        public Task UpdateStoreAsync(Store store);

        /// <summary>
        /// Represents method for deleting specific store.
        /// </summary>
        /// <param name="store">Store to delete</param>
        public void DeleteStore(Store store);
    }
}
