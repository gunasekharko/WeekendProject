public class LendingHistory
{
    public int LendingId { get; set; }
    public int BookId { get; set; }

    public int BorrowerId { get; set; }

    public DateTime BorrowedDate { get; set; }
    
    public DateTime ReturnedDate{ get; set; }
}