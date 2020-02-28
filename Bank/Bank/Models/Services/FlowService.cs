using System.Collections.Generic;
using System.Linq;

namespace Bank.Models
{
    public class FlowService
    {
        private readonly IEnumerable<ISkippable> _skippables;

        public FlowService(IEnumerable<ISkippable> skippables)
        {
            _skippables = skippables;
        }

        public void SkipDay()
        {
            foreach (var item in _skippables)
            {
                item.SkipDay();
            }
        }

        public void Close<T>(int accountId, bool closedInTime)
        {
            _skippables.FirstOrDefault(i => i.GetType() == typeof(T))?.Close(accountId, closedInTime);
        }
    }
}
