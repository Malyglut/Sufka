using System;

namespace Sufka.Game.Utility
{
    [Serializable]
    public class DateTimeSaveData
    {
        public int year;
        public int month;
        public int day;
        public int hour;
        public int minute;
        public int second;

        public DateTimeSaveData(DateTime dateTime)
        {
            year = dateTime.Year;
            month = dateTime.Month;
            day = dateTime.Day;
            hour = dateTime.Hour;
            minute = dateTime.Minute;
            second = dateTime.Second;
        }
    }
}
