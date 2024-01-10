using ExperimentalApp.Core.Models.Identity;
using ExperimentalApp.Core.ViewModels;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace ExperimentalApp.Core.Validators
{
    /// <summary>
    /// Represnets validation for store update view model
    /// </summary>
    public class StoreUpdateViewModelValidator : AbstractValidator<StoreUpdateViewModel>
    {
        /// <summary>
        /// Consutructor in which adusts rules and sets important services.
        /// </summary>
        /// <param name="userManager">Represents user manager service for working with application users</param>
        public StoreUpdateViewModelValidator(UserManager<ApplicationUser> userManager)
        {
            RuleFor(x => x.StoreId)
                .NotEqual(0).WithMessage("StoreId should be specified!")
                .NotNull().WithMessage("StoreId should be specified!");

            RuleFor(x => x.AddressId)
               .NotEqual(0).WithMessage("AddressId should be not 0!");

            RuleFor(x => x.StaffId)
                .MustAsync(async (staffId, cancellationToken) => await ValidationUtilities.BeValidStaffOrAdminIdAsync(userManager, staffId))
                .WithMessage("StaffId must be a valid staff or admin ID.");
        }
    }
}
