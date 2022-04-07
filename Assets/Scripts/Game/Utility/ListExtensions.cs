using System;
using System.Collections.Generic;

namespace Sufka.Game.Utility
{
    public static class ListExtensions
    {
        private static Random rng = new Random();

        public static void Shuffle<T>(this List<T> list)
        {
            var n = list.Count;
            
            while (n > 1) 
            {  
                n--;  
                var k = rng.Next(n + 1);  
                var value = list[k];  
                list[k] = list[n];  
                list[n] = value;  
            }  
        }
    }
}
