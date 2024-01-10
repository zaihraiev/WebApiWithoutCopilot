using ExperimentalApp.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace ExperimentalApp.Middleware
{
    /// <summary>
    /// Represent middleware that check whether the token is valid
    /// </summary>
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITokenService _tokenService;

        /// <summary>
        /// Consturctor with params for TokenValidationMiddleware
        /// </summary>
        /// <param name="next">Represent request processing</param>
        /// <param name="tokenService">Token service for manipulation with token</param>
        public TokenValidationMiddleware(RequestDelegate next, ITokenService tokenService)
        {
            _next = next;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Represents method that checks token is valid or not. 
        /// </summary>
        /// <param name="context">Http context</param>
        /// <returns>Returns when any condition is calls. Stop context processing</returns>
        public async Task Invoke(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            if(!IsAuthorizedEndpoint(endpoint))
            {
                await _next(context);

                return;
            }

            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                var token = context.Request.Headers["Authorization"].ToString().Split(" ").LastOrDefault();

                if (string.IsNullOrEmpty(token))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Invalid token");

                    return;
                }

                if (await _tokenService.IsTokenInBlackListAsync(token))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Token is blacklisted");

                    return;
                }
            }

            await _next(context);
        }

        /// <summary>
        /// Checks if controller endpoint contains [Authorize] attribute
        /// </summary>
        /// <param name="endpoint">Represent endpoint</param>
        /// <returns>Boolean result of operation</returns>
        private bool IsAuthorizedEndpoint(Endpoint endpoint)
        {
            if(endpoint?.Metadata?.GetMetadata<ControllerActionDescriptor>() is ControllerActionDescriptor actionDescriptor)
            {
                if (actionDescriptor.MethodInfo.GetCustomAttributes(typeof(AuthorizeAttribute), true).Length > 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
