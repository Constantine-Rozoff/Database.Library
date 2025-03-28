using ContextAndModels;
using ContextAndModels.Models;
using Microsoft.Data.SqlClient;

namespace Database.Library.Entity.Services;

public class ReaderService
{
    private LibraryContext _context { get; set; }
    
    static string connectionString = "Server=localhost;Database=Library;Trusted_Connection=True;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
    
    public ReaderService(LibraryContext context)
    {
        _context = context;
    }
    
    public void BorrowBook(int readerId, int bookId)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = $"INSERT INTO ВоокBorrowers (BookId, ReaderId, LoanDate, ReturnDate, LoanPeriod) VALUES (@bookId, @readerId, @loanDate, @returnDate, @loanPeriod)";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@bookId", bookId);
                cmd.Parameters.AddWithValue("@readerId", readerId);
                cmd.Parameters.AddWithValue("@loanDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@returnDate", DateTime.Now.AddDays(30));
                cmd.Parameters.AddWithValue("@loanPeriod", 30);

                int rowsAffected = cmd.ExecuteNonQuery();
                Console.WriteLine($"Rows Inserted: {rowsAffected}");
            }
        }
    }

    public void GetBorrowedList(Reader reader)
    {
        
    }
    
    public void GetBooksAuthors(IQueryable<BookAuthor> books)
    {
        int index = 0;
        foreach (var book in books)
        {
            string authors = string.Join(", ", $"{book.FirstName} {book.MiddleName} {book.LastName}");
            Console.WriteLine($"{++index} - {book.Title} - {authors}");
        }
    }

    public void GetBooks(IQueryable<Book> bookList)
    {
        int index = 0;
        foreach (var book in bookList)
        {
            string list = string.Join(", ", $"{book.Title} - {book.Year} - {book.PublishingCountry} - {book.PublishingCity}");
            Console.WriteLine($"{++index} - {list}");
        }
    }
    
    public void GetAuthors(IQueryable<Author> authors)
    {
        int index = 0;
        foreach (var author in authors)
        {
            string list = string.Join(", ", $"{author.FirstName} - {author.MiddleName} - {author.LastName} - {author.DateOfBirth}");
            Console.WriteLine($"{++index} - {list}");
        }
    }

    public void GetAuthors()
    {
        List<Author> authors = new List<Author>();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = "SELECT FirstName, LastName, MiddleName, DateOfBirth FROM Authors ";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(
                            $"FirstName: {reader["FirstName"]}, LastName: {reader["LastName"]}, MiddleName: {reader["MiddleName"]}, DateOfBirth: {reader["DateOfBirth"]}");

                        authors.Add(new Author
                        {
                            FirstName = reader.GetString(0),
                            LastName = reader.GetString(1),
                            MiddleName = reader.GetString(2),
                            DateOfBirth = reader.GetDateTime(3)
                        });
                    }
                }
            }
        }
    }
}
