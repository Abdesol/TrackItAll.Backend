using Azure;
using Azure.Data.Tables;

namespace TrackItAll.Domain.Entities;

/// <summary>
/// Represents a category for an expense.
/// </summary>
public class Category : ITableEntity
{
    public DateTimeOffset? Timestamp { get; set; }
    
    public ETag ETag { get; set; }

    public string PartitionKey { get; set; }

    private string _rowKey;
    public string RowKey
    {
        get => _rowKey;
        set
        {
            _rowKey = value;
            Id = int.Parse(value);
        }
    }

    /// <summary>
    /// Gets or sets the name of the category.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Id that we get from the row key set method
    /// </summary>
    public int Id { get; set; }
}