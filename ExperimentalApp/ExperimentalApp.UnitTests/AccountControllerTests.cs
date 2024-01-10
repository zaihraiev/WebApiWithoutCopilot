using ExperimentalApp.BusinessLogic.Interfaces;
using ExperimentalApp.Controllers;
using ExperimentalApp.Core.Constants;
using ExperimentalApp.Core.Models;
using ExperimentalApp.Core.Models.Identity;
using ExperimentalApp.Core.ViewModels;
using ExperimentalApp.UnitTests.Fakes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ExperimentalApp.UnitTests
{
    [TestFixture]
    public class AccountControllerTests
    {
        private AccountController _controller;
        private Mock<FakeUserManager> _mockUserManager;
        private Mock<FakeRoleManager> _mockRoleManager;
        private Mock<ITokenService> _mockTokenService;
        private Mock<IStoreService> _mockStoreService;

        [SetUp] 
        public void SetUp() 
        {
            _mockUserManager = new Mock<FakeUserManager>();
            _mockRoleManager = new Mock<FakeRoleManager>();
            _mockTokenService = new Mock<ITokenService>();
            _mockStoreService = new Mock<IStoreService>();
            _controller = new AccountController(_mockUserManager.Object, _mockRoleManager.Object, _mockStoreService.Object, _mockTokenService.Object);
        }

        [Test]
        public void AssignRole_WhenUserIsNotFound_ReturnsNotFound()
        {
            // Arrange
            var userId = "1";
            var roleId = "2";
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser)null);

            // Act 
            var result = _controller.AssignRole(userId, roleId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
            _mockUserManager.Verify(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void AssignRole_WhenCurrentUserIsNotAdminOrStaff_ReturnsForbid()
        {
            // Arrange
            var userId = "1";
            var roleId = "2";
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser());

            _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<string> { RoleNames.Customer });

            SetIdentifierToUser(userId, RoleNames.Customer);

            // Act 
            var result = _controller.AssignRole(userId, roleId);

            // Assert
            Assert.IsInstanceOf<ForbidResult>(result.Result);
            _mockUserManager.Verify(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void AssignRole_WhenRoleIsNotFound_ReturnsBadRequest()
        {
            // Arrange
            var userId = "1";
            var roleId = "2";
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser());

            _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<string> { RoleNames.Staff });

            _mockRoleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult((IdentityRole)null));

            SetIdentifierToUser(userId, RoleNames.Staff);

            // Act 
            var result = _controller.AssignRole(userId, roleId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
            _mockUserManager.Verify(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void AssignRole_WhenCurrentUserIsStaffAndRoleToAssignIsAdmin_ReturnsForbid()
        {
            // Arrange
            var userId = "1";
            var roleId = "2";
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser());

            _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<string> { RoleNames.Staff });

            _mockRoleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<IdentityRole>(new IdentityRole(RoleNames.Admin)));

            SetIdentifierToUser(userId, RoleNames.Staff);

            // Act 
            var result = _controller.AssignRole(userId, roleId);

            // Assert
            Assert.IsInstanceOf<ForbidResult>(result.Result);
            _mockUserManager.Verify(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void AssignRole_WhenCurrentUserIsAdminAndAllConditionsIsRight_ReturnsOkResult()
        {
            // Arrange
            var userId = "1";
            var roleId = "2";
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser());

            _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<string> { RoleNames.Admin });

            _mockRoleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<IdentityRole>(new IdentityRole(RoleNames.Admin)));

            SetIdentifierToUser(userId, RoleNames.Staff);

            // Act 
            var result = _controller.AssignRole(userId, roleId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            _mockUserManager.Verify(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Register_WhenModelStateIsInvalid_ReturnBadRequest()
        {
            // Arrange
            var registerViewModel = new RegisterViewModel
            {
                FirstName = "test"
            };

            // Act
            var result = _controller.Register(registerViewModel);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
            _mockUserManager.Verify(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void Register_WhenCreateUserIsFailed_AddsErrorsToModelStateAndReturnsBadRequest()
        {
            // Arange
            var registerViewModel = new RegisterViewModel
            {
                FirstName = "test",
                LastName = "test",
                Email = "test@test.com",
                UserName = "test123",
                Password = "Test!123",
                ConfirmPassword = "Test!123"
            };

            _mockStoreService.Setup(x => x.GetStoreByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Store)null);

            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error" }));

            // Act
            var result = _controller.Register(registerViewModel);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
            Assert.IsTrue(_controller.ModelState.ErrorCount > 0);
        }

        [Test]
        public void Register_WhenCreateUserIsSuccessAndRoleNotFound_AddsErrorToModelStateAndSetsDefaultCustomerRole()
        {
            // Arange
            var registerViewModel = new RegisterViewModel
            {
                FirstName = "test",
                LastName = "test",
                Email = "test@test.com",
                UserName = "test123",
                Password = "Test!123", 
                ConfirmPassword = "Test!123",
                SelectedRole = "Fake"
            };

            _mockStoreService.Setup(x => x.GetStoreByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Store)null);

            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _mockRoleManager.Setup(x => x.FindByNameAsync(registerViewModel.SelectedRole)).ReturnsAsync((IdentityRole)null);

            _mockRoleManager.Setup(x => x.FindByNameAsync(RoleNames.Customer)).ReturnsAsync(new IdentityRole(RoleNames.Customer));

            _mockUserManager.Setup(x => x.FindByNameAsync(registerViewModel.UserName)).ReturnsAsync(new ApplicationUser());
            
            _mockUserManager.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser());

            _mockUserManager.Setup(m => m.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new[] { RoleNames.Admin });

            _mockUserManager.Setup(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _mockRoleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new IdentityRole(RoleNames.Customer));

            SetIdentifierToUser("2", RoleNames.Admin);

            // Act
            var result = _controller.Register(registerViewModel);

            // Assert
            Assert.IsTrue(_controller.ModelState.ErrorCount > 0);
            _mockRoleManager.Verify(x => x.FindByNameAsync(It.IsAny<string>()), Times.Exactly(2));
            _mockRoleManager.Verify(x => x.FindByNameAsync(RoleNames.Customer), Times.Once);
            _mockUserManager.Verify(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), RoleNames.Customer), Times.Once);
        }

        [Test]
        public void Register_WhenAssigneRoleSuccess_UserCreatesAndRoleAssignedAndReturnsOkObjectResult()
        {
            // Arange
            var registerViewModel = new RegisterViewModel
            {
                FirstName = "test",
                LastName = "test",
                Email = "test@test.com",
                UserName = "test123",
                Password = "Test!123",
                ConfirmPassword = "Test!123",
                SelectedRole = RoleNames.Admin,
            };

            _mockStoreService.Setup(x => x.GetStoreByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Store)null);

            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _mockRoleManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new IdentityRole(registerViewModel.SelectedRole));

            _mockUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser() { Id = "1" });

            _mockUserManager.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser());

            _mockUserManager.Setup(m => m.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new[] { RoleNames.Admin });

            _mockUserManager.Setup(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _mockRoleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new IdentityRole(RoleNames.Admin));

            SetIdentifierToUser("2", RoleNames.Admin);

            // Act
            var result = _controller.Register(registerViewModel);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            _mockUserManager.Verify(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), RoleNames.Admin), Times.Once);
        }

        [Test] 
        public void Login_WhenUserIsNotFind_ReturnsBadRequest()
        {
            // Arange
            var loginViewModel = new LoginViewModel()
            {
                Email = "user@toto.com",
                Password = "Test!123",
            };

            _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser)null);

            // Act
            var result = _controller.Login(loginViewModel);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result.Result);
        }

        [Test]
        public void Login_WhenPasswordIsInCorrect_ReturnsBadRequest()
        {
            // Arange
            var loginViewModel = new LoginViewModel()
            {
                Email = "user@toto.com",
                Password = "Test!123",
            };

            _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser());

            _mockUserManager.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            // Act
            var result = _controller.Login(loginViewModel);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result.Result);
        }

        [Test]
        public void Login_WhenAllRight_GeneratesJwtTokenAndReturnsOkObjectResult()
        {
            // Arange
            var loginViewModel = new LoginViewModel()
            {
                Email = "user@toto.com",
                Password = "Test!123",
            };

            _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser());

            _mockUserManager.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<string>());

            _mockTokenService.Setup(x => x.CreateJWTToken(It.IsAny<ApplicationUser>(), new List<string>())).
                Returns("test");

            // Act
            var result = _controller.Login(loginViewModel);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public void Logout_WhenCall_SetsTokenAsInvalidAndReturnsOkResult()
        {
            // Arange 
            _mockTokenService.Setup(service => service.SetTokenAsInvalidAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            SetFakeHttpContext();

            // Act
            var result = _controller.LogOut();

            // Assert
            Assert.IsInstanceOf<OkResult>(result.Result);
            _mockTokenService.Verify(x => x.SetTokenAsInvalidAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void RejectRoles_WhenUserIsNotFound_ReturnsNotFound()
        {
            // Arrange
            var userId = "1";
            var roleName = "Customer";
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser)null);

            // Act 
            var result = _controller.RejectRoles(userId, roleName);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
            _mockUserManager.Verify(x => x.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void RejectRoles_WhenCurrentUserIsNotAdminOrStaff_ReturnsForbid()
        {
            // Arrange
            var userId = "1";
            var roleName = "Customer";
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser());

            _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<string> { RoleNames.Customer });

            SetIdentifierToUser(userId, RoleNames.Customer);

            // Act 
            var result = _controller.RejectRoles(userId, roleName);

            // Assert
            Assert.IsInstanceOf<ForbidResult>(result.Result);
            _mockUserManager.Verify(x => x.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void RejectRoles_WhenRoleIsNotFound_ReturnsBadRequest()
        {
            // Arrange
            var userId = "1";
            var roleName = "Test";
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser());

            _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<string> { RoleNames.Staff });

            _mockRoleManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult((IdentityRole)null));

            SetIdentifierToUser(userId, RoleNames.Staff);

            // Act 
            var result = _controller.RejectRoles(userId, roleName);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
            _mockUserManager.Verify(x => x.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
        }

        [TestCase(RoleNames.Admin, RoleNames.Staff)]
        [TestCase(RoleNames.Staff, RoleNames.Staff)]
        public async Task RejectRoles_WhenCurrentUserIsStaffAndRoleToRejectIsForbidden_ReturnsForbid(string roleToReject, string currentUserRole)
        {
            // Arrange
            var userId = "1";
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser());

            _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<string> { currentUserRole });

            _mockRoleManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<IdentityRole>(new IdentityRole(roleToReject)));

            SetIdentifierToUser(userId, currentUserRole);

            // Act 
            var result = await _controller.RejectRoles(userId, roleToReject);

            // Assert
            Assert.IsInstanceOf<ForbidResult>(result);
            _mockUserManager.Verify(x => x.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void RejectRoles_WhenFailedToRemoveRole_ReturnsBadRequestResult()
        {
            // Arrange
            var userId = "1";
            var roleName = "Staff";
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser());

            _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<string> { RoleNames.Admin });

            _mockRoleManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<IdentityRole>(new IdentityRole(RoleNames.Staff)));

            _mockUserManager.Setup(x => x.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error"}));

            SetIdentifierToUser(userId, RoleNames.Admin);

            // Act 
            var result = _controller.RejectRoles(userId, roleName);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
            _mockUserManager.Verify(x => x.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void RejectRoles_WhenCurrentUserIsAdminAndRejectRoleIsStuff_UpdatesStoreIdRejectRoleAndReturnsOkResult()
        {
            // Arrange
            var userId = "1";
            var roleName = "Staff";
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser());

            _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<string> { RoleNames.Admin });

            _mockRoleManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<IdentityRole>(new IdentityRole(RoleNames.Staff)));

            _mockUserManager.Setup(x => x.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);

            SetIdentifierToUser(userId, RoleNames.Admin);

            // Act 
            var result = _controller.RejectRoles(userId, roleName);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            _mockUserManager.Verify(x => x.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
            _mockUserManager.Verify(x => x.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Once);
        }

        [Test]
        public void RejectRoles_WhenCurrentUserIsAdminAndRejectRoleIsCustomer_RejectRoleAndReturnsOkResult()
        {
            // Arrange
            var userId = "1";
            var roleName = "Customer";
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser());

            _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<string> { RoleNames.Admin });

            _mockRoleManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<IdentityRole>(new IdentityRole(RoleNames.Customer)));

            _mockUserManager.Setup(x => x.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            SetIdentifierToUser(userId, RoleNames.Admin);

            // Act 
            var result = _controller.RejectRoles(userId, roleName);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            _mockUserManager.Verify(x => x.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
            _mockUserManager.Verify(x => x.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
        }

        [Test]
        public void ChangeUserRole_WhenUserIsNotFound_ReturnsNotFound()
        {
            // Arrange
            var userId = "1";
            var roleId = "2";
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser)null);

            // Act 
            var result = _controller.ChangeUserRole(userId, roleId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
            _mockUserManager.Verify(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
            _mockUserManager.Verify(x => x.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
            _mockUserManager.Verify(x => x.RemoveFromRolesAsync(It.IsAny<ApplicationUser>(), It.IsAny<List<string>>()), Times.Never);
        }

        [Test]
        public void ChangeUserRole_WhenCurrentUserIsNotAdmin_ReturnsForbid()
        {
            // Arrange
            var userId = "1";
            var roleId = "2";
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser());

            _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<string> { RoleNames.Customer });

            SetIdentifierToUser(userId, RoleNames.Customer);

            // Act 
            var result = _controller.ChangeUserRole(userId, roleId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
            _mockUserManager.Verify(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
            _mockUserManager.Verify(x => x.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
            _mockUserManager.Verify(x => x.RemoveFromRolesAsync(It.IsAny<ApplicationUser>(), It.IsAny<List<string>>()), Times.Never);
        }

        [Test]
        public void ChangeUserRole_WhenRoleIsNotFound_ReturnsBadRequest()
        {
            // Arrange
            var userId = "1";
            var roleId = "2";
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser());

            _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<string> { RoleNames.Admin });

            _mockRoleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult((IdentityRole)null));

            SetIdentifierToUser(userId, RoleNames.Admin);

            // Act 
            var result = _controller.ChangeUserRole(userId, roleId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
            _mockUserManager.Verify(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
            _mockUserManager.Verify(x => x.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
            _mockUserManager.Verify(x => x.RemoveFromRolesAsync(It.IsAny<ApplicationUser>(), It.IsAny<List<string>>()), Times.Never);
        }

        [Test]
        public void ChangeUserRole_WhenCurrentUserIsAdminAndAllConditionsIsRight_ReturnsOkResult()
        {
            // Arrange
            var userId = "1";
            var roleId = "2";
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser());

            _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<string> { RoleNames.Admin });

            _mockRoleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<IdentityRole>(new IdentityRole(RoleNames.Admin)));

            _mockUserManager.Setup(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            SetIdentifierToUser(userId, RoleNames.Staff);

            // Act 
            var result = _controller.ChangeUserRole(userId, roleId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            _mockUserManager.Verify(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
            _mockUserManager.Verify(x => x.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
            _mockUserManager.Verify(x => x.RemoveFromRolesAsync(It.IsAny<ApplicationUser>(), It.IsAny<List<string>>()), Times.Once);
        }

        [Test]
        public void ChangeUserRole_WhenCurrentUserIsAdminAndRolesToDeleteContainsStaff_ReturnsOkResult()
        {
            // Arrange
            var userId = "1";
            var roleId = "2";
            var user = new ApplicationUser
            {
                Id = userId
            };

            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<string> { RoleNames.Admin });

            _mockRoleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<IdentityRole>(new IdentityRole(RoleNames.Admin)));

            _mockUserManager.Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { RoleNames.Staff });

            _mockUserManager.Setup(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            SetIdentifierToUser(userId, RoleNames.Staff);

            // Act 
            var result = _controller.ChangeUserRole(userId, roleId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            _mockUserManager.Verify(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
            _mockUserManager.Verify(x => x.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Once);
            _mockUserManager.Verify(x => x.RemoveFromRolesAsync(It.IsAny<ApplicationUser>(), It.IsAny<List<string>>()), Times.Once);
        }

        [Test]
        public void ChangeUserRole_WhenAddToRoleFailed_ErrorsAddsToModelStateAndReturnsBadRequestResult()
        {
            // Arrange
            var userId = "1";
            var roleId = "2";
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser());

            _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<string> { RoleNames.Admin });

            _mockRoleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<IdentityRole>(new IdentityRole(RoleNames.Admin)));

            _mockUserManager.Setup(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error"}));

            SetIdentifierToUser(userId, RoleNames.Staff);

            // Act 
            var result = _controller.ChangeUserRole(userId, roleId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
            Assert.IsTrue(_controller.ModelState.ErrorCount > 0);
            _mockUserManager.Verify(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
            _mockUserManager.Verify(x => x.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
            _mockUserManager.Verify(x => x.RemoveFromRolesAsync(It.IsAny<ApplicationUser>(), It.IsAny<List<string>>()), Times.Once);
        }

        private void SetIdentifierToUser(string id, string role)
        {
            var fakeUser = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim(ClaimTypes.Role, role)
            }));

            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = fakeUser
                }
            };

            _controller.ControllerContext = context;
        }

        private void SetFakeHttpContext()
        {
            var authServiceMock = new Mock<IAuthenticationService>();
            authServiceMock
                .Setup(_ => _.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.FromResult((object)null));

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(_ => _.GetService(typeof(IAuthenticationService)))
                .Returns(authServiceMock.Object);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    RequestServices = serviceProviderMock.Object
                }
            };
        }

        private void SimulateValidation(object model)
        {
            var validationContext = new ValidationContext(model, null, null);
            var validationResults = new List<ValidationResult>();

            Validator.TryValidateObject(model, validationContext, validationResults, true);
            foreach (var validationResult in validationResults)
            {
                _controller.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }
        }
    }
}
