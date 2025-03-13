using ContextAndModels;

namespace Database.Library.Entity;

public class ВоокBorrower
{
    public int Id { get; set; }         
    public int BookId { get; set; }         
    public int ReaderId { get; set; }       
    public DateTime LoanDate { get; set; }  
    public DateTime? ReturnDate { get; set; } 
    public int LoanPeriod { get; set; }     

    public Book Book { get; set; }  
    public Reader Reader { get; set; }
}