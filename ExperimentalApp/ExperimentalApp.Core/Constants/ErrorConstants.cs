namespace ExperimentalApp.Core.Constants
{
    /// <summary>
    /// Represents error messages in the application
    /// </summary>
    public class ErrorConstants
    {
        public const string FailedToAssignRole = "Failed to assign role to user";
        public const string RoleNotExist = "Role does not exist";
        public const string InsufficientPermissions = "Insufficient permissions";
        public const string UserNotFound = "User not found";
        public const string StoreIdRequired = "Store ID is required to fill for staff";
        public const string StoreIdNotDefault = "StoreId cannot be 0";
        public const string SelectedRoleDoesNotExistDefaultAssigned = $"The selected role does not exist, the default role was assigned: {RoleNames.Customer}";

        public const string EmailAddressIsRequired = "Email address is required.";
        public const string EmailAddressFormat = "Invalid email address format.";
        public const string FirstNameIsRequired = "First name is required.";
        public const string LastNameIsRequired = "Last name is required.";
        public const string PasswordIsRequired = "Password is required";
        public const string PasswordMinLength = "Password should contain more than 6 characters.";
        public const string StrongPassword = "The password should be strong.";
        public const string SameConfirmPassword = "Password and confirmation password do not match.";
        public const string UserNameIsRequired = "Username is required.";

        public const string FailedToRejectRole = "Failed to revoke user`s role";
        public const string SuccessfullyRejectRoleAndStoreUpdate = "User store was successfully updated. Role was rejected.";
        public const string SuccessfullyRejectedRole = "User role was successfully rejected.";

        public const string FailedToCreateFilm = "Failed to create film";
        public const string FailedToDeleteFilm = "Failed to delete film";
    }
}
