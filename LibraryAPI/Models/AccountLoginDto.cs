using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models;

public class AccountLoginDto
{
    public record AccounLoginDto(
        [StringLength(20, MinimumLength = 2)] string login,
        [StringLength(50, MinimumLength = 2)] string password);
}