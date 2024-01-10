using ExperimentalApp.Core.Constants;
using ExperimentalApp.Core.ViewModels;
using FluentValidation;

namespace ExperimentalApp.Core.Validators
{
    /// <summary>
    /// Represnets validation for films filter view model
    /// </summary>
    public class FilmsViewModelValidator : AbstractValidator<FilmsFilterViewModel>
    {
        /// <summary>
        /// Represents a rules for films filter view model fields.
        /// </summary>
        public FilmsViewModelValidator()
        {
            RuleFor(x => x.StoreId)
               .NotEqual(0).WithMessage("StoreId should be specified!")
               .NotNull().WithMessage("StoreId should be specified!");
        }
    }
}
