namespace brinux.sudokuSolver
{
	class Sudoku
	{
		private int[][] Schema;
		private bool[][][] Options;

		public Sudoku()
		{
			ResetSchema();

			ResetOptions();
		}

		public bool IsSolved()
		{
			for (var r = 0; r < 9; r++)
			{
				Options[r] = new bool[9][];

				for (var c = 0; c < 9; c++)
				{
					if (Schema[r][c] == 0)
					{
						return false;
					}
				}
			}

			return true;
		}

		public List<MoveSelection> Solve()
		{
			List<MoveSelection>? results;
			List<MoveSelection> appliedMoves = new List<MoveSelection>();

			do
			{
				results = FindRulesResults();

				foreach (var result in results)
				{
					Set(result.Row, result.Column, result.Value);

					appliedMoves.Add(result);
				}
			}
			while (results != null && results.Count() > 0);

			return appliedMoves;
		}

		public void Set(int[][] setup)
		{
			if (setup.Length != 9 || setup[0].Length != 9)
			{
				throw new ArgumentException($"Sudoku schema must be 9x9, instead { setup.Length }x{ setup[0].Length } was provided.");
			}

			ResetSchema();

			ResetOptions();

			for (int r = 0; r < setup.Length; r++)
			{
				for (int c = 0; c < setup[r].Length; c++)
				{
					if (setup[r][c] != 0)
					{
						Set(r + 1, c + 1, setup[r][c]);
					}
				}
			}
		}

		public void Set(int row, int column, int value)
		{
			Set(new MoveSelection(row, column, value));
		}

		public void Set(MoveSelection move)
		{
			if (!Options[move.Row - 1][move.Column - 1][move.Value - 1])
			{
				throw new ArgumentOutOfRangeException("Sudoku schema cell value: c", $"Value { move.Value } is not acceptable for cell { move.Row }:{ move.Column }.");
			}

			Schema[move.Row - 1][move.Column - 1] = move.Value;

			for (int n = 0; n < 9; n++)
			{
				Options[move.Row - 1][move.Column - 1][n] = false;
			}

			for (var c = 0; c < 9; c++)
			{
				Options[move.Row - 1][c][move.Value - 1] = false;
			}

			for (var r = 0; r < 9; r++)
			{
				Options[r][move.Column - 1][move.Value - 1] = false;
			}

			for (var r = ((move.Row - 1) / 3) * 3; r < ((move.Row - 1) / 3) * 3 + 3; r++)
			{
				for (var c = ((move.Column - 1) / 3) * 3; c < ((move.Column - 1) / 3) * 3 + 3; c++)
				{
					Options[r][c][move.Value - 1] = false;
				}
			}

			while (HandleMandatoryLineOptionForSquare()) {}
		}

		public void PrintSchema()
		{
			Console.WriteLine();

			for (var r = 0; r < 9; r++)
			{
				for (var c = 0; c < 9; c++)
				{
					Console.Write($" {( Schema[r][c] == 0 ? "_" : Schema[r][c] )} ");
				}

				Console.WriteLine();
			}
		}

		public void PrintOptions()
		{
			for (var r = 0; r < 9; r++)
			{
				for (var c = 0; c < 9; c++)
				{
					if (Schema[r][c] == 0)
					{
						Console.Write($"Cell { r + 1 }:{ c + 1} - ");

						for (var n = 0; n < 9; n++)
						{
							if (Options[r][c][n])
							{
								Console.Write($"{ n + 1 } ");
							}
						}

						Console.WriteLine();
					}
				}

				Console.WriteLine();
			}
		}

		private void ResetSchema()
		{
			Schema = new int[9][];

			for (var r = 0; r < 9; r++)
			{
				Schema[r] = new int[9];

				for (var c = 0; c < 9; c++)
				{
					Schema[r][c] = 0;
				}
			}
		}

		private void ResetOptions()
		{
			Options = new bool[9][][];

			for (var r = 0; r < 9; r++)
			{
				Options[r] = new bool[9][];

				for (var c = 0; c < 9; c++)
				{
					Options[r][c] = new bool[9];

					for (var n = 0; n < 9; n++)
					{
						Options[r][c][n] = true;
					}
				}
			}
		}

		private List<MoveSelection> FindRulesResults()
		{
			var results = new List<MoveSelection>();

			for (var r = 0; r < 9; r++)
			{
				for (var c = 0; c < 9; c++)
				{
					if (Schema[r][c] == 0)
					{
						var result = SingleOptionForCell(r, c);

						if (result != null && !results.Any(r => r.Row == result.Row && r.Column == result.Column && r.Value == result.Value))
						{
							results.Add(result);
						}
					}

					if ((r + 1) % 3 == 0 && (c + 1) % 3 == 0)
					{
						for (var n = 1; n <= 9; n++)
						{
							var result = SingleLocationForSquare(r, c, n);

							if (result != null && !results.Any(r => r.Row == result.Row && r.Column == result.Column && r.Value == result.Value))
							{
								results.Add(result);
							}
						}
					}
				}
			}

			for (var i = 0; i < 9; i++)
			{
				for (var n = 1; n <= 9; n++)
				{
					var result = SingleLocationForColumn(i, n);

					if (result != null && !results.Any(r => r.Row == result.Row && r.Column == result.Column && r.Value == result.Value))
					{
						results.Add(result);
					}

					result = SingleLocationForRow(i, n);

					if (result != null && !results.Any(r => r.Row == result.Row && r.Column == result.Column && r.Value == result.Value))
					{
						results.Add(result);
					}
				}
			}

			return results;
		}

