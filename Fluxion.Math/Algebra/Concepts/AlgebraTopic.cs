namespace Fluxion.Math.Algebra.Concepts
{
    public sealed record FormulaItem(string Name, string Formula, string Notes = "");
    public sealed record AlgebraTopic(string Title, IReadOnlyList<FormulaItem> Items);
}
