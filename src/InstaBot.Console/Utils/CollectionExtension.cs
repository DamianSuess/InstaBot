using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace InstaBot.Console.Utils
{
    public static class CollectionExtension
    {
        //public static void Shuffle<T>(this IList<T> list)
        //{
        //    var provider = new RNGCryptoServiceProvider();
        //    var n = list.Count;
        //    while (n > 1)
        //    {
        //        var box = new byte[1];
        //        do provider.GetBytes(box); while (!(box[0] < n*(byte.MaxValue/n)));
        //        var k = box[0]%n;
        //        n--;
        //        var value = list[k];
        //        list[k] = list[n];
        //        list[n] = value;
        //    }
        //}

        public static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                int k = rng.Next(n--);
                T temp = list[n];
                list[n] = list[k];
                list[k] = temp;
            }
        }
    }
}