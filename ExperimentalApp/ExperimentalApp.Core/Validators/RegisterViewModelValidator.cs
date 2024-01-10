using ExperimentalApp.Core.Constants;
using ExperimentalApp.Core.ViewModels;
using FluentValidation;

namespace ExperimentalApp.Core.Validators
{
    /// <summary>
    /// Validation for register view model
    /// </summary>
    public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
    {
        /// <summary>
        /// Represents a rules for register view model fields.
        /// </summary>
        public RegisterViewModelValidator()
        {
            RuleFor(model => model.Email)
                .NotEmpty().WithMessage(ErrorConstants.EmailAddressIsRequired)
                .EmailAddress().WithMessage(ErrorConstants.EmailAddressFormat);

            RuleFor(model => model.FirstName)
                .NotEmpty().WithMessage(ErrorConstants.FirstNameIsRequired);

            RuleFor(model => model.LastName)
                .NotEmpty().WithMessage(ErrorConstants.LastNameIsRequired);

            RuleFor(model => model.Password)
                .NotEmpty().WithMessage(ErrorConstants.PasswordIsRequired)
                .MinimumLength(6).WithMessage(ErrorConstants.PasswordMinLength)
                .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$")
                .WithMessage(ErrorConstants.StrongPassword);

            RuleFor(model => model.ConfirmPassword)
                .Equal(model => model.Password).WithMessage(ErrorConstants.SameConfirmPassword)
                .When(model => !string.IsNullOrEmpty(model.Password));

            RuleFor(model => model.UserName)
                .NotEmpty().WithMessage(ErrorConstants.UserNameIsRequired);

            RuleFor(model => model.SelectedRole)
                .Equal(RoleNames.Staff)
                .When(model => model.SelectedRole == RoleNames.Staff);

            RuleFor(model => model.StoreId)
                .NotNull().WithMessage(ErrorConstants.StoreIdRequired)
                .NotEqual(0).WithMessage(ErrorConstants.StoreIdNotDefault)
                .When(model => model.SelectedRole == RoleNames.Staff)
                .WithMessage(ErrorConstants.StoreIdRequired);
        }
    }
}
