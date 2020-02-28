namespace Bank.Models
{
    public interface ISkippable
    {
        void SkipDay();

        void Close(int accountId, bool closeInTime);
    }
}
