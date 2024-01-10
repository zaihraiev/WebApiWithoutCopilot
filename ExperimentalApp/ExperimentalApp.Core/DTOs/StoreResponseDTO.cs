using ExperimentalApp.Core.Models;

namespace ExperimentalApp.Core.DTOs
{
    /// <summary>
    /// Represents store data transfer object for response
    /// </summary>
    public class StoreResponseDTO
    {
        /// <summary>
        /// Gets or sets StoreId
        /// </summary>
        public int StoreId { get; set; }

        /// <summary>
        /// Gets or sets ManagerStaffId
        /// </summary>
        public string ManagerStaffId { get; set; }

        /// <summary>
        /// Gets or sets Address
        /// </summary>
        public Address Address { get; set; }

        /// <summary>
        /// Gets or sets LastUpdate date time
        /// </summary>
        public DateTime LastUpdate { get; set; }
    }
}
