namespace Database.Library.Entity;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int PublishingCodeTypeId { get; set; }
    public string PublishingCode { get; set; }
    public int Year { get; set; }
    public string PublishingCountry { get; set; }
    public string PublishingCity { get; set; }

    public PublishingCodeType PublishingCodeType { get; set; }
    public ICollection<BookAuthor> BookAuthors { get; set; }
}
