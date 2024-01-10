using ExperimentalApp.Core.Models.Identity;
using ExperimentalApp.Core.ViewModels;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace ExperimentalApp.Core.Validators
{
    /// <summary>
    /// Represnets validation for store view model
    /// </summary>
    public class StoreViewModelValidator : AbstractValidator<StoreViewModel>
    {
        /// <summary>
        /// Consutructor in which adusts rules and sets important services.
        /// </summary>
        /// <param name="userManager">Represents user manager service for working with application users</param>
        public StoreViewModelValidator(UserManager<ApplicationUser> userManager)
        {
            RuleFor(x => x.AddressId)
                .NotEqual(0).WithMessage("AddressId should be specified!")
                .NotNull().WithMessage("AddressId should be specified!");

            RuleFor(x => x.StaffId)
                .MustAsync(async (staffId, cancellationToken) => await ValidationUtilities.BeValidStaffOrAdminIdAsync(userManager, staffId))
                .WithMessage("StaffId must be a valid staff or admin ID.");
        }
    }
}
