namespace ContextAndModels.Models;

public class DocumentType
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<Reader> Readers { get; set; }
}
