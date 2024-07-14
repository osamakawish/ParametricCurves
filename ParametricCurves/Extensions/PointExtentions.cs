using System.Windows;

namespace ParametricCurves.Extensions;

public static class PointExtentions
{
    public static bool IsNaNPoint(this Point point) => double.IsNaN(point.X) || double.IsNaN(point.Y);
}
