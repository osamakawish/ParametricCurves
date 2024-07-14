using System.Windows;

namespace ParametricCurves.PointCollection;

/// <summary>
/// Allows storing a point by reference and record.
/// </summary>
/// <param name="Point"></param>
public record PointNode(Point Point, PointCollection XCollection, PointCollection YCollection);

public record Node();