using ParametricCurves.Extensions;

namespace ParametricCurves;

public static class ISetExtensions
{
    public static void AddRange<T>(this ISet<T> set, IEnumerable<T> other)
        => other.ForEach(item => set.Add(item));
}
