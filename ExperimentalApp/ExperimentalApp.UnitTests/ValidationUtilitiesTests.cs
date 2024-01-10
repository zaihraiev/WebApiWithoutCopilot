using ExperimentalApp.Core.Constants;
using ExperimentalApp.Core.Models.Identity;
using ExperimentalApp.Core.Validators;
using ExperimentalApp.UnitTests.Fakes;
using Moq;

namespace ExperimentalApp.UnitTests
{
    [TestFixture]
    public class ValidationUtilitiesTests
    {
        private Mock<FakeUserManager> _userManagerMock;

        [SetUp]
        public void SetUp()
        {
            _userManagerMock = new Mock<FakeUserManager>();
        }

        [Test]
        public async Task BeValidStaffOrAdminIdAsync_UserFoundWithValidRoles_ReturnsTrue()
        {
            // Arrange
            var staffId = "2";

            var user = new ApplicationUser();
            _userManagerMock.Setup(m => m.FindByIdAsync(staffId)).ReturnsAsync(user);

            var userRoles = new List<string> { RoleNames.Admin, RoleNames.Staff };
            _userManagerMock.Setup(m => m.GetRolesAsync(user)).ReturnsAsync(userRoles);

            // Act
            var result = await ValidationUtilities.BeValidStaffOrAdminIdAsync(_userManagerMock.Object, staffId);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task BeValidStaffOrAdminIdAsync_UserFoundWithInValidRoles_ReturnsFalse()
        {
            // Arrange
            var staffId = "2";

            var user = new ApplicationUser();
            _userManagerMock.Setup(m => m.FindByIdAsync(staffId)).ReturnsAsync(user);

            var userRoles = new List<string> { RoleNames.Customer };
            _userManagerMock.Setup(m => m.GetRolesAsync(user)).ReturnsAsync(userRoles);

            // Act
            var result = await ValidationUtilities.BeValidStaffOrAdminIdAsync(_userManagerMock.Object, staffId);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task BeValidStaffOrAdminIdAsync_WhenUserNotFound_ReturnsFalse()
        {
            // Arrange
            var staffId = "2";

            _userManagerMock.Setup(m => m.FindByIdAsync(staffId)).ReturnsAsync((ApplicationUser)null);

            // Act
            var result = await ValidationUtilities.BeValidStaffOrAdminIdAsync(_userManagerMock.Object, staffId);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task BeValidStaffOrAdminIdAsync_WhenStaffIdIsEmpty_ReturnsTrue()
        {
            // Arrange
            var staffId = string.Empty;

            // Act
            var result = await ValidationUtilities.BeValidStaffOrAdminIdAsync(_userManagerMock.Object, staffId);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
