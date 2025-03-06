// See https://aka.ms/new-console-template for more information


using System.Data;
using Database.Library.Entity;
using Microsoft.Data.SqlClient;

internal class Program
{
    static string connectionString = "Server=localhost;Database=master;Trusted_Connection=True;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
    
    public static void Main(string[] args)
    {
        //using var context = new MasterContext();

        //AddLibrarian("login", "password", "email");
        //GetLibrarians();
    }
    
    static void AddLibrarian(string login, string password, string email)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = $"INSERT INTO Librarian (Login, Password, Email) VALUES (@login, @password, @email)";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@login", login);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@email", email);

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