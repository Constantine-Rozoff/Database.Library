using System.Text.Json.Serialization;

namespace ContextAndModels.Models;

public class BorrowedBook
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public  Book Book { get; set; } 
    public int ReaderId { get; set; }
    [JsonIgnore]
    public Reader Reader { get; set; }
    public DateTime DateBorrowed { get; set; }
    public DateTime DateForBorrowed { get; set; } = DateTime.Now.AddDays(30);
    public DateTime? DateReturned { get; set; }
}