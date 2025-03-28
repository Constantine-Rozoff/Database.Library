using ContextAndModels.Models;
using Database.Library.Entity;
using LibraryAPI.Models;
using LibraryAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Controllers;

[Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Employee")]
    public class LibrarianController : ControllerBase
    {
        private IBooksService _booksService;
        private readonly IAuthorUpdate _authorUpdate;
        private LibraryContext _libraryContext;

        public LibrarianController(LibraryContext libraryContext, IBooksService booksService, IAuthorUpdate authorUpdate)
        {
            _libraryContext = libraryContext;
            _booksService = booksService;
            _authorUpdate = authorUpdate;

        }
        [HttpGet("debug-auth")]
        public IActionResult DebugAuth()
        {
            return Ok(new
            {
                User = Request.Headers["Authorization"].ToString().Replace("Bearer ", ""),
                IsAuthenticated = User.Identity?.IsAuthenticated,
                Roles = User.Claims.Where(c => c.Type == "role").Select(c => c.Value)
            });
        }
        [HttpGet("Books")]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _booksService.GetBooks();
            var booksDTO = books.Select(book => new BookDto
            {
                Id = book.Id,
                Name = book.Title,
                Authors = book.BookAuthors.Select(author => new AuthorDto
                {
                    FirstName = author.FirstName,
                    LastName = author.LastName,
                    MiddleName = author.MiddleName,
                    DateOfBirth = author.DateOfBirth
                }).ToList(),
                PublishingCodeType = book.PublishingCodeType,
                Year = book.Year,
                Country = book.PublishingCountry,
                City = book.PublishingCity,
                DaysBorrowed = book.DaysBorrowed
            }).ToList();
            return Ok(booksDTO);
        }

        [HttpGet("BooksByAuthors")]
        public async Task<IActionResult> BookByAuthors(FullNameAuthorDto authorDto)
        {
            var result = (await _booksService.GetBooks())
                .Where(book =>
                        book.BookAuthors
                            .Any(a => a.FirstName.Contains(authorDto.FirstName) &&
                            a.LastName.Contains(authorDto.LastName) &&
                            a.MiddleName.Contains(authorDto.MiddleName)));
            var booksDTO = result.Select(book => new BookDto
            {
                Id = book.Id,
                Name = book.Title,
                Authors = book.BookAuthors.Select(author => new AuthorDto
                {
                    FirstName = author.FirstName,
                    LastName = author.LastName,
                    MiddleName = author.MiddleName,
                    DateOfBirth = author.DateOfBirth
                }).ToList(),
                PublishingCodeType = book.PublishingCodeType,
                Year = book.Year,
                Country = book.PublishingCountry,
                City = book.PublishingCity,
                DaysBorrowed = book.DaysBorrowed
            }).ToList();
            return Ok(booksDTO);
        }

        [HttpGet("BookByTitle")]
        public async Task<IActionResult> BoksByTitle(string title)
        {
            var result = (await _booksService.GetBooks())
                .Where(b => b.Title.Contains(title));
            if (result == null || !result.Any()) { return NotFound("Book not found"); }
            var booksDTO = result.Select(book => new BookDto
            {
                Id = book.Id,
                Name = book.Title,
                Authors = book.BookAuthors.Select(author => new AuthorDto
                {
                    FirstName = author.FirstName,
                    LastName = author.LastName,
                    MiddleName = author.MiddleName,
                    DateOfBirth = author.DateOfBirth
                }).ToList(),
                PublishingCodeType = book.PublishingCodeType,
                Year = book.Year,
                Country = book.PublishingCountry,
                City = book.PublishingCity,
                DaysBorrowed = book.DaysBorrowed
            }).ToList();
            return Ok(booksDTO);
        }
        [HttpGet("Authors")]
        public async Task<IActionResult> GetAuthors()
        {
            var Authors = await _libraryContext.Authors.ToListAsync();
            if (Authors == null || !Authors.Any()) { return NotFound("Authors not found"); }
            return Ok(Authors);
        }
        [HttpGet("AuthorsByName")]
        public async Task<IActionResult> GetAuthorsByName(string name) => (await _libraryContext.Authors.AnyAsync(a => a.FirstName.Contains(name)))
        ? Ok(await _libraryContext.Authors.Where(a => a.FirstName.Contains(name)).ToListAsync())
        : NotFound("Author not found");

        [HttpGet("GetDebtorsReaders")]
        public async Task<IActionResult> GetDebtorsReaders()
        {
            var borr = await _booksService.BorrowingHistory();
            var res = borr.Where(r => r.BorrowedBooks.Any(b => b.DateReturned == null));
            return Ok(res);
        }

        [HttpGet("GetBorrowingHistory")]
        public async Task<IActionResult> GetBorrowingHistory() => Ok(await _booksService.BorrowingHistory());
        [HttpGet("GetBorrowingHistoryUser")]
        public async Task<IActionResult> GetBorrowingHistoryUser(string login)
        {
            var borr = await _booksService.BorrowingHistory();
            var historyBorriwingUser = borr.Where(u => u.Login == login)
                    .SelectMany(u => u.BorrowedBooks)  
                    .OrderBy(b => b.DateBorrowed)  
                    .ToList();
            if (historyBorriwingUser != null)
            {
                int countOverdue = historyBorriwingUser.Count(b => b.DateForBorrowed < DateTime.Now && b.DateReturned == null);
                BorrowingHistoryUserDto reader = new BorrowingHistoryUserDto(historyBorriwingUser, countOverdue);
                return Ok(reader);
            }
            return BadRequest();
        }

        [HttpPost("AddBook")]
        public async Task<IActionResult> AddBook(BookDto bookDto)
        {
            PublishingCodeType pubCod = _libraryContext.PublishingCodeTypes.FirstOrDefault(a => a.Name == bookDto.PublishingCodeType.Name);
            if (pubCod == null)
            {
                pubCod = new PublishingCodeType() { Name = bookDto.PublishingCodeType.Name! };
            }
            Book book = new Book()
            {
                Title = bookDto.Name,
                BookAuthors = bookDto.Authors.Select(x => new BookAuthor()
                {
                    FirstName = x.FirstName,
                    MiddleName = x.MiddleName,
                    LastName = x.LastName,
                    DateOfBirth = x.DateOfBirth
                }).ToList(),
                PublishingCodeType = pubCod,
                Year = bookDto.Year ?? 0,
                PublishingCountry = bookDto.Country,
                PublishingCity = bookDto.City,
                DaysBorrowed = bookDto.DaysBorrowed ?? 0
            };
            _libraryContext.Books.Add(book);
            return await _libraryContext.SaveChangesAsync() > 0 ? Ok() : BadRequest();
        }

        [HttpPatch("UpdateBook/{findTitle}")]
        public async Task<IActionResult> UpdateBook(string findTitle, BookDto bookDto)
        {
            var existingBook = await _libraryContext.Books.Include<Book, object>(b => b.BookAuthors).FirstOrDefaultAsync(b => b.Title == findTitle);

            if (existingBook == null)
            {
                return NotFound("Book not found.");
            }
            if (!string.IsNullOrEmpty(bookDto.Name))
            {
                existingBook.Title = bookDto.Name;
            }

            if (bookDto.Year != default)
            {
                existingBook.Year = bookDto.Year ?? existingBook.Year;
            }
            if (!string.IsNullOrEmpty(bookDto.Country))
            {
                existingBook.PublishingCountry = bookDto.Country;
            }
            if (!string.IsNullOrEmpty(bookDto.City))
            {
                existingBook.PublishingCity = bookDto.City;
            }
            if (bookDto.DaysBorrowed != default)
            {
                existingBook.DaysBorrowed = bookDto.DaysBorrowed ?? existingBook.DaysBorrowed;
            }
            return await _libraryContext.SaveChangesAsync() > 0 ? Ok() : BadRequest();
        }

        [HttpPost("AddAuthor")]
        public async Task<IActionResult> AddAuthor(AuthorDto authorDto)
        {
            var findAuthor = await _libraryContext.Authors.FirstOrDefaultAsync(a => a.FirstName == authorDto.FirstName && a.MiddleName == authorDto.MiddleName && a.LastName == authorDto.LastName && a.DateOfBirth == authorDto.DateOfBirth);
            if (findAuthor == null)
            {
                Author author = new Author()
                {
                    FirstName = authorDto.FirstName,
                    LastName = authorDto.LastName,
                    MiddleName = authorDto.MiddleName,
                    DateOfBirth = authorDto.DateOfBirth
                };
                _libraryContext.Authors.Add(author);
            }
            return await _libraryContext.SaveChangesAsync() > 0 ? Ok() : BadRequest();
        }
        [HttpPatch("UpdateAuthor/{findAuthor}")]
        public async Task<IActionResult> UpdateAuthor(string findAuthor, AuthorDto bookDto)
        {
            var s = await _authorUpdate.Update(findAuthor, bookDto);
            return await _libraryContext.SaveChangesAsync() > 0 ? Ok() : BadRequest();
        }

        [HttpDelete("DeleteAuthor")]
        public async Task<IActionResult> DeleteAuthor(AuthorDto authorDto)
        {
            var findAuthor = await _libraryContext.Authors.FirstOrDefaultAsync(a => a.FirstName == authorDto.FirstName && a.LastName == authorDto.MiddleName && a.LastName == authorDto.LastName && a.DateOfBirth == authorDto.DateOfBirth);
            if (findAuthor != null) _libraryContext.Authors.Remove(findAuthor);
            return await _libraryContext.SaveChangesAsync() > 0 ? Ok() : BadRequest();
        }
        [HttpDelete("DeleteReader")]
        public async Task<IActionResult> DeleteReader(string login)
        {
            var user = await _libraryContext.Readers.FirstOrDefaultAsync(r => r.Login == login);
            if (user != null) _libraryContext.Readers.Remove(user);

            return await _libraryContext.SaveChangesAsync() > 0 ? Ok() : BadRequest();
        }
        [HttpDelete("DeleteBook")]
        public async Task<IActionResult> DeleteBook(string title)
        {
            var book = await _libraryContext.Books.FirstOrDefaultAsync(b => b.Title == title);
            if (book != null) _libraryContext.Books.Remove(book);
            return await _libraryContext.SaveChangesAsync() > 0 ? Ok() : BadRequest();
        }
        [HttpPost("AddBorrowedBook")]
        public async Task<IActionResult> AddBorrowedBook(string login, string title)
        {
            var result = (await _booksService.GetBooks())
                .Where(b => b.Title.Contains(title));
            if (result == null || !result.Any())
            {
                BorrowedBook borrowedBook = new BorrowedBook()
                {
                    Reader = await _libraryContext.Readers.FirstAsync(r => r.Login == login),
                    Book = await _libraryContext.Books.FirstAsync(b => b.Title == title),
                    DateBorrowed = DateTime.Now,
                };
                _libraryContext.BorrowedBooks.Add(borrowedBook);
            }
            return await _libraryContext.SaveChangesAsync() > 0 ? Ok() : BadRequest();
        }
        [HttpPatch("ReturnBook")]
        public async Task<IActionResult> ReturnBook(string title, DateTime dateReturn)
        {
            var result = _libraryContext.BorrowedBooks.Include(b=>b.Book).Include(r=>r.Reader).FirstOrDefaultAsync(bb=>bb.Book.Title==title&& bb.DateReturned==null).Result;
            if (result == null) return NotFound("Book not found");
            result.DateReturned = dateReturn;
            return await _libraryContext.SaveChangesAsync() > 0 ? Ok() : BadRequest();
        }
       
   }