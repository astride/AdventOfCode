using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AdventOfCode.Runner
{
	// https://adventofcode.com/
	class Program
	{
		static void Main(string[] args)
		{
			var today = DateTime.Today;
			var availablePuzzles = GetAvailablePuzzles();

			if (availablePuzzles.Any(puzzle => puzzle.Date == today))
			{
				Console.WriteLine($"Vil du løse dagens oppgave? Idag er det {today.ToString("ddd dd.MM.yyyy")} (svar blankt for 'ja'): ");

				if (string.IsNullOrEmpty(Console.ReadLine()))
				{
					var puzzle = availablePuzzles.First(puzzle => puzzle.Date == today);

					RequestModeAndSolve(today.Year, today.Day, puzzle.SolverType);
				}
			}

			var availablePuzzleDates = availablePuzzles.Select(puzzle => puzzle.Date);

			int year = RequestYear(availablePuzzleDates);
			int day = RequestDay(year, availablePuzzleDates);

			var selectedPuzzle = availablePuzzles
				.First(puzzle => 
					puzzle.Date.Year == year &&
					puzzle.Date.Day == day);

			RequestModeAndSolve(year, day, selectedPuzzle.SolverType);
		}

		static IEnumerable<PuzzleInfo> GetAvailablePuzzles()
		{
			var puzzleSolvers = GetPuzzleSolvers();
			var puzzleInputs = GetPuzzleInputFilePaths();

			var puzzleDates = puzzleSolvers
				.Select(solver => solver.PuzzleDate)
				.Intersect(puzzleInputs
					.Select(input => input.PuzzleDate));

			puzzleInputs = puzzleInputs.Where(input => puzzleDates.Contains(input.PuzzleDate));

			var puzzles = puzzleInputs
				.Select(input => new PuzzleInfo(
					input.PuzzleDate,
					puzzleSolvers.First(solver => solver.PuzzleDate == input.PuzzleDate).SolverType,
					input.inputFilePath,
					input.inputExampleFilePath));

			return puzzles;
		}

		static string GetPuzzleRootLocation()
		{
			var assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly()?.Location);

			if (assemblyLocation == null)
			{
				throw new Exception("Assembly location not found");
			}

			var puzzleRootLocation = Directory.GetParent(assemblyLocation).Parent.Parent.FullName;

			if (puzzleRootLocation == null)
			{
				throw new Exception($"Puzzle root location not found. Tried to find grandparent of the assembly location ({assemblyLocation})");
			}

			return puzzleRootLocation;
		}

		static int RequestYear(IEnumerable<DateTime> availableDates)
		{
			var availableYears = availableDates
				.Select(date => date.Year)
				.Distinct();

			while (true)
			{
				Console.WriteLine($"Hvilket år vil du bryne deg på? Du kan velge mellom {string.Join(", ", availableYears)}: ");

				if (int.TryParse(Console.ReadLine(), out var year))
				{
					return year;
				}
			}
		}

		static int RequestDay(int year, IEnumerable<DateTime> availableDates)
		{
			var availableDays = availableDates
				.Where(date => date.Year == year)
				.Select(date => date.Day);

			while (true)
			{
				Console.WriteLine($"Hvilken dag i {year} vil du løse oppgaven for? Du kan velge mellom {string.Join(", ", availableDays)}: ");

				if (int.TryParse(Console.ReadLine(), out var day))
				{
					return day;
				}
			}
		}

		static bool RequestTestMode()
		{
			Console.WriteLine("Testmodus? (svar blankt for 'ja'): ");
			
			return string.IsNullOrEmpty(Console.ReadLine());
		}

		static void RequestModeAndSolve(int year, int day, Type solverType)
		{
			var testMode = RequestTestMode();
			var puzzleDay = new DateTime(year, 12, day);

			Console.WriteLine($"\nDa setter vi i gang! Oppgavedagen er {puzzleDay.ToString("dd. MMM yyyy")}, og vi løser den med {(testMode ? "testdata" : "reelle data")}.\n");

			var input = GetInputFor(year, day, testMode); //TODO

			var solver = (IPuzzleSolver)Activator.CreateInstance(solverType);

			solver.SolvePuzzle(input);

			Console.WriteLine("--- Løsninger ---");
			Console.WriteLine($"{nameof(solver.Part1Solution)}: {solver.Part1Solution}");
			Console.WriteLine($"{nameof(solver.Part2Solution)}: {solver.Part2Solution}");
		}

		static string[] GetInputFor(int year, int day, bool testMode)
		{
			var puzzleRootLocation = GetPuzzleRootLocation();

			var filePath = Path.Combine(puzzleRootLocation, DirectoryNameForYear(year), DirectoryNameForDay(day), InputFileName(day, testMode));

			if (!File.Exists(filePath))
			{
				throw new Exception($"No input file found when searching for {filePath}");
			}

			return File.ReadAllLines(filePath, Encoding.Default);
		}

		private static IEnumerable<(DateTime PuzzleDate, string inputFilePath, string inputExampleFilePath)> GetPuzzleInputFilePaths()
		{
			//TODO
			return default;
		}

		private static IEnumerable<(DateTime PuzzleDate, Type SolverType)> GetPuzzleSolvers()
		{
			var yearAndAssembly = new Dictionary<int, Assembly>
			{
				[2021] = typeof(Y2021.Day01).Assembly,
				[2020] = typeof(Y2020.Day01).Assembly
			};

			var solvers = new List<(DateTime Date, Type SolverType)>();

			var types = yearAndAssembly
				.Select(yaa =>
					new KeyValuePair<int, IEnumerable<Type>>(
						yaa.Key,
						yaa.Value.GetTypes()
							.Where(type => IsPuzzleSolver(type))))
				.SelectMany(yearAndTypes => yearAndTypes.Value
					.Select(type => (GetPuzzleDateFor(yearAndTypes.Key, type), type)));

			return types;
		}

		private static bool IsPuzzleSolver(Type type)
		{
			//TODO
			return default;
		}

		private static DateTime GetPuzzleDateFor(int year, Type solverType)
		{
			//TODO
			return default;
		}

		private static string InputFileName(int day, bool testMode) => testMode
			? $"{InputFileNameBaseForDay(day)}Example.txt"
			: $"{InputFileNameBaseForDay(day)}.txt";

		private static string InputFileNameBaseForDay(int day) => $"{DirectoryNameForDay(day)}Input";

		private static string DirectoryNameForYear(int year) => $"AdventOfCode.{year}";

		private static string DirectoryNameForDay(int day) => $"Day{day:D2}";
	}

	public class PuzzleInfo
	{
		public PuzzleInfo(DateTime date, Type solverType, string inputFilePath, string inputExampleFilePath)
		{
			Date = date;
			SolverType = solverType;
			InputFilePath = inputFilePath;
			InputExampleFilePath = inputExampleFilePath;
		}

		public DateTime Date { get; set; }

		public Type SolverType { get; set; }

		public string InputFilePath { get; set; }

		public string InputExampleFilePath {get; set; }
	}
}
