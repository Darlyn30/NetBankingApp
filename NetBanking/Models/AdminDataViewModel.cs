namespace WebApp.NetBanking.Models
{
    public class AdminDataViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalActiveUsers { get; set; }
        public int TotalInactiveUsers { get; set; }
        public int TotalTransactions { get; set; }
        public int TotalTodayTransactions { get; set; }
        public int TotalPayments { get; set; }
        public int TotalTodayPayments { get; set; }

        // Products
        public int TotalSavingsAccounts { get; set; }
        public int TotalLoans { get; set; }
        public int TotalCreditCards { get; set; }
    }
}
