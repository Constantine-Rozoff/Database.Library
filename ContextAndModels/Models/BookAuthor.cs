namespace ContextAndModels.Models;

public class BookAuthor
{
    public int BookId { get; set; }
    public Book Book { get; set; }
    public string Title { get; set; }
    public int AuthorId { get; set; }
    public Author Author { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    
    public DateTime DateOfBirth { get; set; }
}