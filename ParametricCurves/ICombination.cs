namespace ParametricCurves;

public delegate Contour CurveCombination(Curve other);

public interface ICurveCombination
{
    /// <summary>
    /// Applies the desired combination to the current curve, placing both under the same contour.
    /// </summary>
    /// <param name="combination"></param>
    /// <param name="other"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void Apply(CurveCombination combination, Curve other) => throw new NotImplementedException();

    public Contour Intersect(Curve other) => throw new NotImplementedException();
    public Contour Union(Curve other) => throw new NotImplementedException();
    public Contour Exclusion(Curve other) => throw new NotImplementedException();
    public Contour Subtract(Curve other) => throw new NotImplementedException();
}
