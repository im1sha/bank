using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank
{
    public class TimeService
    {
        public void AddMonths(int months)
        {
            DeltaMonths += months;
        }

        public void AddDays(int days)
        {
            DeltaDays += days;
        }

        private static int DeltaDays = 0;
        private static int DeltaMonths = 0;

        public DateTime CurrentTime => DateTime.Now.AddDays(DeltaDays).AddMonths(DeltaMonths);

        public bool CheckActive(DateTime? termination)
        {
            if (termination == null)
            {
                return true;
            }

            return CurrentTime < termination;
        }
    }

}
