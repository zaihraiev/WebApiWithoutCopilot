namespace ExperimentalApp.Core.Models;

/// <summary>
/// Represents a class for working with Payment
/// </summary>
public partial class Payment
{
    /// <summary>
    /// Gets or sets PaymentId
    /// </summary>
    public int PaymentId { get; set; }

    /// <summary>
    /// Gets or sets CustomerId
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Gets or sets StaffId
    /// </summary>
    public int StaffId { get; set; }

    /// <summary>
    /// Gets or sets RentalId
    /// </summary>
    public int RentalId { get; set; }

    /// <summary>
    /// Gets or sets Amount
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets PaymentDate date time
    /// </summary>
    public DateTime PaymentDate { get; set; }

    /// <summary>
    /// Gets or sets Customer
    /// </summary>
    public virtual Customer Customer { get; set; } = null!;

    /// <summary>
    /// Gets or sets Rental
    /// </summary>
    public virtual Rental Rental { get; set; } = null!;

    /// <summary>
    /// Gets or sets Staff
    /// </summary>
    public virtual Staff Staff { get; set; } = null!;
}
