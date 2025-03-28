using ContextAndModels;
using ContextAndModels.Models;
using LibraryAPI.Models;

namespace LibraryAPI.Services.Interfaces;

public interface IBooksService
{
    Task<ICollection<Book>> GetBooks();
    
    Task<ICollection<ReaderDto>> BorrowingHistory();
}