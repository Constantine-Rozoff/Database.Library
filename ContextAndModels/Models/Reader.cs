namespace Database.Library.Entity;

public class Reader
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int DocumentTypeId { get; set; }
    public string DocumentNumber { get; set; }
    
    public DocumentType DocumentType { get; set; }
}
