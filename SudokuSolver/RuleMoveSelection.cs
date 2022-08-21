namespace brinux.sudokuSolver
{
	public class RuleMoveSelection : MoveSelection
	{
		public string Description { get; }

		public RuleMoveSelection(int row, int column, int value, string description) : base(row, column, value)
		{
			Description = description;
		}

		public override void Print()
		{
			Console.WriteLine($"Row: { Row }, Column: { Column }, Value: { Value }, Rule: { Description }");
		}
	}
}
