namespace Sufka.Game.Utility
{
    public static class PolishTextUtility
    {
        private const string PUNKTOW_STRING = "punktów";
        private const string PUNKT_STRING = "punkt";
        private const string PUNKTY_STRING = "punkty";
        
        private const string ZADANIE_STRING = "zadanie";
        private const string ZADANIA_STRING = "zadania";
        private const string ZADAN_STRING = "zadań";
        

        public static string GetProperPointsString(int value)
        {
            string pointsString;

            if (value % 10 == 0)
            {
                pointsString = PUNKTOW_STRING;
            }
            else if (value % 10 == 1)
            {
                pointsString = value < 10 ? PUNKT_STRING : PUNKTOW_STRING;
            }
            else if (value % 10 <= 4)
            {
                pointsString = value % 100 <10 ? PUNKTY_STRING : PUNKTOW_STRING;
            }
            else
            {
                pointsString = PUNKTOW_STRING;
            }

            return pointsString;
        }

        public static string GetProperTasksString(int value)
        {
            string tasksString;

            if (value % 10 == 1)
            {
                tasksString = value < 10 ? ZADANIE_STRING : ZADAN_STRING;
            }
            else if (value % 10 <= 4)
            {
                tasksString = ZADANIA_STRING;
            }
            else
            {
                tasksString = ZADAN_STRING;
            }
            
            return tasksString;
        }
    }
}
