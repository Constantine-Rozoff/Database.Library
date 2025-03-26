using ContextAndModels.Models;

namespace LibraryAPI.Services.Interfaces;

public interface ITokenService
{
    string CreateToken(Employee user);
}