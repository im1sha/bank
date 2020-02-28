namespace Bank.Models
{
    public interface ISkippable
    {
        void SkipDay();

        void Close(Account source, bool closeInTime);
    }
}