		private bool HandleMandatoryLineOptionForSquare()
		{
			var anyUpdate = false;

			for (var squareRow = 0; squareRow < 3; squareRow++)
			{
				for (var squareColumn = 0; squareColumn < 3; squareColumn++)
				{
					for (var n = 1; n <= 9; n++)
					{
						bool[] optionInRows = new bool[3] { false, false, false };
						bool[] optionInColumns = new bool[3] { false, false, false };

						for (var r = 0; r < 3; r++)
						{
							for (var c = 0; c < 3; c++)
							{
								if (Options[squareRow * 3 + r][squareColumn * 3 + c][n - 1])
								{
									optionInRows[r] = true;
									optionInColumns[c] = true;
								}
							}
						}

						if (optionInRows.Where(o => o).Count() == 1)
						{
							var currentlyUpdated = false;

							int rowIndex = -1;

							for (var i = 0; i < 3; i++)
							{
								if (optionInRows[i])
								{
									rowIndex = i;
									break;
								}
							}

							for (var c = 0; c < 9; c++)
							{
								if (Options[squareRow * 3 + rowIndex][c][n - 1] && (c < squareColumn * 3 || c > (squareColumn + 1) * 3 - 1))
								{
									Options[squareRow * 3 + rowIndex][c][n - 1] = false;
									currentlyUpdated = true;
								}
							}

							if (currentlyUpdated)
							{
								//Console.WriteLine();
								//Console.WriteLine($"Square { squareRow + 1 }:{ squareColumn + 1 } - Option for n. { n } set only for row { squareRow * 3 + rowIndex + 1 }.");
							}

							anyUpdate |= currentlyUpdated;
						}

						if (optionInColumns.Where(o => o).Count() == 1)
						{
							var currentlyUpdated = false;

							int columnIndex = -1;

							for (var i = 0; i < 3; i++)
							{
								if (optionInColumns[i])
								{
									columnIndex = i;
									break;
								}
							}

							for (var r = 0; r < 9; r++)
							{
								if (Options[r][squareColumn * 3 + columnIndex][n - 1] && (r < squareRow * 3 || r > (squareRow + 1) * 3 - 1))
								{
									Options[r][squareColumn * 3 + columnIndex][n - 1] = false;
									currentlyUpdated = true;
								}
							}

							if (currentlyUpdated)
							{
								//Console.WriteLine();
								//Console.WriteLine($"Square { squareRow + 1 }:{ squareColumn + 1 } - Option for n. { n } set only for column { squareRow * 3 + columnIndex + 1 }.");
							}

							anyUpdate |= currentlyUpdated;
						}
					}
				}
			}

			return anyUpdate;
		}

		private MoveSelection? SingleOptionForCell(int r, int c)
		{
			int count = 0;
			int number = -1;

			if (Schema[r][c] == 0)
			{
				for (var n = 1; n <= 9; n++)
				{
					if (Options[r][c][n - 1])
					{
						count++;
						number = n;
					}
				}
			}

			return count == 1 ?
				new RuleMoveSelection(r + 1, c + 1, number, "Only option in the cell") :
				null;
		}

		private MoveSelection? SingleLocationForRow(int r, int number)
		{
			int count = 0;
			int column = -1;

			for (var c = 0; c < 9; c++)
			{
				if (Schema[r][c] == 0 && Options[r][c][number - 1])
				{
					count++;
					column = c;
				}
			}

			return count == 1 ?
				new RuleMoveSelection(r + 1, column + 1, number, "Only option for the number in the row") :
				null;
		}

		private MoveSelection? SingleLocationForColumn(int c, int number)
		{
			int count = 0;
			int row = -1;

			for (var r = 0; r < 9; r++)
			{
				if (Schema[r][c] == 0 && Options[r][c][number - 1])
				{
					count++;
					row = r;
				}
			}

			return count == 1 ?
				new RuleMoveSelection(row + 1, c + 1, number, "Only option for the number in the column") :
				null;
		}

		private MoveSelection? SingleLocationForSquare(int r, int c, int number)
		{
			int count = 0;
			int row = -1;
			int column = -1;

			for (var currentRow = ((r - 1) / 3) * 3; currentRow < ((r - 1) / 3) * 3 + 3; currentRow++)
			{
				for (var currentColumn = ((c - 1) / 3) * 3; currentColumn < ((c - 1) / 3) * 3 + 3; currentColumn++)
				{
					if (Schema[r][c] == 0 && Options[r][c][number - 1])
					{
						count++;
						row = currentRow;
						column = currentColumn;
					}
				}
			}

			return count == 1 ?
				new RuleMoveSelection(row + 1, column + 1, number, "Only option for the number in the square") :
				null;
		}
	}
}