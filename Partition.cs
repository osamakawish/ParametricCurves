namespace ParametricCurves;

public record Partition(double Start, double End, int Subdivisions = 1)
{
    public bool Forward { get; } = Start < End;
    public double Delta { get; } = End - Start;
    public List<double> Values => [.. Enumerable.Range(0, Subdivisions + 1).Select(x => Start + x * Delta)];
    public double SortedDelta { get; } = (Start < End ? End - Start : Start - End) / Subdivisions;
    public List<double> SortedValues => [.. Enumerable.Range(0, Subdivisions + 1).Select(x => (Forward ? Start : End) + x * SortedDelta)];
}