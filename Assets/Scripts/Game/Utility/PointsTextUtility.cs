namespace Sufka.Game.Utility
{
    public static class PointsTextUtility
    {
        private const string PUNKTOW_STRING = "punktów";
        private const string PUNKT_STRING = "punkt";
        private const string PUNKTY_STRING = "punkty";

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
    }
}
