namespace LibraryAPI.Models;

public class ReaderDto
{
    public string Login { get; set; }
    public List<BorrowedBookDto> BorrowedBooks { get; set; }
}