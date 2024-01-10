using ExperimentalApp.Core.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExperimentalApp.UnitTests.Fakes
{
    public class FakeSignInManager : SignInManager<ApplicationUser>
    {
        public FakeSignInManager()
            : base(
                  new Mock<FakeUserManager>().Object,
                  new HttpContextAccessor(),
                  new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
                  new Mock<Microsoft.Extensions.Options.IOptions<IdentityOptions>>().Object,
                  new Mock<ILogger<SignInManager<ApplicationUser>>>().Object,
                  new Mock<Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider>().Object,
                  new Mock<Microsoft.AspNetCore.Identity.IUserConfirmation<ApplicationUser>>().Object
                  )
        { }
    }
}
