using AutoMapper;
using ExperimentalApp.BusinessLogic.Interfaces;
using ExperimentalApp.Core.DTOs;
using ExperimentalApp.Core.Models;
using ExperimentalApp.Core.Models.Identity;
using ExperimentalApp.Core.ViewModels;
using ExperimentalApp.DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ExperimentalApp.BusinessLogic.Services
{
    /// <summary>
    /// Store service class for working with store entites
    /// </summary>
    public class StoreService : IStoreService
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Store service constructor 
        /// </summary>
        /// <param name="storeRepository">Store repository for working with db context directly</param>
        /// <param name="mapper">Represents mapper for mapping entities</param>
        /// <param name="userManager">Represents user manager service for working with application users</param>
        public StoreService(IStoreRepository storeRepository, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _storeRepository = storeRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        /// <inheritdoc/>
        public async Task<Store> GetStoreByIdAsync(int storeId)
        {
            return await _storeRepository.GetStoreByIdAsync(storeId);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<StoreResponseDTO>> GetStoresListAsync()
        {
            var stores = await _storeRepository.GetStoresListAsync();
            var storesResponse = _mapper.Map<List<StoreResponseDTO>>(stores);

            return storesResponse;
        }

        /// <inheritdoc/>
        public async Task<bool> AddStoreAsync(StoreViewModel storeViewModel)
        {
            if (!string.IsNullOrEmpty(storeViewModel.StaffId))
            {
                var user = await _userManager.FindByIdAsync(storeViewModel.StaffId);

                if(user is null)
                {
                    return false;
                }
            }

            var store = new Store
            {
                AddressId = storeViewModel.AddressId,
                ManagerStaffId = storeViewModel.StaffId
            };

            _storeRepository.AddStoreAsync(store);

            return await _storeRepository.Complete();
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateStoreAsync(StoreUpdateViewModel storeViewModel)
        {
            var store = await _storeRepository.GetStoreByIdAsync(storeViewModel.StoreId);

            if(store != null)
            {
                if (!string.IsNullOrEmpty(storeViewModel.StaffId))
                {
                    var staff = await _userManager.FindByIdAsync(storeViewModel.StaffId);

                    if (staff == null)
                    {
                        return false;
                    }

                    store.ManagerStaffId = staff.Id;
                }
                
                if(storeViewModel.AddressId != null && storeViewModel.AddressId > 0)
                {
                    store.AddressId = storeViewModel.AddressId.Value;
                }

                store.LastUpdate = DateTime.Now;

                await _storeRepository.UpdateStoreAsync(store);
            }

            return await _storeRepository.Complete();
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteStoreAsync(int storeId)
        {
            var store = await _storeRepository.GetStoreByIdAsync(storeId);

            if(store == null)
            {
                return false;
            }

            _storeRepository.DeleteStore(store);

            return await _storeRepository.Complete();
        }
    }
}
