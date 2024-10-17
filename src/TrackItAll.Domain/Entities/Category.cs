namespace TrackItAll.Domain.Entities;

/// <summary>
/// Represents a category for an expense.
/// </summary>
public class Category
{
    /// <summary>
    /// Gets or sets the unique identifier for the category.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the category.
    /// </summary>
    public string Name { get; set; }
}