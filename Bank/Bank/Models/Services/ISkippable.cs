namespace Bank.Models
{
    public interface ISkippable
    {
        void SkipDay();

        bool Close(int accountId, bool closeInTime);
    }
}
