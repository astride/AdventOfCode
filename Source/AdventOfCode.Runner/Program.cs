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
		private static IDictionary<int, Assembly> _assemblyByYear = new Dictionary<int, Assembly>
		{
			[2021] = typeof(Y2021.Day01Solver).Assembly,
			[2020] = typeof(Y2020.Day01Solver).Assembly
		};

		private static readonly string _puzzleRootLocation = 
			Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;

		private const int December = 12;
		private const string Day = "Day";
		private const string Year = "Y";
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
					RequestModeAndSolve(availablePuzzles.Single(puzzle => puzzle.Date == today));
				}
			}
			else
			{
				var availablePuzzleDates = availablePuzzles.Select(puzzle => puzzle.Date);

				int year = ResolveYear(availablePuzzleDates);
				int day = ResolveDay(availablePuzzleDates, year);

				RequestModeAndSolve(availablePuzzles.First(puzzle => puzzle.Date == new DateTime(year, December, day)));
			}
		}

		static IEnumerable<PuzzleSolverInfo> GetAvailablePuzzles()
		{
			var puzzleSolvers = GetPuzzleSolvers();

			var availablePuzzleDates = puzzleSolvers
				.Select(solver => solver.Date)
				.Intersect(GetPuzzleInputDates())
				.Distinct();

			puzzleSolvers = puzzleSolvers.Where(solver => availablePuzzleDates.Contains(solver.Date));

			return puzzleSolvers;
		}

		static int ResolveYear(IEnumerable<DateTime> availableDates)
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

				Console.WriteLine($"\nThere are no puzzle solvers available for year {year}.");
			}
		}

		static int ResolveDay(IEnumerable<DateTime> availableDates, int year)
		{
			var availableDays = availableDates
				.Where(date => date.Year == year)
				.Select(date => date.Day);

			if (availableDays.Count() == 1)
			{
				var day = availableDays.Single();

				Console.WriteLine($"\nThe only puzzle solver existing for {year} is for day {day}.\n");

				return day;
			}

			while (true)
			{
				Console.WriteLine($"\nFor which day in {year} would you like to solve the puzzle? Available days are {string.Join(", ", availableDays.OrderBy(day => day))}:");

				if (int.TryParse(Console.ReadLine(), out var day) &&
					availableDays.Contains(day))
				{
					return day;
				}

				Console.WriteLine($"\nDay {day} is not available for year {year}.");
			}
		}

		static void RequestModeAndSolve(PuzzleSolverInfo puzzleSolver)
		{
			Console.WriteLine("\nTest mode? (leave blank for 'yes')");
			var testMode = string.IsNullOrEmpty(Console.ReadLine());

			var solver = (IPuzzleSolver)Activator.CreateInstance(puzzleSolver.Type);

			var input = GetInput(puzzleSolver.Date, testMode);

			solver.SolvePuzzle(input);

			var additionalInfo = testMode ? " (test data)" : string.Empty;

			Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n");
			Console.WriteLine($"Let's go! Puzzle day is {puzzleSolver.Date.ToString("dd. MMM yyyy")}.\n");
			Console.WriteLine($"{nameof(solver.Part1Solution)}{additionalInfo}: {solver.Part1Solution}\n");
			Console.WriteLine($"{nameof(solver.Part2Solution)}{additionalInfo}: {solver.Part2Solution}\n");
			Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n");
		}

		static string[] GetInput(DateTime date, bool testMode)
		{
			var yearDir = $"{AdventOfCode}.{date.Year}";
			var dayDir = $"{Day}{date.Day:D2}";
			var fileName = testMode ? InputExampleFileName : InputFileName;

			var filePath = Path.Combine(_puzzleRootLocation, yearDir, dayDir, fileName);

			if (!File.Exists(filePath))
			{
				throw new Exception($"No input file found when searching for {filePath}");
			}

			return File.ReadAllLines(filePath, Encoding.Default);
		}

		private static IEnumerable<DateTime> GetPuzzleInputDates()
		{
			var inputFilePaths = Directory.GetFiles(_puzzleRootLocation, InputWildcardFileName, SearchOption.AllDirectories);

			var pathByYearCollection = inputFilePaths
				.Select(path => path.Without(_puzzleRootLocation + Path.DirectorySeparatorChar))
				.GroupBy(path => int.Parse(path
					.Split(Path.DirectorySeparatorChar)
					.First(dirName => dirName.Contains(AdventOfCode))
					.Without($"{AdventOfCode}.")));

			IEnumerable<IGrouping<string, string>> pathByDayCollection;
			IEnumerable<string> inputFileNames;
			int day;

			foreach (var yearAndPath in pathByYearCollection)
			{
				pathByDayCollection = yearAndPath.GroupBy(path => path.Split(Path.DirectorySeparatorChar)[1]);

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
			var solvers = _assemblyByYear
				.SelectMany(yearAndAssembly => yearAndAssembly.Value
					.GetTypes()
					.Where(type => IsPuzzleSolver(type)))
				.Select(type => new PuzzleSolverInfo(GetPuzzleDateFor(type), type));

			return solvers;
		}

		private static bool IsPuzzleSolver(Type type)
		{
			if (!type.Name.StartsWith(Day) || !type.Name.EndsWith(Solver))
			{
				return false;
			}

			var day = type.Name.Without(Day).Without(Solver);

			if (day.Length != 2 || !int.TryParse(day, out _))
			{
				return false;
			}

			return true;
		}

		private static DateTime GetPuzzleDateFor(Type solverType)
		{
			var yearString = solverType.FullName
				.Split('.')
				.First(dirName => dirName.Contains(Year))
				.Without(Year);

			var dayString = solverType.Name
				.Without(Day)
				.Without(Solver);

			if (int.TryParse(dayString, out int day) && day > 0 && day <= 25 &&
				int.TryParse(yearString, out int year) && year >= DateTime.MinValue.Year)
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
