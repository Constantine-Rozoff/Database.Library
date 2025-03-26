using System.IdentityModel.Tokens.Jwt;
using ContextAndModels.Models;
using Database.Library.Entity;
using LibraryAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Services;

public class BorrowedBooksByUserService : IBorrowedBooksByUserService
{
    private readonly LibraryContext _libraryContext;

    public BorrowedBooksByUserService(LibraryContext libraryContext)
    {
        _libraryContext = libraryContext;
    }

    public async Task<ICollection<BorrowedBook>> GetBorrowedBooksByUserAsync(string token)
    {
        var log = new JwtSecurityTokenHandler()
            .ReadJwtToken(token)?
            .Claims
            .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.NameId)?
            .Value;
        
        if (string.IsNullOrEmpty(log)) 
            return default;

        var readerId = await _libraryContext.Employees
            .Where(u => u.Login == log)
            .Select(u => u.Id)
            .FirstOrDefaultAsync();

        if (readerId == 0) return default;

        var borrowedBooks = await _libraryContext.BorrowedBooks
            .Where(b => b.ReaderId == readerId)
            .Include(b => b.Book)
            .Include(r => r.Reader)
            .Include(a => a.Book.BookAuthors)
            .Where(v => true)
            .ToListAsync();

        return borrowedBooks;
    }
}