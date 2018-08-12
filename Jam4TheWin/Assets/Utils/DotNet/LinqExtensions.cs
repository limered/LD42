using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils.DotNet
{
    public static class LinqExtensions
    {
        public static T NthElement<T>(this IEnumerable<T> coll, int n)
        {
            return coll.OrderBy(x => x).Skip(n - 1).FirstOrDefault();
        }

        public static T RandomElement<T>(this IList<T> coll)
        {
            var length = coll.Count;
            var random = new Random();
            return coll.ElementAt((int) (random.NextDouble()*length));
        }

        public static T RandomElement<T>(this T[] coll)
        {
            var length = coll.Length;
            var random = new Random();
            return coll.ElementAt((int)(random.NextDouble() * length));
        }
    }
}
