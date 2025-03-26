using ContextAndModels;
using Database.Library.Entity;
using Microsoft.Data.SqlClient;
using Database.Library.Entity.Services;

internal class Program
{
    public static void Main(string[] args)
    {
        using var context = new LibraryContext();
        
        context.Database.EnsureCreated();
        
        LibrarianService libService = new LibrarianService(context);
        
        ReaderService readService = new ReaderService(context);
        
        //libService.AddLibrarian("lib1", "123", "lib@gmail.com", 8);
        
        //AddReader("Login", "Password5",	"Email@gmail.com", "FirstName", "LastName",	1, "1234567778");
        
        readService.GetAuthors();
        
        //readService.GetBooks();
        
        // readService.GetAuthors();
        
        // readService.GetBooksAuthors(context.BookAuthors);
        //
        // Console.WriteLine("-----");
        //
        // readService.GetBooks(context.Books);
        //
        // Console.WriteLine("-----");
        //
        // readService.GetAuthors(context.Authors);
        //
        // Console.WriteLine("Add book");
        //
        // libService.AddBook("The flowers for Elgernone", 1,	"NY005", 	1988,	"USA",	"New York");
        //
        // Console.WriteLine("Add reader");
        //
        // libService.AddReader("michael_shushu",	"password777",	"michael_shushu@example.com",	"michael",	"shushu",	1,	"5554567890");
        //
        // Console.WriteLine("Add librarian");
        //
        // libService.AddLibrarian("jackie_chahn",	"777",	"jackie_chahn.com",	12);
        
        // Book bookUpdate = new Book()
        // {
        //     Title = "Book Title",
        //     PublishingCodeTypeId = 1,
        //     PublishingCode = "Book Publishing Code",
        //     Year = 2000,
        //     PublishingCountry = "US",
        //     PublishingCity = "Simferopol"
        // };
        //
        // libService.UpdateBook(8, bookUpdate, context);

        // Author author = new Author()
        // {
        //     FirstName = "Genadiy",
        //     LastName = "Ivanov",
        //     MiddleName = "Kawtan",
        //     DateOfBirth = Convert.ToDateTime("1988-03-03")
        // };
        //
        // //libService.AddAuthor(author, context);
        //
        // libService.UpdateAuthor(3, author, context);

        // Reader reader = new Reader()
        // {
        //     Login = "admin",
        //     Password = "admin",
        //     Email = "john.doe@gmail.com",
        //     FirstName = "John",
        //     LastName = "Doe",
        //     DocumentType = new DocumentType()
        //     { 
        //         Name = "book"
        //     },
        //     DocumentNumber = "1111111111"
        // };
        //
        // libService.UpdateReader(8, reader, context);
        
        //TODO: Figure how to put a book here
        //readService.BorrowBook(5, 2);
        
        //libService.AddBook("The Great Gatsby 2",	1,	"NY002",	1925,	"USA",	"New York");

        //context.SaveChanges();

        //libService.GetLibrarians();
        
        //context.Dispose();
    }
}
