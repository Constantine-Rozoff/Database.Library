using Database.Library.Entity;
using Microsoft.Data.SqlClient;
using Database.Library.Entity.Services;

internal class Program
{
    static string connectionString = "Server=localhost;Database=Library;Trusted_Connection=True;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
    
    public static void Main(string[] args)
    {
        using var context = new LibraryContext();
        
        context.Database.EnsureCreated();
        
        LibrarianService libService = new LibrarianService();
        
        ReaderService readService = new ReaderService();
        
        //libService.AddLibrarian("lib1", "123", "lib@gmail.com", 8);
        
        //AddReader("Login", "Password5",	"Email@gmail.com", "FirstName", "LastName",	1, "1234567778");
        
        //GetReaders();
        
        // readService.GetBooks();
        
        // readService.GetAuthors();
        
        //TODO: Figure how to put a book here
        readService.BorrowBook(5, 2);
        
        //libService.AddBook("The Great Gatsby 2",	1,	"NY002",	1925,	"USA",	"New York");

        context.SaveChanges();

        //libService.GetLibrarians();
        
        context.Dispose();
    }
}
