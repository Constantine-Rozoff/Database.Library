namespace ContextAndModels.Models;

public class Librarian
{
    public int Id { get; set; }
    
    public string Login { get; set; }
    
    public string Password { get; set; }
    
    public string Email { get; set; }
    
    public Reader? Reader { get; set; }
}