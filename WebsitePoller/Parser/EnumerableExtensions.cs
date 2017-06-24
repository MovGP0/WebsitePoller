using System.Collections.Generic;
using System.Linq;

namespace WebsitePoller.Parser
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> WithoutNull<T>(this IEnumerable<T> items)
            where T : class 
        {
            return items.Where(item => !ReferenceEquals(item, null));
        }
    }
}