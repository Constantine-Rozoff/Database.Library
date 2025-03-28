using ContextAndModels.Models;
using Database.Library.Entity;
using LibraryAPI.Models;
using LibraryAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Services;

 internal class BookService : IBooksService
 {
     private readonly LibraryContext _libraryContext;
    
     public BookService(LibraryContext libraryContext)
     {
         _libraryContext = libraryContext; 
     }
     public async Task<ICollection<Book>> GetBooks()
     {
         return await _libraryContext.Books
             .Where(book => _libraryContext.BorrowedBooks.All(borrowed => borrowed.BookId != book.Id))
             .Include(b => b.BookAuthors)
             .Include(b => b.PublishingCodeType)
             .ToListAsync();
     }
    
     async Task<ICollection<ReaderDto>> IBooksService.BorrowingHistory()
     {
         var readers = await _libraryContext.BorrowedBooks
             .Include(bb => bb.Reader) 
             .Include(bb => bb.Book)
             .ThenInclude(b => b.BookAuthors) 
             .Select(bb => new ReaderDto
             {
                 Login = bb.Reader.Login,
                 BorrowedBooks = new List<BorrowedBookDto>
                 {
                     new BorrowedBookDto
                     {
                         Name = bb.Book.Title, 
                         Authors = bb.Book.BookAuthors!.Select(a => new AuthorDto
                         {
                             FirstName = a.FirstName,  
                             LastName = a.LastName,  
                             MiddleName = a.MiddleName 
                         }).ToList(),
                         DateBorrowed = bb.DateBorrowed, 
                         DateForBorrowed = bb.DateForBorrowed,  
                         DateReturned = bb.DateReturned 
                     }
                 }
             }).ToListAsync();
         return readers;
     }
}