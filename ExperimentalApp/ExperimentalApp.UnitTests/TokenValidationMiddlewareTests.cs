using ExperimentalApp.BusinessLogic.Interfaces;
using ExperimentalApp.Controllers;
using ExperimentalApp.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Moq;

namespace ExperimentalApp.UnitTests
{
    [TestFixture]
    public class TokenValidationMiddlewareTests
    {
        [Test]
        public async Task Invoke_NonAuthorizedEndpoint_CallsNext()
        {
            // Arrange
            var nextDelegate = new Mock<RequestDelegate>();
            var tokenServiceMock = new Mock<ITokenService>();

            var middleware = new TokenValidationMiddleware(nextDelegate.Object, tokenServiceMock.Object);
            var context = new DefaultHttpContext();
            context.SetEndpoint(new Endpoint(nextDelegate.Object, new EndpointMetadataCollection(), "test"));

            // Act
            await middleware.Invoke(context);

            // Assert
            nextDelegate.Verify(next => next(context), Times.Once);
            Assert.That(context.Response.StatusCode, Is.Not.EqualTo(StatusCodes.Status401Unauthorized));
        }

        [Test]
        public async Task Invoke_AuthorizedEndpointWithoutToken_ReturnsUnauthorized()
        {
            // Arrange
            var nextDelegate = new Mock<RequestDelegate>();
            var tokenServiceMock = new Mock<ITokenService>();

            var middleware = new TokenValidationMiddleware(nextDelegate.Object, tokenServiceMock.Object);
            var context = new DefaultHttpContext();
            var actionDescriptor = new ControllerActionDescriptor { MethodInfo = typeof(AccountController).GetMethod("LogOut") };

            context.SetEndpoint(new Endpoint(nextDelegate.Object, new EndpointMetadataCollection(actionDescriptor), "test"));
            context.Request.Headers["Authorization"] = string.Empty;

            // Act
            await middleware.Invoke(context);

            // Assert
            Assert.That(StatusCodes.Status401Unauthorized, Is.EqualTo(context.Response.StatusCode));
        }

        [Test]
        public async Task Invoke_WhenTokenInBlackList_ReturnsUnauthorized()
        {
            // Arrange
            var nextDelegate = new Mock<RequestDelegate>();
            var tokenServiceMock = new Mock<ITokenService>();

            tokenServiceMock.Setup(x => x.IsTokenInBlackListAsync(It.IsAny<string>())).Returns(Task.FromResult(true));

            var middleware = new TokenValidationMiddleware(nextDelegate.Object, tokenServiceMock.Object);
            var context = new DefaultHttpContext();
            var actionDescriptor = new ControllerActionDescriptor { MethodInfo = typeof(AccountController).GetMethod("LogOut") };

            context.SetEndpoint(new Endpoint(nextDelegate.Object, new EndpointMetadataCollection(actionDescriptor), "test"));
            context.Request.Headers["Authorization"] = "Bearer test";          

            // Act
            await middleware.Invoke(context);

            // Assert
            Assert.That(StatusCodes.Status401Unauthorized, Is.EqualTo(context.Response.StatusCode));
        }
    }
}
