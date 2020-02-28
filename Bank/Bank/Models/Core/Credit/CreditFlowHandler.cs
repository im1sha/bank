namespace Bank.Models
{
    public class CreditFlowHandler : ISkippable
    {
        private readonly BankAppDbContext _db;
        private readonly CreditDbEntityRetriever _creditDb;
        private readonly TimeService _timeService;

        public CreditFlowHandler(CreditDbEntityRetriever creditDb, TimeService timeService, BankAppDbContext db)
        {
            _creditDb = creditDb;
            _timeService = timeService;
            _db = db;
        }

        public void Close(Account source, bool closedInTime)
        {
        }

        public void SkipDay()
        {
        }
    }
}
