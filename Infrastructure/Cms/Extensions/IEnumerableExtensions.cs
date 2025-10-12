using System.Collections;

namespace ChildFund.Infrastructure.Cms.Extensions;

public static class EnumerableExtensions
{
    public static TType FirstOfType<TType>(this IEnumerable list) => list.OfType<TType>().FirstOrDefault();

    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var element in source)
        {
            action(element);
        }
    }

    public static IEnumerable<TSource> CustomDistinctBy<TSource, TKey>
        (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        var seenKeys = new HashSet<TKey>();
        foreach (var element in source)
        {
            if (seenKeys.Add(keySelector(element)))
            {
                yield return element;
            }
        }
    }
}