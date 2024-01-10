namespace ExperimentalApp.Core.ViewModels
{
    /// <summary>
    /// Represents store view model
    /// </summary>
    public class StoreUpdateViewModel
    {
        /// <summary>
        /// Sets storeId for store
        /// </summary>
        public int StoreId { get; set; }

        /// <summary>
        /// Sets addressId for store
        /// </summary>
        public int? AddressId { get; set; }

        /// <summary>
        /// Sets staffId for store
        /// </summary>
        public string? StaffId { get; set; }
    }
}
