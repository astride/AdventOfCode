using AdventOfCode.Common;
using AdventOfCode.Common.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AdventOfCode.Runner
{
	class Program
	{
		private static IDictionary<int, Assembly> _yearAndAssembly = new Dictionary<int, Assembly>
		{
			[2021] = typeof(Y2021.Day01Solver).Assembly,
			[2020] = typeof(Y2020.Day01Solver).Assembly
		};

		private static string _puzzleRootLocation;
		
		private const int December = 12;
		private const string Day = "Day";
		private const string Solver = "Solver";
		private const string AdventOfCode = "AdventOfCode";
		private const string InputWildcardFileName = "Input*.txt";
		private const string InputFileName = "Input.txt";
		private const string InputExampleFileName = "InputExample.txt";

		static void Main(string[] args)
		{
			var today = DateTime.Today;
			var availablePuzzles = GetAvailablePuzzles();

			if (availablePuzzles.Any(puzzle => puzzle.Date == today))
			{
				Console.WriteLine($"Do you want to solve today's puzzle? Today is {today.ToString("dd.MM.yyyy")} (leave blank for 'yes'):");

				if (string.IsNullOrEmpty(Console.ReadLine()))
				{
					var puzzle = availablePuzzles.Single(puzzle => puzzle.Date == today);

					RequestModeAndSolve(puzzle);
				}
			}
			else
			{
				var availablePuzzleDates = availablePuzzles.Select(puzzle => puzzle.Date);

				int year = RequestYear(availablePuzzleDates);
				int day = RequestDay(year, availablePuzzleDates);

				var selectedPuzzle = availablePuzzles
					.First(puzzle =>
						puzzle.Date.Year == year &&
						puzzle.Date.Day == day);

				RequestModeAndSolve(selectedPuzzle);
			}
		}

		static IEnumerable<PuzzleSolverInfo> GetAvailablePuzzles()
		{
			var puzzleSolvers = GetPuzzleSolvers();
			var puzzleInputDates = GetPuzzleInputDates();

			var puzzleDates = puzzleSolvers
				.Select(solver => solver.Date)
				.Intersect(puzzleInputDates)
				.Distinct();

			puzzleSolvers = puzzleSolvers.Where(solver => puzzleDates.Contains(solver.Date));

			return puzzleSolvers;
		}

		static string GetPuzzleRootLocation()
		{
			if (_puzzleRootLocation != null)
			{
				return _puzzleRootLocation;
			}

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

			_puzzleRootLocation = puzzleRootLocation;

			return _puzzleRootLocation;
		}

		static int RequestYear(IEnumerable<DateTime> availableDates)
		{
			var availableYears = availableDates
				.Select(date => date.Year)
				.Distinct();

			if (availableYears.Count() == 1)
			{
				var year = availableYears.Single();

				Console.WriteLine($"Puzzle solvers only exist for year {year}.");

				return year;
			}

			while (true)
			{
				Console.WriteLine($"From which year would you like to select a puzzle to solve? You can choose between {string.Join(", ", availableYears.OrderBy(year => year))}:");

				if (int.TryParse(Console.ReadLine(), out var year) &&
					availableYears.Contains(year))
				{
					return year;
				}

				Console.WriteLine($"There are no puzzle solvers available for year {year}.");
			}
		}

		static int RequestDay(int year, IEnumerable<DateTime> availableDates)
		{
			var availableDays = availableDates
				.Where(date => date.Year == year)
				.Select(date => date.Day);

			if (availableDays.Count() == 1)
			{
				var day = availableDays.Single();

				Console.WriteLine($"The only puzzle solver existing for {year} is for day {day}.");

				return day;
			}

			while (true)
			{
				Console.WriteLine($"For which day in {year} would you like to solve the puzzle? Available days are {string.Join(", ", availableDays.OrderBy(day => day))}:");

				if (int.TryParse(Console.ReadLine(), out var day) &&
					availableDays.Contains(day))
				{
					return day;
				}

				Console.WriteLine($"Day {day} is not available for year {year}.");
			}
		}

		static bool RequestTestMode()
		{
			Console.WriteLine("Test mode? (leave blank for 'yes')");
			
			return string.IsNullOrEmpty(Console.ReadLine());
		}

		static void RequestModeAndSolve(PuzzleSolverInfo puzzleSolver)
		{
			var testMode = RequestTestMode();

			var solver = (IPuzzleSolver)Activator.CreateInstance(puzzleSolver.Type);
			
			var input = GetInput(puzzleSolver, testMode);

			solver.SolvePuzzle(input);

			var additionalInfo = testMode ? " (test data)" : "";

			Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n");
			Console.WriteLine($"LET'S GO! Puzzle day is {puzzleSolver.Date.ToString("dd. MMM yyyy")}.\n");
			Console.WriteLine($"{nameof(solver.Part1Solution)}{additionalInfo}: {solver.Part1Solution}\n");
			Console.WriteLine($"{nameof(solver.Part2Solution)}{additionalInfo}: {solver.Part2Solution}\n");
			Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n");
		}

		static string[] GetInput(PuzzleSolverInfo puzzleSolver, bool testMode)
		{
			var day = $"{puzzleSolver.Date.Day:D2}";

			var yearDir = $"{AdventOfCode}.{puzzleSolver.Date.Year}";
			var dayDir = $"{Day}{day}";
			var fileName = testMode ? InputExampleFileName : InputFileName;

			var filePath = Path.Combine(GetPuzzleRootLocation(), yearDir, dayDir, fileName);

			if (!File.Exists(filePath))
			{
				throw new Exception($"No input file found when searching for {filePath}");
			}

			return File.ReadAllLines(filePath, Encoding.Default);
		}

		private static IEnumerable<DateTime> GetPuzzleInputDates()
		{
			var puzzleRootLocation = GetPuzzleRootLocation();

			var inputFilePaths = Directory.GetFiles(puzzleRootLocation, InputWildcardFileName, SearchOption.AllDirectories);

			var pathByYearCollection = inputFilePaths
				.Select(path => path
					.Substring((puzzleRootLocation + Path.DirectorySeparatorChar).Length))
				.GroupBy(path => int.Parse(path
					.Split(Path.DirectorySeparatorChar)
					.First(dir => dir.Contains(AdventOfCode))
					.Split('.')[1]));

			IEnumerable<IGrouping<string, string>> pathByDayCollection;
			IEnumerable<string> inputFileNames;
			int day;

			foreach (var yearAndPath in pathByYearCollection)
			{
				pathByDayCollection = yearAndPath.GroupBy(yap => yap.Split(Path.DirectorySeparatorChar)[1]);

				foreach (var dayAndPath in pathByDayCollection)
				{
					inputFileNames = dayAndPath.Select(path => path.Split(Path.DirectorySeparatorChar).Last());

					if (inputFileNames.Count(name => name == InputFileName) == 1 &&
						inputFileNames.Count(name => name == InputExampleFileName) == 1)
					{
						day = int.Parse(dayAndPath.Key.Without(Day));

						yield return (new DateTime(yearAndPath.Key, December, day));
					}
				}
			}
		}

		private static IEnumerable<PuzzleSolverInfo> GetPuzzleSolvers()
		{
			var yearAndTypesCollection = _yearAndAssembly
				.Select(yaa =>
					new KeyValuePair<int, IEnumerable<Type>>(
						yaa.Key,
						yaa.Value.GetTypes()
							.Where(type => IsPuzzleSolver(type))));

			var solvers = yearAndTypesCollection
				.SelectMany(yearAndTypes => yearAndTypes.Value
					.Select(type => new PuzzleSolverInfo(GetPuzzleDateFor(yearAndTypes.Key, type), type)));

			return solvers;
		}

		private static bool IsPuzzleSolver(Type type)
		{
			if (!type.Name.Contains(Day) ||
				!type.Name.Contains(Solver)) return false;

			var day = type.Name.Without(Day).Without(Solver);

			if (day.Length != 2 ||
				!int.TryParse(day, out _))
			{
				return false;
			}

			return true;
		}

		private static DateTime GetPuzzleDateFor(int year, Type solverType)
		{
			var dayString = solverType.Name.Without(Day).Without(Solver);

			if (int.TryParse(dayString, out int day))
			{
				return new DateTime(year, December, day);
			}

			return default;
		}
	}

	public class PuzzleSolverInfo
	{
		public PuzzleSolverInfo(DateTime date, Type solverType)
		{
			Date = date;
			Type = solverType;
		}

		public DateTime Date { get; set; }
		public Type Type { get; set; }
	}
}
