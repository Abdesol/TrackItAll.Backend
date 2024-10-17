using Newtonsoft.Json;

namespace TrackItAll.Domain.Entities;

/// <summary>
/// Represents an expense record with associated details such as amount, date, description, and category.
/// </summary>
public class Expense
{
    /// <summary>
    /// Gets or sets the unique identifier for the expense.
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user who owns the expense.
    /// </summary>
    public string OwnerId { get; set; }

    /// <summary>
    /// Gets or sets the date the expense was incurred.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Gets or sets the amount of the expense.
    /// </summary>
    public double Amount { get; set; }

    /// <summary>
    /// Gets or sets the description or note about the expense.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the category to which the expense belongs.
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Gets or sets the category object associated with the expense. This is ignored in JSON serialization.
    /// </summary>
    [JsonIgnore]
    public Category? Category { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the receipt associated with the expense, if any.
    /// </summary>
    public string? ReceiptId { get; set; }
}