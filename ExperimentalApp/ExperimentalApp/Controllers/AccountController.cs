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
    /// Represent controller for managing user`s accounts. 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IStoreService _storeService;
        private readonly ITokenService _tokenService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="roleManager">The role manager.</param>
        /// <param name="storeService">The store service.</param>
        /// <param name="tokenService">Service for token managing</param>
        public AccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IStoreService storeService, ITokenService tokenService)   
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _storeService = storeService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="registerViewModel">The register view model.</param>
        /// <returns>The result of the registration.</returns>
        [HttpPost]
        [Route("register")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = $"{RoleNames.Admin},{RoleNames.Staff}")]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            var validator = new RegisterViewModelValidator();
            var validationResult = await validator.ValidateAsync(registerViewModel);

            if (validationResult.IsValid)
            {
                var storeIsExist = await CheckIfStoreIsExistsAsync(registerViewModel.StoreId.GetValueOrDefault());

                var user = new ApplicationUser
                {
                    UserName = registerViewModel.UserName,
                    Email = registerViewModel.Email,
                    FirstName = registerViewModel.FirstName,
                    LastName = registerViewModel.LastName,
                    StoreId = storeIsExist ? registerViewModel.StoreId : null,
                };

                var identityResult = await _userManager.CreateAsync(user, registerViewModel.Password);

                if(identityResult.Succeeded)
                {
                    var role = await _roleManager.FindByNameAsync(registerViewModel.SelectedRole);

                    if (role == null)
                    {
                        role = await _roleManager.FindByNameAsync(RoleNames.Customer);
                        ModelState.AddModelError(string.Empty, ErrorConstants.SelectedRoleDoesNotExistDefaultAssigned);
                    }

                    var createdUser = await _userManager.FindByNameAsync(user.UserName);
                    var result = await AssignRole(createdUser.Id , role.Id);

                    if(result is OkObjectResult)
                    {
                        return Ok(SuccessConstants.UserWasRegistered);
                    }
                    ModelState.AddModelError(string.Empty, ErrorConstants.FailedToAssignRole);
                }

                AddErrors(identityResult);
            }
            else
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Represents the login action.
        /// </summary>
        /// <param name="loginViewModel">Represents login view model</param>
        /// <returns>Returns the user who is logged in</returns>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            var user = await _userManager.FindByEmailAsync(loginViewModel.Email);

            if(user != null) 
            {
                var checkPassword = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);

                if (checkPassword)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);

                    var jwtToken = _tokenService.CreateJWTToken(user, userRoles.ToList());

                    var response = new LoginResponseViewModel
                    {
                        JwtToken = jwtToken
                    };

                    return Ok(response);
                }
            }

            return BadRequest();
        }

        /// <summary>
        /// Represents the logout action.
        /// </summary>
        /// <returns>Status code</returns>
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> LogOut()
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ").LastOrDefault();

            await _tokenService.SetTokenAsInvalidAsync(token);

            return Ok();
        }
                 
        /// <summary>
        /// Represents the action for assign specific role to specific user.
        /// </summary>
        /// <param name="userId">User ID that role will be assigned</param>
        /// <param name="roleId">Role ID that will be assigned</param>
        /// <returns>Returns a status code depending on the result of the performed operation</returns>
        [Authorize]
        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole(string userId, string roleId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(ErrorConstants.UserNotFound);
            }

            var currentUserRoles = await _userManager.GetRolesAsync(await _userManager.GetUserAsync(HttpContext.User));
            if (!currentUserRoles.Contains(RoleNames.Admin) && !currentUserRoles.Contains(RoleNames.Staff))
            {
                return Forbid(ErrorConstants.InsufficientPermissions);
            }

            var role = await _roleManager.FindByIdAsync(roleId);  
            if (role == null)
            {
                return BadRequest(ErrorConstants.RoleNotExist);
            }

            if (currentUserRoles.Contains(RoleNames.Staff) && role.Name == RoleNames.Admin)
            {
                return Forbid(ErrorConstants.FailedToAssignRole);
            }

            await _userManager.AddToRoleAsync(user, role.Name);

            return Ok(SuccessConstants.SuccessfulRoleAssigned);
        }

        /// <summary>
        /// Represents endpoint for revoking user`s role
        /// </summary>
        /// <param name="userId">User id whose role will be deleted</param>
        /// <param name="roleName">Role to delete</param>
        /// <returns>Returns a status code depending on the result of the performed operation</returns>
        [Authorize(AuthenticationSchemes = "Bearer", Roles = $"{RoleNames.Admin},{RoleNames.Staff}")]
        [HttpPost("reject-roles")]
        public async Task<IActionResult> RejectRoles(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(ErrorConstants.UserNotFound);
            }

            var currentUserRoles = await _userManager.GetRolesAsync(await _userManager.GetUserAsync(HttpContext.User));
            if (!currentUserRoles.Contains(RoleNames.Admin) && !currentUserRoles.Contains(RoleNames.Staff))
            {
                return Forbid(ErrorConstants.InsufficientPermissions);
            }

            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return BadRequest(ErrorConstants.RoleNotExist);
            }

            if (currentUserRoles.Contains(RoleNames.Staff) && (role.Name == RoleNames.Admin || role.Name == RoleNames.Staff))
            {
                return Forbid(ErrorConstants.FailedToRejectRole);
            }

            var resultRemove = await _userManager.RemoveFromRoleAsync(user, role.Name);

            if(role.Name == RoleNames.Staff && resultRemove.Succeeded) 
            {
                user.StoreId = null;
                var resultUpdate = await _userManager.UpdateAsync(user);

                if(resultUpdate.Succeeded) 
                {
                    return Ok(ErrorConstants.SuccessfullyRejectRoleAndStoreUpdate);
                }
            }

            if(resultRemove.Succeeded) 
            {
                return Ok(ErrorConstants.SuccessfullyRejectedRole);
            }

            return BadRequest(ErrorConstants.FailedToRejectRole);
        }

        /// <summary>
        /// Represents endpoint for changing user`s roles
        /// </summary>
        /// <param name="userId">User id whose role will be changed</param>
        /// <param name="roleId">Role id to assign</param>
        /// <returns>Returns a status code depending on the result of the performed operation</returns>
        [HttpPost]
        [Route("change-user-role")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = RoleNames.Admin)]
        public async Task<IActionResult> ChangeUserRole(string userId, string roleId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(ErrorConstants.UserNotFound);
            }

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return BadRequest(ErrorConstants.RoleNotExist);
            }

            var userRolesToDelete = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRolesToDelete);

            var result = await _userManager.AddToRoleAsync(user, role.Name);

            if(result.Succeeded)
            {
                if (userRolesToDelete.Contains(RoleNames.Staff))
                {
                    user.StoreId = null;
                    await _userManager.UpdateAsync(user);
                }

                return Ok(SuccessConstants.SuccessfulRoleAssigned);
            }

            AddErrors(result);

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Handle errors from user registration.
        /// </summary>
        /// <param name="result">Result of the request from the Action</param>
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        /// <summary>
        /// Checks if store is exists
        /// </summary>
        /// <param name="storeId">Store id to chekc</param>
        /// <returns>Boolean result of operation</returns>
        private async Task<bool> CheckIfStoreIsExistsAsync(int storeId)
        {
            return await _storeService.GetStoreByIdAsync(storeId) != null;
        }
    }
}
