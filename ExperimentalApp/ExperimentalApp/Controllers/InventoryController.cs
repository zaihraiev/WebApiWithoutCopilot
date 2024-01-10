using ExperimentalApp.BusinessLogic.Interfaces;
using ExperimentalApp.Core.Constants;
using ExperimentalApp.Core.Models.Identity;
using ExperimentalApp.Core.Validators;
using ExperimentalApp.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExperimentalApp.Controllers
{
    /// <summary>
    /// Represents controller for working with inventory.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class InventoryController : ControllerBase
    {
        private readonly IStoreService _storeService;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Consturcotr with params
        /// </summary>
        /// <param name="storeService">Represents store service for working with stores</param>
        /// <param name="userManager">Represents user manager service for working with application users</param>
        public InventoryController(IStoreService storeService, UserManager<ApplicationUser> userManager)
        {
            _storeService = storeService;
            _userManager = userManager;
        }

        /// <summary>
        /// Represents endpoint to get all stores or empty list of stores. 
        /// </summary>
        /// <returns>Collection of stores</returns>
        [HttpGet]
        [Route("stores-list")]
        public async Task<IActionResult> GetStoresList() 
        {
            var stores = await _storeService.GetStoresListAsync();

            return Ok(stores);
        }

        /// <summary>
        /// Represents endpoint for adding new store
        /// </summary>
        /// <param name="storeViewModel">Store view model</param>
        /// <returns>Boolean result of operation</returns>
        [Authorize(Roles = RoleNames.Admin)]
        [HttpPost]
        [Route("add-store")]
        public async Task<IActionResult> AddStore(StoreViewModel storeViewModel) 
        {
            var validator = new StoreViewModelValidator(_userManager);
            var validationResult = await validator.ValidateAsync(storeViewModel);

            if (validationResult.IsValid)
            {
                var result = await _storeService.AddStoreAsync(storeViewModel);

                if (result)
                {
                    return Ok();
                }
            }

            return BadRequest();
        }

        /// <summary>
        /// Represents endpoint for updating store
        /// </summary>
        /// <param name="storeViewModel">Store params to update</param>
        /// <returns>Returns result of execution as status code</returns>
        [HttpPut]
        [Route("update-store")]
        public async Task<IActionResult> UpdateStore(StoreUpdateViewModel storeViewModel)
        {
            var validator = new StoreUpdateViewModelValidator(_userManager);
            var validationResult = await validator.ValidateAsync(storeViewModel);

            if (validationResult.IsValid)
            {
                var result = await _storeService.UpdateStoreAsync(storeViewModel);

                if (result)
                {
                    return Ok();
                }
            }

            return BadRequest();
        }

        /// <summary>
        /// Represents endpoint for deleting store
        /// </summary>
        /// <param name="storeId">Store id to delete</param>
        /// <returns>Status code result of execution</returns>
        [HttpDelete]
        [Authorize(Roles = RoleNames.Admin)]
        [Route("delete-store")]
        public async Task<IActionResult> DeleteStore(int storeId)
        {
            var result = await _storeService.DeleteStoreAsync(storeId);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
