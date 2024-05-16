namespace library;

public class Transaction {
    public int Id { get; set; }
    public int FromAccountId { get; set; }
    public int ToAccountId { get; set; }
    public float Amount { get; set; }
    public DateTime TransactionDate { get; set; }
}