using System.Linq;

namespace WebsitePoller.Entities
{
    public static class ArrayHashExtensions
    {
        public static int GetHashCode(this string[] array)
        {
            return array == null ? 0 
                : array.Aggregate(0, (current, item) => (item.GetHashCode() * 397) ^ current);
        }

        public static int GetHashCode(this int[] array)
        {
            return array == null ? 0
                : array.Aggregate(0, (current, item) => (item.GetHashCode() * 397) ^ current);
        }
    }
}