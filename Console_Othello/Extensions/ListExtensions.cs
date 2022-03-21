using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Othello.Extensions
{
    public static class ListExtensions
    {
        public static T Random<T>(this IEnumerable<T> list)
        {
            if(!list.Any())
            {
                return default(T);
            }

            var rand = new Random();

            var index = rand.Next(list.Count() - 1);

            return list.ElementAt(index);
        }
    }
}
