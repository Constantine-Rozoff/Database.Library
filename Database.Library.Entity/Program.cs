// See https://aka.ms/new-console-template for more information

using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;

public class Librarian
{
    public string Login { get; set; }
    
    public string Password { get; set; }
    
    public string Email { get; set; }
}

public class Reader
{
    public string Login { get; set; }
    
    public string Password { get; set; }
    
    public string Email { get; set; }
    
    public string Name { get; set; }
    
    public string LastName { get; set; }
    
    public virtual ICollection<DocumentType> Documents { get; set; }
    
    public string DocumentNumber { get; set; }
}

public class Book
{
    public string Name { get; set; }

    public virtual ICollection<Author> Authors { get; set; }
    
    public int PublishCode { get; set; }
    public ICollection<PublishCodeType> PublishCodeTypes { get; set; }

    public int Year { get; set; }

    public string PublishCountry { get; set; }

    public string PublishCity { get; set; }
}

public class Author
{
    public string Name { get; set; }
    
    public string LastName { get; set; }
    
    public string SecondName { get; set; }
    
    public DateTime DateOfBirth { get; set; }
}

public class DocumentType
{
    public string Type { get; set; }
}

public class PublishCodeType
{
    public string Type { get; set; }
}

internal class Program
{
    private static string connectionString = "Server=localhost;Database=master;Trusted_Connection=True";
    
    public static void Main(string[] args)
    {
        var librarian = new Librarian()
        {
            Login = "lib1",
            Password = "123",
            Email = "lib@gmail.com"
        };

        using var context = new LibraryContext();
        
        context.Dispose();
    }
}

public class LibraryContext : DbContext
    {
        public DbSet<Librarian> Librarians { get; set; }

        public DbSet<Reader> Readers { get; set; }

        public DbSet<Book> Books { get; set; }
        
        public DbSet<Author> Authors { get; set; }

        public DbSet<DocumentType> DocumentTypes { get; set; }
        
        public DbSet<PublishCodeType> PublishCodeTypes { get; set; }

        public LibraryContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder

                .UseSqlServer("Server=localhost;Database=master;Trusted_Connection=True")

                .LogTo(Console.WriteLine);


            base.OnConfiguring(optionsBuilder);
        }
    }