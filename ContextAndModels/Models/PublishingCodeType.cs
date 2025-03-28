namespace ContextAndModels.Models;

public class PublishingCodeType
{
    public int Id { get; set; }
    
    public string Name { get; set; }

    public ICollection<Book> Books { get; set; }
}