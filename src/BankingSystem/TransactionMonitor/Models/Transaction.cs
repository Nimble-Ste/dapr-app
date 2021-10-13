namespace TransactionMonitor.Models
{
    public class Transaction
    {
        public string AccountId { get; set; }

        public TransactionType TransactionType { get; set; }

        public double Amount { get; set; }
    }

    public enum TransactionType
    {
        Withdrawal = 1
    }
}
