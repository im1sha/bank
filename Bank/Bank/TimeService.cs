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

        public DateTime CurrentTime { get; private set; }

        public TimeService(string pathToShiftStorage, DateTime dateTime)
        {
            CurrentTime = dateTime;
            _pathToStorage = pathToShiftStorage;
            WriteToStorage();
        }
     
        public void AddDays(int days)
        {
            CurrentTime = CurrentTime.AddDays(days);
            WriteToStorage();
        }

        public bool IsMultipleOfMonth(DateTime openDate)
        {
            if (CurrentTime < openDate)
            {
                throw new ArgumentNullException();
            }
            if (CurrentTime == openDate)
            {
                return false;
            }    
            return ((int)Math.Floor((CurrentTime - openDate).TotalDays) % 30 == 0);
        }

        private void WriteToStorage()
        { 
            File.WriteAllText(_pathToStorage, $"{CurrentTime.Year} {CurrentTime.Month} {CurrentTime.Day}");
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
