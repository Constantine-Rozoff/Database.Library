namespace LibraryAPI.Models;

public record BorrowingHistoryUserDto(ICollection<BorrowedBookDto> BorrowedBooks,int? countOverdue);