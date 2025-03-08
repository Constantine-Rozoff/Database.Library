using Microsoft.Data.SqlClient;

namespace Database.Library.Entity.Services;

public class ReaderService
{
    static string connectionString = "Server=localhost;Database=Library;Trusted_Connection=True;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
    
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
    
    public void GetBooks()
    {
        List<Book> books = new List<Book>();
    
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();

            string query = @"
                SELECT 
                    b.Title AS BookTitle,
                    a.FirstName,
                    a.LastName,
                    a.MiddleName
                FROM 
                    [Library].[dbo].[Books] b
                JOIN 
                    [Library].[dbo].[Authors] a ON a.Id = b.Id";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string bookTitle = reader.GetString(reader.GetOrdinal("BookTitle"));
                        string firstName = reader.GetString(reader.GetOrdinal("FirstName"));
                        string lastName = reader.GetString(reader.GetOrdinal("LastName"));
                        string middleName = reader.IsDBNull(reader.GetOrdinal("MiddleName")) ? null : reader.GetString(reader.GetOrdinal("MiddleName"));

                        Console.WriteLine($"Title: {bookTitle}");
                        Console.WriteLine($"Name: {firstName} {middleName} {lastName}");
                        Console.WriteLine(new string('-', 50));
                    }
                }
            }
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
