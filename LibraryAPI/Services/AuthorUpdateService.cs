using System.Text.RegularExpressions;
using ContextAndModels.Models;
using Database.Library.Entity;
using LibraryAPI.Models;
using LibraryAPI.Services.Interfaces;

namespace LibraryAPI.Services;

internal class AuthorUpdateService : IAuthorUpdate
{
    private  LibraryContext _libraryContext;
    
    public AuthorUpdateService(LibraryContext libraryContext)
    {
        _libraryContext = libraryContext;
    }
    public async Task<Author> Update(string findAuthor, AuthorDto authorDto)
    {
        var result = Regex.Split(findAuthor, @"(?<=[а-яА-ЯёЁіІїЇєЄґҐA-Za-z])(?=[А-ЯA-Z])");
        Author updatedAuthor = new Author();

        if (result.Length == 3)
        {
            updatedAuthor =  _libraryContext.Authors
                .FirstOrDefault(a => a.FirstName.Contains(result[0]) && a.LastName.Contains(result[1]) && a.LastName.Contains(result[2]));
        }
        else
        {
            throw new ArgumentException("Invalid author format.");
        }
        if (updatedAuthor != null)
        {
            updatedAuthor.FirstName = authorDto.FirstName;
            updatedAuthor.LastName = authorDto.MiddleName;
            updatedAuthor.LastName = authorDto.LastName ?? updatedAuthor.LastName;
            if (authorDto.DateOfBirth != default)
            {
                updatedAuthor.DateOfBirth = authorDto.DateOfBirth;
            }
        }

        return updatedAuthor;
    }
}