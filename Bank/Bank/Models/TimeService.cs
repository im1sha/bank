using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
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
            if (CountElapsedDays(openDate) == 0)
            {
                return false;
            }    
            var result = CountElapsedDays(openDate) % 30 == 0;

            return result;
        }

        public int CountElapsedDays(DateTime startDay)
        {
            return (int)(CurrentTime - startDay).TotalDays;
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
