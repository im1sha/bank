using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Bank
{
    public class TimeService
    {
        private readonly string _pathToStorage;
        private int _deltaDays;
        private int _deltaMonths;

        public DateTime CurrentTime => DateTime.Now.AddDays(_deltaDays).AddMonths(_deltaMonths);

        public TimeService(string pathToShiftStorage, int deltaDays, int deltaMonths)
        {
            _deltaDays = deltaDays;
            _deltaMonths = deltaMonths;
            _pathToStorage = pathToShiftStorage;
            WriteToStorage();
        }

        public void AddMonths(int months)
        {
            _deltaMonths += months;
            WriteToStorage();
        }

        public void AddDays(int days)
        {
            _deltaDays += days;
            WriteToStorage();
        }

        private void WriteToStorage()
        { 
            File.WriteAllText(_pathToStorage, $"{_deltaDays} {_deltaMonths}");
        }       

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
