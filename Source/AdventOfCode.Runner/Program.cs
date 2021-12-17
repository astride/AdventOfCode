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

		private const string DateFormat = "dd. MMM yyyy";
		
		private const string Solver = "Solver";
		private const string AdventOfCode = "AdventOfCode";
		
		private const string InputWildcardFileName = "Input*.txt";
		private const string InputFileName = "Input.txt";
		private const string InputExampleFileName = "InputExample.txt";

		static void Main(string[] args)
		{
			ResolvePuzzleDateAndPlay();
		}

		static void ResolvePuzzleDateAndPlay()
        {
			var today = DateTime.Today;
			var yesterday = today.AddDays(-1);

			var availablePuzzles = GetAvailablePuzzles();

			if (availablePuzzles.Any(puzzle => puzzle.Date == today))
			{
				Console.WriteLine($"Do you want to solve today's puzzle? Today is {today.ToString(DateFormat)} (leave blank for 'yes'):");

				InterpretResponseFor(today, availablePuzzles);
			}
			else if (availablePuzzles.Any(puzzle => puzzle.Date == yesterday))
			{
				Console.WriteLine($"Today's puzzle is not available, but maybe you want to solve yesterday's puzzle? Yesterday was {yesterday.ToString(DateFormat)} (leave blank for 'yes'):");

				InterpretResponseFor(yesterday, availablePuzzles);
			}
			else
			{
				var latestAvailablePuzzleDate = availablePuzzles.Select(puzzle => puzzle.Date).Max();

				Console.WriteLine($"The latest available puzzle date is {latestAvailablePuzzleDate.ToString(DateFormat)}. Do you want to solve it? (leave blank for 'yes'):");

				InterpretResponseFor(latestAvailablePuzzleDate, availablePuzzles);
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

		static void InterpretResponseFor(DateTime suggestedDate, IEnumerable<PuzzleSolverInfo> availablePuzzles)
        {
			if (string.IsNullOrEmpty(Console.ReadLine()))
			{
				RequestModeAndSolve(availablePuzzles.Single(puzzle => puzzle.Date == suggestedDate));
			}
			else
			{
				RequestDate(availablePuzzles);
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
			Console.WriteLine($"Let's go! Puzzle day is {puzzleSolver.Date.ToString(DateFormat)}.\n");
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

		static void RequestDate(IEnumerable<PuzzleSolverInfo> availablePuzzles)
		{
			var availableDates = availablePuzzles.Select(puzzle => puzzle.Date);

			int year = ResolveYear(availableDates);
			int day = ResolveDay(availableDates, year);

			RequestModeAndSolve(availablePuzzles.First(puzzle => puzzle.Date == new DateTime(year, December, day)));
		}

		static int ResolveYear(IEnumerable<DateTime> availableDates)
		{
			var availableYears = availableDates
				.Select(date => date.Year)
				.Distinct();

			if (availableYears.Count() == 1)
			{
				var year = availableYears.Single();

				Console.WriteLine($"\nPuzzle solvers only exist for year {year}.");

				return year;
			}

			while (true)
			{
				Console.WriteLine($"\nFrom which year would you like to select a puzzle to solve? You can choose between {string.Join(", ", availableYears.OrderBy(year => year))}:");

				var response = Console.ReadLine();

				if (int.TryParse(response, out var year))
				{
					if (availableYears.Contains(year))
                    {
						return year;
                    }
					else if (year > DateTime.Today.Year ||
						(year == DateTime.Today.Year && DateTime.Today < new DateTime(year, December, 1)))
                    {
						Console.WriteLine($"\nGetting futuristic, now? No puzzles have been published for {year} yet.");
					}
					else if (year < 2015)
                    {
						Console.WriteLine("\nAdvent of Code published its first puzzle in 2015. No puzzles will ever exist prior to 2015.");
                    }
					else
                    {
						Console.WriteLine($"\nThere are no puzzle solvers available for year {year}.");
					}
				}
				else
				{
					Console.WriteLine($"\nTrying to fool me? Give me a number next time, please.");
				}
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

				var response = Console.ReadLine();

				if (int.TryParse(response, out var day))
                {
					if (availableDays.Contains(day))
                    {
						return day;
                    }
					else if (day < 1 || day > 31)
                    {
						Console.WriteLine($"\nCome on! What on earth are you entering {day} for? That's not a day you'll find in my calendar.");
                    }
					else if (day > 25)
                    {
						Console.WriteLine($"\nDay {day} is not a possible puzzle day. Valid puzzle days are from December 1st (puzzle day 1) until (and including) December 25th (puzzle day 25).");
                    }
					else if (year == DateTime.Today.Year && day > DateTime.Today.Day)
                    {
						var dayDiff = day - DateTime.Today.Day;

						Console.WriteLine($"\nGetting futuristic, now? You'll need to wait {dayDiff} {(dayDiff == 1 ? "day" : "days")} for that puzzle to even be published.");
                    }
					else
                    {
						Console.WriteLine($"\nDay {day} is not available for year {year}.");
					}
				}
				else
                {
					Console.WriteLine($"\nTrying to fool me? Give me a number next time, please.");
                }
			}
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
