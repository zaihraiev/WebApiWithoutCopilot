using ExperimentalApp.Core.DTOs;
using ExperimentalApp.Core.Models;
using ExperimentalApp.Core.ViewModels;

namespace ExperimentalApp.BusinessLogic.Interfaces
{
    /// <summary>
    /// Represents interface for working with store service
    /// </summary>
    public interface IStoreService
    {
        /// <summary>
        /// Represent method for getting store by id
        /// </summary>
        /// <param name="storeId">Store id to search</param>
        /// <returns>Store searched by id</returns>
        public Task<Store> GetStoreByIdAsync(int storeId);

        /// <summary>
        /// Represents method for getting stores list
        /// </summary>
        /// <returns>Stores list with specific fields</returns>
        public Task<IEnumerable<StoreResponseDTO>> GetStoresListAsync();

        /// <summary>
        /// Represents method for adding new store to database
        /// </summary>
        /// <param name="storeViewModel">Store view model</param>
        /// <returns>Boolean result of operation</returns>
        public Task<bool> AddStoreAsync(StoreViewModel storeViewModel);

        /// <summary>
        /// Represents method for updating store
        /// </summary>
        /// <param name="storeViewModel">Values to update</param>
        /// <returns>Boolean result of operation</returns>
        public Task<bool> UpdateStoreAsync(StoreUpdateViewModel storeViewModel);

        /// <summary>
        /// Represnets method for deleting store
        /// </summary>
        /// <param name="storeId">Store id to delete</param>
        /// <returns>Boolean result of operation</returns>
        public Task<bool> DeleteStoreAsync(int storeId);
    }
}
