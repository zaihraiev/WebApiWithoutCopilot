using ExperimentalApp.Core.Constants;

namespace ExperimentalApp.Core.ViewModels
{
    /// <summary>
    /// Represents the view model for user registration.
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the confirmation password of the user.
        /// </summary>
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the store ID.
        /// </summary>
        public int? StoreId { get; set; }

        /// <summary>
        /// Gets or sets the role of the user.
        /// </summary>
        public string SelectedRole { get; set; } = RoleNames.Customer;
    }
}
