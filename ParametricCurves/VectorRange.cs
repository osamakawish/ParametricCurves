using System.Windows;

namespace ParametricCurves;

public record VectorRange(Vector Start, Vector End)
{
    public static VectorRange operator +(VectorRange left, VectorRange right)
        => new(left.Start + right.Start, left.End + right.End);

    public static VectorRange operator -(VectorRange left, VectorRange right)
        => new(left.Start - right.Start, left.End - right.End);
}
