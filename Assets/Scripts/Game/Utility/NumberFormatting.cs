using System.Globalization;

namespace Sufka.Game.Utility
{
    public static class NumberFormatting
    {
        public static string Format(int number)
        {
            var numberFormat = new CultureInfo(CultureInfo.CurrentCulture.Name).NumberFormat;

            numberFormat.NumberDecimalDigits = 0;
            numberFormat.NumberGroupSeparator = " ";
            numberFormat.NumberGroupSizes = new[] {3};

            return number.ToString("N", numberFormat);
        }
    }
}
