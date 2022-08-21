using System.ComponentModel.DataAnnotations;

namespace brinux.sudokuSolver
{
	public class MoveSelection
	{
		public int Row { get; }
		public int Column { get; }
		public int Value { get; }

		public MoveSelection(int row, int column, int value)
		{
			if (row < 1 || row > 9)
			{
				throw new ArgumentOutOfRangeException("Sudoku schema row number: r", "Value out of range: 1-9.");
			}

			if (column < 1 || column > 9)
			{
				throw new ArgumentOutOfRangeException("Sudoku schema column number: c", "Value out of range: 1-9.");
			}

			if (value < 1 || value > 9)
			{
				throw new ArgumentOutOfRangeException("Sudoku schema cell value: c", "Value out of range: 1-9.");
			}

			Row = row;
			Column = column;
			Value = value;
		}

		public virtual void Print()
		{
			Console.WriteLine($"Row: { Row }, Column: { Column }, Value: { Value }");
		}
	}
}
