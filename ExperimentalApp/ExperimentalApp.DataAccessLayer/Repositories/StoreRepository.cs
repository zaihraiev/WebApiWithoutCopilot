using ExperimentalApp.Core.Models;
using ExperimentalApp.DataAccessLayer.DBContext;
using ExperimentalApp.DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExperimentalApp.DataAccessLayer.Repositories
{
    /// <summary>
    /// Represents class for working with stores entities by using data base context
    /// </summary>
    public class StoreRepository : IStoreRepository
    {
        private readonly DvdRentalContext _context;

        /// <summary>
        /// Constructor for store repository with params
        /// </summary>
        /// <param name="context">Data base context for working with entity</param>
        public StoreRepository(DvdRentalContext context) 
        { 
            _context = context;
        }

        /// <inheritdoc/>
        public async Task AddStoreAsync(Store store)
        {
            if(await _context.Stores.AnyAsync(s => s.AddressId == store.AddressId))
            {
                return;
            }

            await _context.Stores.AddAsync(store);
        }

        /// <inheritdoc/>
        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        /// <inheritdoc/>
        public async Task<Store> GetStoreByIdAsync(int storeId)
        {
            return await _context.Stores.Where(s => s.StoreId == storeId).FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Store>> GetStoresListAsync()
        {
            return await _context.Stores.Include(s => s.Address).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateStoreAsync(Store store)
        {
            _context.Update(store);
        }

        /// <inheritdoc/>
        public void DeleteStore(Store store)
        {
            _context.Stores.Remove(store);
        }
    }
}
