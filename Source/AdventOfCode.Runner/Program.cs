using AdventOfCode.Common;
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
				Console.WriteLine($"Vil du løse dagens oppgave? Idag er det {today.ToString("ddd dd.MM.yyyy")} (svar blankt for 'ja'): ");

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

		static IEnumerable<PuzzleInfo> GetAvailablePuzzles()
		{
			var puzzleSolvers = GetPuzzleSolvers();
			var puzzleInputs = GetPuzzleInputFilePaths();

			var puzzleDates = puzzleSolvers
				.Select(solver => solver.PuzzleDate)
				.Intersect(puzzleInputs
					.Select(input => input.PuzzleDate))
				.Distinct();

			puzzleInputs = puzzleInputs.Where(input => puzzleDates.Contains(input.PuzzleDate));

			var puzzles = puzzleInputs
				.Select(input => new PuzzleInfo(
					input.PuzzleDate,
					puzzleSolvers.First(solver => solver.PuzzleDate == input.PuzzleDate).SolverType,
					input.RelativeInputFilePath,
					input.RelativeInputExampleFilePath));

			return puzzles;
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

			//TODO if only one; tell user and return year

			while (true)
			{
				Console.WriteLine($"Hvilket år vil du bryne deg på? Du kan velge mellom {string.Join(", ", availableYears)}: ");

				if (int.TryParse(Console.ReadLine(), out var year))
				{
					//TODO if year not in availableYears: tell user, don't return

					return year;
				}
			}
		}

		static int RequestDay(int year, IEnumerable<DateTime> availableDates)
		{
			var availableDays = availableDates
				.Where(date => date.Year == year)
				.Select(date => date.Day);

			//TODO if only one: tell user and return day

			while (true)
			{
				Console.WriteLine($"Hvilken dag i {year} vil du løse oppgaven for? Du kan velge mellom {string.Join(", ", availableDays)}: ");

				if (int.TryParse(Console.ReadLine(), out var day))
				{
					//TODO if day not in availableDays: tell user, don't return

					return day;
				}
			}
		}

		static bool RequestTestMode()
		{
			Console.WriteLine("Testmodus? (svar blankt for 'ja'): ");
			
			return string.IsNullOrEmpty(Console.ReadLine());
		}

		static void RequestModeAndSolve(PuzzleInfo puzzle)
		{
			var testMode = RequestTestMode();

			var solver = (IPuzzleSolver)Activator.CreateInstance(puzzle.SolverType);
			
			var inputFilePath = testMode ? puzzle.RelativeInputExampleFilePath : puzzle.RelativeInputFilePath;
			var input = GetContent(inputFilePath);

			solver.SolvePuzzle(input);

			Console.WriteLine($"\nDa setter vi i gang! Oppgavedagen er {puzzle.Date.ToString("dd. MMM yyyy")}, og vi løser den med {(testMode ? "testdata" : "reelle data")}.\n");
			Console.WriteLine("--- Løsninger ---");
			Console.WriteLine($"{nameof(solver.Part1Solution)}: {solver.Part1Solution}");
			Console.WriteLine($"{nameof(solver.Part2Solution)}: {solver.Part2Solution}");
			Console.WriteLine("-----------------\n");
		}

		static string[] GetContent(string relativeFilePath)
		{
			var filePath = Path.Combine(GetPuzzleRootLocation(), relativeFilePath);

			if (!File.Exists(filePath))
			{
				throw new Exception($"No input file found when searching for {filePath}");
			}

			return File.ReadAllLines(filePath, Encoding.Default);
		}

		private static IEnumerable<(DateTime PuzzleDate, string RelativeInputFilePath, string RelativeInputExampleFilePath)> GetPuzzleInputFilePaths()
		{
			var puzzleRootLocation = GetPuzzleRootLocation();

			var inputFilePaths = Directory.GetFiles(puzzleRootLocation, InputWildcardFileName, SearchOption.AllDirectories);

			var pathsByYear = inputFilePaths
				.Select(path => path
					.Substring((puzzleRootLocation + Path.DirectorySeparatorChar).Length))
				.GroupBy(path => int.Parse(path
					.Split(Path.DirectorySeparatorChar)
					.First(dir => dir.Contains(AdventOfCode))
					.Split('.')[1]));

			foreach (var yearAndPaths in pathsByYear)
			{
				foreach (var dayAndPaths in yearAndPaths
					.GroupBy(yap => yap.Split(Path.DirectorySeparatorChar)[1]))
				{
					if (dayAndPaths.Count(path => path.Split(Path.DirectorySeparatorChar).Last() == InputFileName) == 1 &&
						dayAndPaths.Count(path => path.Split(Path.DirectorySeparatorChar).Last() == InputExampleFileName) == 1)
					{
						yield return (
							new DateTime(yearAndPaths.Key, December, int.Parse(dayAndPaths.Key.Substring(Day.Length))),
							dayAndPaths.Single(path => path.Contains(InputFileName)),
							dayAndPaths.Single(path => path.Contains(InputExampleFileName)));
					}
				}
			}
		}

		private static IEnumerable<(DateTime PuzzleDate, Type SolverType)> GetPuzzleSolvers()
		{
			var types = _yearAndAssembly
				.Select(yaa =>
					new KeyValuePair<int, IEnumerable<Type>>(
						yaa.Key, //Fungerer
						yaa.Value.GetTypes() //Fungerer ikke
							.Where(type => IsPuzzleSolver(type))));

			var solvers = types
				.SelectMany(yearAndTypes => yearAndTypes.Value
					.Select(type => (GetPuzzleDateFor(yearAndTypes.Key, type), type)));

			return solvers;
		}

		private static bool IsPuzzleSolver(Type type)
		{
			if (!type.Name.Contains(Day) ||
				!type.Name.Contains(Solver)) return false;

			var day = string.Join("", type.Name.Except(Day).Except(Solver));

			if (day.Length != 2 ||
				!int.TryParse(day, out _))
			{
				return false;
			}

			return true;
		}

		private static DateTime GetPuzzleDateFor(int year, Type solverType)
		{
			var dayString = string.Join("", solverType.Name.Except(Day).Except(Solver));

			if (int.TryParse(dayString, out int day))
			{
				return new DateTime(year, December, day);
			}

			return default;
		}
	}

	public class PuzzleInfo
	{
		public PuzzleInfo(DateTime date, Type solverType, string relativeInputFilePath, string relativeInputExampleFilePath)
		{
			Date = date;
			SolverType = solverType;
			RelativeInputFilePath = relativeInputFilePath;
			RelativeInputExampleFilePath = relativeInputExampleFilePath;
		}

		public DateTime Date { get; set; }

		public Type SolverType { get; set; }

		public string RelativeInputFilePath { get; set; }

		public string RelativeInputExampleFilePath {get; set; }
	}
}
