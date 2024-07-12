using System.Windows;

namespace ParametricCurves;

public static class PointExt
{
    public static bool IsNaNPoint(this Point point) => double.IsNaN(point.X) || double.IsNaN(point.Y);
}
