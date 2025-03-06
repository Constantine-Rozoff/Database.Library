using Microsoft.EntityFrameworkCore;

namespace Database.Library.Entity
{
    public class LibraryContext  : DbContext
    {
        public LibraryContext()
        {
        }
        
        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Librarian> Librarians { get; set; }
    
        public virtual DbSet<Reader> Readers { get; set; }
    
        public virtual DbSet<DocumentType> DocumentTypes { get; set; }
    
        public virtual DbSet<Book> Books { get; set; }
    
        public virtual DbSet<Author> Authors { get; set; }
    
        public virtual DbSet<BookAuthor> BookAuthors { get; set; }
    
        public virtual DbSet<PublishingCodeType> PublishingCodeTypes { get; set; }

    
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("Server=localhost;Database=Library;Trusted_Connection=True;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookAuthor>()
                .HasKey(ba => new { ba.BookId, ba.AuthorId });

            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.Book)
                .WithMany(b => b.BookAuthors)
                .HasForeignKey(ba => ba.BookId);

            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.Author)
                .WithMany(a => a.BookAuthors)
                .HasForeignKey(ba => ba.AuthorId);
        }
    }
}
