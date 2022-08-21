namespace brinux.sudokuSolver
{
	class Program
	{
		static void Main(string[] args)
		{
			var sudoku = new Sudoku();

			/*sudoku.Set(new int[9, 9]
			{
				{ 9, 0, 0, 0, 0, 6, 8, 1, 0 },
				{ 7, 0, 0, 0, 0, 3, 0, 0, 0 },
				{ 0, 0, 3, 0, 1, 0, 6, 0, 0 },
				{ 0, 1, 9, 0, 0, 0, 0, 0, 0 },
				{ 0, 2, 0, 0, 0, 0, 0, 4, 0 },
				{ 0, 0, 0, 0, 0, 0, 5, 7, 0 },
				{ 0, 0, 8, 0, 9, 0, 2, 0, 0 },
				{ 0, 0, 0, 4, 0, 0, 0, 0, 1 },
				{ 0, 7, 6, 2, 0, 0, 0, 0, 5 }
			});*/
			sudoku.Set(new int[][]
			{
				new int[9] { 9, 5, 0, 0, 0, 6, 8, 1, 0 },
				new int[9] { 7, 0, 0, 0, 0, 3, 0, 0, 0 },
				new int[9] { 0, 0, 3, 0, 1, 0, 6, 0, 0 },
				new int[9] { 0, 1, 9, 0, 0, 0, 0, 0, 0 },
				new int[9] { 0, 2, 0, 0, 0, 0, 0, 4, 0 },
				new int[9] { 0, 0, 0, 0, 0, 0, 5, 7, 0 },
				new int[9] { 0, 0, 8, 0, 9, 0, 2, 0, 0 },
				new int[9] { 0, 0, 0, 4, 0, 0, 0, 0, 1 },
				new int[9] { 0, 7, 6, 2, 0, 0, 0, 0, 5 }
			});
			/*
			sudoku.Set(new int[][]
			{
				new int[9] { 9, 0, 0, 0, 0, 6, 8, 1, 3 },
				new int[9] { 7, 6, 1, 0, 0, 3, 0, 0, 0 },
				new int[9] { 0, 0, 3, 0, 1, 0, 6, 0, 7 },
				new int[9] { 0, 1, 9, 0, 0, 0, 3, 0, 0 },
				new int[9] { 0, 2, 7, 0, 0, 0, 1, 4, 0 },
				new int[9] { 0, 0, 4, 0, 0, 0, 5, 7, 0 },
				new int[9] { 0, 0, 8, 0, 9, 0, 2, 0, 0 },
				new int[9] { 0, 9, 0, 4, 0, 0, 7, 0, 1 },
				new int[9] { 0, 7, 6, 2, 0, 0, 0, 0, 5 }
			});*/

			sudoku.PrintSchema();

			var moves = sudoku.Solve();

			if (sudoku.IsSolved())
			{
				Console.WriteLine();
				Console.WriteLine("Solved!");

				PrintMoves(moves);

				sudoku.PrintSchema();
			}
			else
			{
				Console.WriteLine();
				Console.WriteLine("Unable to proceed...");

				Console.WriteLine();
				sudoku.PrintSchema();

				sudoku.PrintOptions();
			}
		}

		public static void PrintMoves(List<MoveSelection> moves)
		{
			if (moves.Count > 0)
			{
				Console.WriteLine();
				Console.WriteLine("Moves:");
			}

			foreach (var move in moves)
			{
				move.Print();
			}
		}
	}
}