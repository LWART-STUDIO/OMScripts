using System;
using System.Collections.Generic;
using System.Linq;

public static class ProjectTools
{
    public static bool AreAll<T>(this T[] source, Func<T, bool> condition)
    { return source.Where(condition).Count() == source.Count(); }

    public static bool AreAllTheSame<T>(this IEnumerable<T> source)
    { return source.Distinct().Count() == 1; }
    public static bool BruteforceSame(int[] input,int count)
    {
        for (int i = 1; i < input.Length; i++)
        {
            if (input[0] != input[i]) 
            {
                return false; 
            }
            else if(input[i] != count) { return false; }

                
        }

        return true;
    }
}
