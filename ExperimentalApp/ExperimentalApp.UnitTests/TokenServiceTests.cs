using ExperimentalApp.BusinessLogic.Interfaces;
using ExperimentalApp.BusinessLogic.Services;
using ExperimentalApp.Core.Constants;
using ExperimentalApp.Core.Models;
using ExperimentalApp.Core.Models.Identity;
using ExperimentalApp.DataAccessLayer.DBContext;
using ExperimentalApp.UnitTests.Fakes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MockQueryable.Moq;
using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ExperimentalApp.UnitTests
{
    [TestFixture]
    public class TokenServiceTests
    {
        private IConfiguration _configuration;
        private ITokenService _tokenService;
        private Mock<DvdRentalContext> _context;

        [SetUp] 
        public void SetUp() 
        {
            _context = GetFakeDbContext();
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "Jwt:Key", "secretKey!12345secretKey!12345secretKey!12345secretKey!12345" },
                    { "Jwt:Issuer", "https://localhost:7251" },
                    { "Jwt:Audience", "https://localhost:7251" }
                }).Build();

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(provider => provider.GetService(typeof(DvdRentalContext)))
                .Returns(_context.Object);

            var scopeMock = new Mock<IServiceScope>();
            scopeMock.Setup(scope => scope.ServiceProvider).Returns(serviceProviderMock.Object);

            var scopeFactoryMock = new Mock<IServiceScopeFactory>();
            scopeFactoryMock.Setup(factory => factory.CreateScope()).Returns(scopeMock.Object);

            _tokenService = new TokenService(scopeFactoryMock.Object, _configuration);
        }

        [Test]
        public void CreateJwtToken_GeneratesValidToken_ReturnsToken()
        {
            // Arrange
            var user = new ApplicationUser { Email = "test@test.com" };
            var roles = new List<string> { RoleNames.Admin };
            var expectedMinutes = 15;

            // Act
            var token = _tokenService.CreateJWTToken(user, roles);
            var tokenHandler = new JwtSecurityTokenHandler();
            var decodedToken = tokenHandler.ReadJwtToken(token);

            var emailClaim = decodedToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var roleClaims = decodedToken.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            var expirationClaim = decodedToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;

            // Assert
            Assert.NotNull(token);
            Assert.That(_configuration["Jwt:Issuer"], Is.EqualTo(decodedToken.Issuer));
            Assert.That(_configuration["Jwt:Audience"], Is.EqualTo(decodedToken.Audiences.FirstOrDefault()));
            Assert.That(user.Email, Is.EqualTo(emailClaim));
            Assert.IsTrue(roles.All(r => roleClaims.Contains(r)));
            Assert.That(DateTimeOffset.FromUnixTimeSeconds(long.Parse(expirationClaim)).LocalDateTime, Is.EqualTo(DateTime.Now.AddMinutes(expectedMinutes)).Within(2).Seconds);
        }

        [Test]
        public void IsTokenInBlackList_WhenTokenInBlackList_ReturnsTrue()
        {
            // Act
            var result = _tokenService.IsTokenInBlackListAsync("test!123").Result;

            // Assert
            Assert.True(result);
        }

        [Test]
        public async Task IsTokenInBlackList_WhenTokenNotInBlackList_ReturnsFalse()
        {
            // Act
            var result = await _tokenService.IsTokenInBlackListAsync("test!122");

            // Assert
            Assert.False(result);
        }

        [Test]
        public void SetTokenAsInvalid_WhenCalls_SetsTokenToBlackList()
        {
            // Arrange
            var tokenString = "token!122";
            
            // Act
            var result = _tokenService.SetTokenAsInvalidAsync(tokenString);

            // Assert
            _context.Verify(db => db.BlacklistedTokens.Add(It.IsAny<BlackListedToken>()), Times.Once);
            _context.Verify(db => db.SaveChangesAsync(default), Times.Once);
        }

        private Mock<DvdRentalContext> GetFakeDbContext()
        {
            var blackList = new List<BlackListedToken>
            {
                new BlackListedToken
                {
                    Id = 1,
                    Token = "test!123"
                },
                new BlackListedToken
                {
                    Id = 2,
                    Token = "test!321"
                }
            }.AsQueryable();

            var dbContextMock = new Mock<DvdRentalContext>();

            var dbSetMock = new Mock<DbSet<BlackListedToken>>();

            dbSetMock.As<IAsyncEnumerable<BlackListedToken>>()
            .Setup(x => x.GetAsyncEnumerator(default))
                .Returns(new FakeAsyncEnumerator<BlackListedToken>(blackList.GetEnumerator()));
            dbSetMock.As<IQueryable<BlackListedToken>>()
            .Setup(m => m.Provider)
                .Returns(new FakeAsyncQueryProvider<BlackListedToken>(blackList.Provider));
            dbSetMock.As<IQueryable<BlackListedToken>>()
                .Setup(m => m.Expression).Returns(blackList.Expression);
            dbSetMock.As<IQueryable<BlackListedToken>>()
                .Setup(m => m.ElementType).Returns(blackList.ElementType);
            dbSetMock.As<IQueryable<BlackListedToken>>()
                .Setup(m => m.GetEnumerator()).Returns(blackList.GetEnumerator());

            dbContextMock.Setup(db => db.BlacklistedTokens).ReturnsDbSet(dbSetMock.Object);

            return dbContextMock;
        }
    }
}
