using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksModule
{
    class CalendarManager
    {
        public static DateTime GetTaskEndTime(DateTime startDate, int duration)
        {
            DateTime tmpDate = startDate;
            if (duration != int.MinValue)
            {
                int days = duration;
                while (days != 0)
                {
                    tmpDate = tmpDate.AddDays(1);
                    if (tmpDate.DayOfWeek != DayOfWeek.Saturday && tmpDate.DayOfWeek != DayOfWeek.Sunday && !IsHoliday(tmpDate))
                        days--;
                }
            }
            return tmpDate;
        }

        public static int GetDaysQuantity(double hoursPerDay, double hours)
        {
            return (int)Math.Round(hours / hoursPerDay);
        }

        public static bool IsHoliday(DateTime date)
        {
            //TODO:holiday logic
            return false;
        }
    }
}
