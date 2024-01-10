using ExperimentalApp.Core.Constants;
using ExperimentalApp.Core.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExperimentalApp.Core.Validators
{
    /// <summary>
    /// Validator additional helpers methods
    /// </summary>
    public static class ValidationUtilities
    {
        /// <summary>
        /// Method that validates staff id. 
        /// </summary>
        /// <param name="staffId">Staff id to validate</param>
        /// <returns>Booleand results of operation</returns>
        public static async Task<bool> BeValidStaffOrAdminIdAsync(UserManager<ApplicationUser> userManager, string staffId)
        {
            if (string.IsNullOrEmpty(staffId))
            {
                return true;
            }

            var user = await userManager.FindByIdAsync(staffId);
            if (user == null)
            {
                return false;
            }

            var userRoles = await userManager.GetRolesAsync(user);

            if (userRoles.Contains(RoleNames.Admin) || userRoles.Contains(RoleNames.Staff))
            {
                return true;
            }

            return false;
        }
    }
}
