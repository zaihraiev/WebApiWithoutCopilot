using ExperimentalApp.Core.ViewModels;
using FluentValidation;

namespace ExperimentalApp.Core.Validators
{
    /// <summary>
    /// Represents validator class for checking film add view model
    /// </summary>
    public class FilmAddViewModelValidator : AbstractValidator<FilmAddViewModel>
    {
        /// <summary>
        /// Validator constructor with rules for film view model 
        /// </summary>
        public FilmAddViewModelValidator() 
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title should be specified!");

            RuleFor(x => x.LanguageId)
                .NotEmpty().WithMessage("LanguageId should be specified!")
                .GreaterThan(0).WithMessage("LanguageId should be correct!");

            RuleForEach(f => f.CategoriesIds)
                .NotEmpty()
                .GreaterThan(0);

            RuleForEach(f => f.ActorsIds)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(x => x.ReleaseYear)
                .NotEqual(0)
                .Must(year => year > 1900 && year <= DateTime.Now.Year)
                .When(x => x.ReleaseYear != null && x.ReleaseYear != 0)
                .WithMessage("Provide a valid release year.");
        }
    }
}
