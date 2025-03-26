using ContextAndModels;
using ContextAndModels.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Database.Library.Entity.Services;

public class LibrarianService : ReaderService
{
    public static string connectionString = "Server=localhost;Database=Library;Trusted_Connection=True;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";

    public LibrarianService(LibraryContext context) : base(context)
    {
        
    }

    public void AddLibrarian(Librarian librarian, DbContext context)
    {
        using (context)
        {
            context.Add(librarian);
            context.SaveChanges();
        }
    }

    public void AddReader(Reader reader, DbContext context)
    {
        using (context)
        {
            context.Add(reader);
            context.SaveChanges();
        }
    }
    public void AddBook(Book book, DbContext context)
    {
        using (context)
        {
            context.Add(book);
            context.SaveChanges();
        }
    }
    
    public void AddAuthor(Author author, DbContext context)
    {
        using (context)
        {
            context.Add(author);
            context.SaveChanges();
        }
    }
    
    public void UpdateReader(int readerId, Reader updatedReader, DbContext context)
    {
        using (context)
        {
            var reader = context.Set<Reader>()
                .Include(r => r.DocumentType) 
                .FirstOrDefault(r => r.Id == readerId);

            if (reader != null)
            {
                reader.Login = updatedReader.Login;
                reader.Password = updatedReader.Password;
                reader.Email = updatedReader.Email;
                reader.FirstName = updatedReader.FirstName;
                reader.LastName = updatedReader.LastName;
                reader.DocumentTypeId = updatedReader.DocumentTypeId;
                reader.DocumentNumber = updatedReader.DocumentNumber;

                if (updatedReader.DocumentType != null)
                {
                    reader.DocumentType = updatedReader.DocumentType;
                }

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("Reader not found");
            }
        }
    }
    
    public void UpdateBook(int bookId, Book bookForUpdate, DbContext context)
    {
        using (context)
        {
            var book = context.Set<Book>()
                .Include(b => b.BookAuthors)!  
                .ThenInclude(ba => ba.Author) 
                .FirstOrDefault(b => b.Id == bookId);

            if (book != null)
            {
                book.Title = bookForUpdate.Title;
                book.PublishingCodeTypeId = bookForUpdate.PublishingCodeTypeId;
                book.PublishingCode = bookForUpdate.PublishingCode;
                book.Year = bookForUpdate.Year;
                book.PublishingCountry = bookForUpdate.PublishingCountry;
                book.PublishingCity = bookForUpdate.PublishingCity;

                if (bookForUpdate.BookAuthors != null && bookForUpdate.BookAuthors.Any())
                {
                    book.BookAuthors.Clear();
                    foreach (var bookAuthor in bookForUpdate.BookAuthors)
                    {
                        book.BookAuthors.Add(bookAuthor);
                    }
                }

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("Book is not found");
            }
        }
    }
    
    public void UpdateAuthor(int authorId, Author authorUpdate, DbContext context)
    {
        using (context)
        {
            var author = context.Set<Author>()
                .FirstOrDefault(b => b.Id == authorId);

            if (author != null)
            {
                author.FirstName = authorUpdate.FirstName;
                author.LastName = authorUpdate.LastName;
                author.MiddleName = authorUpdate.MiddleName;
                author.DateOfBirth = authorUpdate.DateOfBirth;

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("Book is not found");
            }
        }
    }
    
    public void AddLibrarian(string login, string password, string email, int readerId)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = $"INSERT INTO Librarians (Login, Password, Email, ReaderId) VALUES (@login, @password, @email, @readerId)";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@login", login);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@readerId", readerId);

                int rowsAffected = cmd.ExecuteNonQuery();
                Console.WriteLine($"Rows Inserted: {rowsAffected}");
            }
        }
    }
    
    public void AddReader(string login, string password, string email, string firstName, string lastName, int documentTypeId, string documentNumber)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = "INSERT INTO [Library].[dbo].[Readers] (Login, Password, Email, FirstName, LastName, DocumentTypeId, DocumentNumber) " +
                           "VALUES (@login, @password, @email, @firstName, @lastName, @documentTypeId, @documentNumber)";
            
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@login", login);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@firstName", firstName);
                cmd.Parameters.AddWithValue("@lastName", lastName);
                cmd.Parameters.AddWithValue("@documentTypeId", documentTypeId);
                cmd.Parameters.AddWithValue("@documentNumber", documentNumber);

                int rowsAffected = cmd.ExecuteNonQuery();
                Console.WriteLine($"Rows Inserted: {rowsAffected}");
            }
        }
    }
    
    public void AddBook(string title, int publishingCodeTypeId, string publishingCode, int year, string publishingCountry, string publishingCity)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = $"INSERT INTO Books (title, publishingCodeTypeId, publishingCode, year, publishingCountry, publishingCity) VALUES (@title, @publishingCodeTypeId, @publishingCode, @year, @publishingCountry, @publishingCity)";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@publishingCodeTypeId", publishingCodeTypeId);
                cmd.Parameters.AddWithValue("@publishingCode", publishingCode);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@publishingCountry", publishingCountry);
                cmd.Parameters.AddWithValue("@publishingCity", publishingCity);

                int rowsAffected = cmd.ExecuteNonQuery();
                Console.WriteLine($"Rows Inserted: {rowsAffected}");
            }
        }
    }
    
    public void GetReaders()
    {
        List<Reader> readers = new List<Reader>();
        
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = $"SELECT Login, Password, Email, FirstName, LastName, DocumentTypeId, DocumentNumber FROM [Library].[dbo].[Readers]";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Login: {reader["Login"]}, Password: {reader["Password"]}, Email: {reader["Email"]}");
    
                        readers.Add(new Reader()
                        {
                            Login = reader.GetString(0),
                            Password = reader.GetString(1),
                            Email = reader.GetString(2),
                            FirstName = reader.GetString(3),
                            LastName = reader.GetString(4),
                            DocumentTypeId = reader.GetInt32(5),
                            DocumentNumber = reader.GetString(6)
                        });
                    }
                }
            }
        }
    }
    
    public void GetLibrarians()
    {
        List<Librarian> librarians = new List<Librarian>();
    
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = "SELECT Login, Password, Email FROM Librarians ";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Login: {reader["Login"]}, Password: {reader["Password"]}, Email: {reader["Email"]}");
    
                        librarians.Add(new Librarian
                        {
                            Login = reader.GetString(0),
                            Password = reader.GetString(1),
                            Email = reader.GetString(2)
                        });
                    }
                }
            }
        }
    }
}