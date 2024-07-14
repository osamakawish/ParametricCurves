namespace ParametricCurves.Extensions;

public static class IEnumerableExtensions
{
    public static void ForEach<T>(this IEnumerable<T> set, Action<T> action)
    {
        foreach (var item in set) action(item);
    }
}
