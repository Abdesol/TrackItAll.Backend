namespace TrackItAll.Domain.Entities;

public class Expense
{
    public string Id { get; set; }
    
    public string OwnerId { get; set; }
    
    public DateTime Date { get; set; }
    
    public double Amount { get; set; }
    
    public string Description { get; set; }
    
    public int CategoryId { get; set; }
    
    public string? ReceiptId { get; set; }
}