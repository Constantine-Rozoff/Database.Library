using ContextAndModels.Models;
using Database.Library.Entity;
using LibraryAPI.Models;

namespace LibraryAPI.Services.Interfaces;

public interface IAuthorUpdate
{
    Task<Author> Update(string findAuthor, AuthorDto authorDto);
}