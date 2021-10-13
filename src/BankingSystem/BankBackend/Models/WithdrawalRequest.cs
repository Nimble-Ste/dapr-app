namespace BankBackend.Models
{
    public class WithdrawalRequest
    {
        public string AccountNumber { get; set; }

        public double Amount { get; set; }
    }
}
