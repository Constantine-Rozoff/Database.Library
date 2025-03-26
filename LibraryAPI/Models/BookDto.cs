using ContextAndModels.Models;
using Database.Library.Entity;

namespace LibraryAPI.Models;

public class BookDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<AuthorDto> Authors { get; set; }
    public PublishingCodeType? PublishingCodeType { get; set; }
    public int? Year { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public int? DaysBorrowed { get; set; }
}