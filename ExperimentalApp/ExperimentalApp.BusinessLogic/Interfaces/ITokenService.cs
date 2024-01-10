using Microsoft.AspNetCore.Identity;

namespace ExperimentalApp.BusinessLogic.Interfaces
{
    /// <summary>
    /// Represents the interface for working with token service.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Creates jwt token for user
        /// </summary>
        /// <param name="user">The user for whom the token will be created</param>
        /// <param name="roles">User`s roles</param>
        /// <returns>Returns generated token for user</returns>
        public string CreateJWTToken(IdentityUser user, List<string> roles);

        /// <summary>
        /// Sets token to database black list table that specified token as invalid.
        /// </summary>
        /// <param name="token">Token to set as invalid</param>
        /// <returns>Async result</returns>
        public Task SetTokenAsInvalidAsync(string token);

        /// <summary>
        /// Represents method that checks if token is in black list.
        /// </summary>
        /// <param name="token">Token to check</param>
        /// <returns>Boolean result of execution</returns>
        public Task<bool> IsTokenInBlackListAsync(string token);
    }
}
