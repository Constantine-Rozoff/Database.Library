using ContextAndModels.Models;

namespace LibraryAPI.Services.Interfaces;

public interface IBorrowedBooksByUserService
{
    Task<ICollection<BorrowedBook>> GetBorrowedBooksByUserAsync(string token);
}