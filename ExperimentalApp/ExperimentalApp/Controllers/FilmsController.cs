using ExperimentalApp.BusinessLogic.Interfaces;
using ExperimentalApp.Core.Constants;
using ExperimentalApp.Core.Validators;
using ExperimentalApp.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExperimentalApp.Controllers
{
    /// <summary>
    /// Represents controller for working with films
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FilmsController : ControllerBase
    {
        private readonly IFilmsService _filmsService;

        /// <summary>
        /// Constructor with neccessary params
        /// </summary>
        /// <param name="filmsService">Film service for working with films</param>
        public FilmsController(IFilmsService filmsService) 
        {
            _filmsService = filmsService;
        }

        /// <summary>
        /// Enpoint for getting films by filter, in antorher case - just to get films.
        /// </summary>
        /// <param name="filmsFilterViewModel">Reprsents view model with params for filtering films</param>
        /// <returns>Colletion of films filtered by viewmodel field`s values</returns>
        [HttpGet]
        [Route("get-films")]
        public async Task<IActionResult> GetFilms([FromQuery] FilmsFilterViewModel filmsFilterViewModel)
        {
            var validator = new FilmsViewModelValidator();
            var validationResult = await validator.ValidateAsync(filmsFilterViewModel);

            if (validationResult.IsValid)
            {
                var result = await _filmsService.GetFilmsAsync(filmsFilterViewModel);

                return Ok(result);
            }

            return BadRequest(validationResult.Errors);
        }

        /// <summary>
        /// Enpoint for deleting film by id
        /// </summary>
        /// <param name="id">Film id to delete</param>
        /// <returns>Returns a status code depending on the result of the performed operation</returns>
        [HttpDelete]
        [Route("delete-film/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = RoleNames.Admin)]
        public async Task<IActionResult> DeleteFilm(int id)
        {
            var result = await _filmsService.DeleteFilmByIdAsync(id);

            if (result)
            {
                return Ok(SuccessConstants.SuccessfulFilmDeleted);
            }

            return BadRequest(ErrorConstants.FailedToDeleteFilm);
        }

        /// <summary>
        /// Endpoint for adding new film
        /// </summary>
        /// <param name="filmAddViewModel">Films view model with properties to set</param>
        /// <returns>Returns a status code depending on the result of the performed operation</returns>
        [HttpPost]
        [Route("add-film")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = $"{RoleNames.Admin},{RoleNames.Staff}")]
        public async Task<IActionResult> AddFilm(FilmAddViewModel filmAddViewModel)
        {
            var validator = new FilmAddViewModelValidator();
            var validationResult = await validator.ValidateAsync(filmAddViewModel);

            if (validationResult.IsValid)
            {
                var result = await _filmsService.AddFilmAsync(filmAddViewModel);

                if (result)
                {
                    return Ok(SuccessConstants.FilmCreatedSuccessfully);
                }            

                return BadRequest(ErrorConstants.FailedToCreateFilm);
            }

            return BadRequest(validationResult.Errors);
        }
    }
}
