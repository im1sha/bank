using System;
using System.Collections.Generic;
using System.Linq;

namespace Bank.Models
{
    public class FlowService
    {
        private readonly IEnumerable<ISkippable> _skippables;
        private readonly TimeService _timeService;

        public FlowService(IEnumerable<ISkippable> skippables, TimeService timeService)
        {
            _skippables = skippables;
            _timeService = timeService;
        }

        public void SkipDay()
        {
            _timeService.AddDays(1);

            foreach (var item in _skippables)
            {
                item.SkipDay();
            }

            GC.Collect();
        }

        public void Close<T>(int accountId, bool closedInTime)
        {
            _skippables.FirstOrDefault(i => i.GetType() == typeof(T))?.Close(accountId, closedInTime);
        }

        public T GetSkippable<T>() where T : class
        {
            return _skippables.FirstOrDefault(i => i.GetType() == typeof(T)) as T;
        }
    }
}
