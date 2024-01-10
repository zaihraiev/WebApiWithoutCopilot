using Microsoft.AspNetCore.Identity;

namespace ExperimentalApp.Core.Models.Identity
{
    /// <summary>
    /// Represents an application user.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets the store ID associated with the user.
        /// </summary>
        public int? StoreId { get; set; }

        /// <summary>
        /// Gets or sets the store associated with the user.
        /// </summary>
        public virtual Store? Store { get; set; }
    }
}
