using Database.Library.Entity;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    static string connectionString = "Server=localhost;Database=Library;Trusted_Connection=True;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
    
    public static void Main(string[] args)
    {
        var librarian = new Librarian()
        {
            Login = "lib1",
            Password = "123",
            Email = "lib@gmail.com",
            Reader = new Reader()
        };

        using var context = new LibraryContext();
        
        context.Database.EnsureCreated();
        
        AddLibrarian("lib1", "123", "lib@gmail.com", 1);
        
        context.Dispose();
    }
    
    static void AddLibrarian(string login, string password, string email, int readerId)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = $"INSERT INTO Librarian (Login, Password, Email, ReaderId) VALUES (@login, @password, @email, @readerId)";
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
    
    static void GetLibrarians()
    {
        List<Librarian> librarians = new List<Librarian>();
    
        librarians.ToList();
    
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = "SELECT Login, Password, Email FROM Librarian ";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Login: {reader["Login"]}, Password: {reader["Password"]}, Email: {reader["Email"]}");
    
                        librarians.Add(new Librarian
                        {
                            Login = reader.GetString("login"),
                            Password = reader.GetString("password"),
                            Email = reader.GetString("email"),
                        });
                    }
                }
            }
        }
    }
}
