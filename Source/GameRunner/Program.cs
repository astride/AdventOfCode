using System.Reflection;
using System.Text;
using Common.Extensions;
using Common.Interfaces;

namespace GameRunner;

public class Program
{
	private const bool PlayGame = false;
	
	private static readonly IDictionary<int, Assembly> AssemblyByYear = new Dictionary<int, Assembly>
	{
		[2023] = typeof(Year2023.Day01Solver).Assembly,
		[2022] = typeof(Year2022.Day01Solver).Assembly,
		[2021] = typeof(Year2021.Day01Solver).Assembly,
		[2020] = typeof(Year2020.Day01Solver).Assembly,
	};

	private static IReadOnlyCollection<PuzzleSolverInfo>? AvailablePuzzles;
	private static IReadOnlyCollection<DateTime>? AvailablePuzzleDates;

	private static string? PuzzleRootLocation;

	private const int December = 12;
	private const string Day = "Day";
	private const string Year = "Year";

	private const string DateFormat = "dd. MMM yyyy";
	
	private const string Solver = "Solver";
	
	private const string InputWildcardFileName = "Input*.txt";
	private const string InputFileName = "Input.txt";
	private const string InputExampleFileName = "InputExample.txt";
	private const string InputExampleFileNamePart1 = "InputExamplePart1.txt";
	private const string InputExampleFileNamePart2 = "InputExamplePart2.txt";

	public static void Main(string[] args)
	{
		if (PlayGame)
		{
			ResolvePuzzleDateAndPlay();
		}
		else
		{
			IPuzzleSolver puzzleSolver = new Year2023.Day01Solver();
			var date = new DateTime(2023, December, 1);

			SolveFor(date, puzzleSolver);
		}
	}

	private static void SolveFor(DateTime puzzleDate, IPuzzleSolver puzzleSolver)
	{
		// Example input
		if (puzzleSolver.UsePartSpecificExampleInputFiles)
		{
			puzzleSolver.SolvePart1(GetInput(puzzleDate, InputExampleFileNamePart1), true);
			puzzleSolver.SolvePart2(GetInput(puzzleDate, InputExampleFileNamePart2), true);
		}
		else
		{
			puzzleSolver.SolvePuzzle(GetInput(puzzleDate, InputExampleFileName), true);
		}

		Console.WriteLine("\nSolutions for example input:");
		Console.WriteLine("Part 1: " + puzzleSolver.Part1Solution);
		Console.WriteLine("Part 2: " + puzzleSolver.Part2Solution);
		
		// Real input
		puzzleSolver.SolvePuzzle(GetInput(puzzleDate, InputFileName), false);
		
		Console.WriteLine("\nSolutions:");
		Console.WriteLine("Part 1: " + puzzleSolver.Part1Solution);
		Console.WriteLine("Part 2: " + puzzleSolver.Part2Solution);

		Console.ReadLine();
	}

	private static void ResolvePuzzleDateAndPlay()
    {
		var today = DateTime.Today;
		var yesterday = today.AddDays(-1);

		var availablePuzzleDates = GetAvailablePuzzleDates();

		if (!availablePuzzleDates.Any())
		{
			Console.WriteLine("No puzzles are available. Now, go do some coding, you lazy ladybug.");
			
			EndGame();
		}
		else if (availablePuzzleDates.Contains(today))
		{
			Console.WriteLine($"Do you want to solve today's puzzle? Today is {today.ToString(DateFormat)} (leave blank for 'yes'):");

			InterpretResponseFor(today);
		}
		else if (availablePuzzleDates.Contains(yesterday))
		{
			Console.WriteLine($"Today's puzzle is not available, but maybe you want to solve yesterday's puzzle? Yesterday was {yesterday.ToString(DateFormat)} (leave blank for 'yes'):");

			InterpretResponseFor(yesterday);
		}
		else
		{
			var latestAvailablePuzzleDate = availablePuzzleDates.Max();

			Console.WriteLine($"The latest available puzzle date is {latestAvailablePuzzleDate.ToString(DateFormat)}. Do you want to solve it? (leave blank for 'yes'):");

			InterpretResponseFor(latestAvailablePuzzleDate);
		}
	}

	private static string GetPuzzleRootLocation()
	{
		if (!string.IsNullOrEmpty(PuzzleRootLocation))
		{
			return PuzzleRootLocation;
		}
		
		var rootLocation = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.Parent?.FullName;

		if (string.IsNullOrEmpty(rootLocation))
		{
			throw new ArgumentNullException(nameof(rootLocation));
		}

		PuzzleRootLocation = rootLocation;

		return rootLocation;
	}

	private static IReadOnlyCollection<PuzzleSolverInfo> GetAvailablePuzzles()
	{
		if (AvailablePuzzles != null)
		{
			return AvailablePuzzles;
		}
		
		var puzzleSolvers = GetPuzzleSolvers();

		var availablePuzzleDates = puzzleSolvers
			.Select(solver => solver.Date)
			.Intersect(GetPuzzleInputDates())
			.Distinct();

		AvailablePuzzles = puzzleSolvers
			.Where(solver => availablePuzzleDates.Contains(solver.Date))
			.ToList();

		return AvailablePuzzles;
	}

	private static IReadOnlyCollection<DateTime> GetAvailablePuzzleDates()
	{
		if (AvailablePuzzleDates != null)
		{
			return AvailablePuzzleDates;
		}
		
		AvailablePuzzleDates = GetAvailablePuzzles()
			.Select(puzzle => puzzle.Date)
			.ToList();

		return AvailablePuzzleDates;
	}

	private static void InterpretResponseFor(DateTime suggestedDate)
	{
		var availablePuzzles = GetAvailablePuzzles();

		if (string.IsNullOrEmpty(Console.ReadLine()))
		{
			RequestModeAndSolve(availablePuzzles.Single(puzzle => puzzle.Date == suggestedDate));
		}
		else
		{
			RequestDate();
		}
	}

	private static void RequestModeAndSolve(PuzzleSolverInfo puzzleSolverInfo)
	{
		Console.WriteLine("\nUsing example data? (leave blank for 'yes')");
		var usingExampleData = string.IsNullOrEmpty(Console.ReadLine());

		var puzzleInstance = Activator.CreateInstance(puzzleSolverInfo.Type);

		if (puzzleInstance == null)
		{
			throw new ArgumentNullException(nameof(puzzleInstance));
		}

		var puzzleSolver = (IPuzzleSolver)puzzleInstance;

		if (usingExampleData && puzzleSolver.UsePartSpecificExampleInputFiles)
		{
			var inputPart1 = GetInput(puzzleSolverInfo.Date, InputExampleFileNamePart1);
			var inputPart2 = GetInput(puzzleSolverInfo.Date, InputExampleFileNamePart2);
				
			puzzleSolver.SolvePart1(inputPart1, true);
			puzzleSolver.SolvePart2(inputPart2, true);
		}
		else
		{
			var inputFileName = usingExampleData ? InputExampleFileName : InputFileName;

			var input = GetInput(puzzleSolverInfo.Date, inputFileName);
			
			puzzleSolver.SolvePuzzle(input, usingExampleData);
		}
		
		var additionalInfo = usingExampleData ? " (example data)" : string.Empty;

		Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n");
		Console.WriteLine($"Let's go! We are playing the {puzzleSolver.Title} puzzle from {puzzleSolverInfo.Date.ToString(DateFormat)}.\n");
		Console.WriteLine($"{nameof(puzzleSolver.Part1Solution)}{additionalInfo}: {puzzleSolver.Part1Solution}\n");
		Console.WriteLine($"{nameof(puzzleSolver.Part2Solution)}{additionalInfo}: {puzzleSolver.Part2Solution}\n");
		Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n");
		
		EndGame();
	}

	private static void EndGame()
	{
		Console.WriteLine("Press any key to exit the game.");
		Console.ReadLine();
	}

	private static string[] GetInput(DateTime date, string fileName)
	{
		var yearDir = $"{Year}{date.Year}";
		var dayDir = $"{Day}{date.Day:D2}";

		var filePath = Path.Combine(GetPuzzleRootLocation(), yearDir, dayDir, fileName);

		if (!File.Exists(filePath))
		{
			throw new Exception($"No input file found when searching for {filePath}");
		}

		return File.ReadAllLines(filePath, Encoding.Default);
	}

	private static void RequestDate()
	{
		var year = ResolveYear();
		var day = ResolveDay(year);

		var targetDate = new DateTime(year, December, day);

		var targetPuzzle = GetAvailablePuzzles()
			.Single(puzzle => puzzle.Date == targetDate);

		RequestModeAndSolve(targetPuzzle);
	}

	private static int ResolveYear()
	{
		var availableYears = GetAvailablePuzzleDates()
			.Select(date => date.Year)
			.Distinct()
			.ToList();

		if (availableYears.Count == 1)
		{
			var year = availableYears.Single();

			Console.WriteLine($"\nPuzzle solvers only exist for year {year}.");

			return year;
		}

		while (true)
		{
			Console.WriteLine($"\nFrom which year would you like to select a puzzle to solve? You can choose between {string.Join(", ", availableYears.OrderBy(_ => _))}:");

			var response = Console.ReadLine();

			if (int.TryParse(response, out var year))
			{
				if (availableYears.Contains(year))
                {
					return year;
                }

				if (year > DateTime.Today.Year ||
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

	private static int ResolveDay(int year)
	{
		var availableDays = GetAvailablePuzzleDates()
			.Where(date => date.Year == year)
			.Select(date => date.Day)
			.ToList();

		if (availableDays.Count == 1)
		{
			var day = availableDays.Single();

			Console.WriteLine($"\nThe only puzzle solver existing for {year} is for day {day}.\n");

			return day;
		}

		while (true)
		{
			Console.WriteLine($"\nFor which day in {year} would you like to solve the puzzle? Available days are {string.Join(", ", availableDays.OrderBy(_ => _))}:");

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
		var inputFilePaths = Directory.GetFiles(GetPuzzleRootLocation(), InputWildcardFileName, SearchOption.AllDirectories);

		var pathByYearCollection = inputFilePaths
			.Select(path => path.Without(GetPuzzleRootLocation() + Path.DirectorySeparatorChar))
			.GroupBy(path => int.Parse(path
				.Split(Path.DirectorySeparatorChar)
				.First(dirName => dirName.Contains(Year))
				.Without(Year)));

		foreach (var yearAndPath in pathByYearCollection)
		{
			var pathByDayCollection = yearAndPath
				.GroupBy(path => path.Split(Path.DirectorySeparatorChar)[1])
				.Where(entry => entry.Key.StartsWith(Day));

			foreach (var dayAndPath in pathByDayCollection)
			{
				var inputFileNames = dayAndPath
					.Select(path => path.Split(Path.DirectorySeparatorChar).Last())
					.ToList();
				
				var isMissingInputFile = inputFileNames.All(name => name != InputFileName);

				if (isMissingInputFile)
				{
					continue;
				}
				
				var isMissingExampleInputFile = inputFileNames.All(name => name != InputExampleFileName);
				var isMissingExampleInputPart1File = inputFileNames.All(name => name != InputExampleFileNamePart1);
				var isMissingExampleInputPart2File = inputFileNames.All(name => name != InputExampleFileNamePart2);

				if (isMissingExampleInputFile && 
				    (isMissingExampleInputPart1File || isMissingExampleInputPart2File))
				{
					continue;
				}
				
				var day = int.Parse(dayAndPath.Key.Without(Day));

				yield return (new DateTime(yearAndPath.Key, December, day));
			}
		}
	}

	private static IReadOnlyCollection<PuzzleSolverInfo> GetPuzzleSolvers()
	{
		var solvers = AssemblyByYear
			.SelectMany(yearAndAssembly => yearAndAssembly.Value
				.GetTypes()
				.Where(IsPuzzleSolver))
			.Select(type => new PuzzleSolverInfo(GetPuzzleDateFor(type), type))
			.ToList();

		return solvers;
	}

	private static bool IsPuzzleSolver(Type type)
	{
		if (!type.Name.StartsWith(Day) || !type.Name.EndsWith(Solver))
		{
			return false;
		}

		var day = type.Name.Without(Day).Without(Solver);

		var dayFormatIsValid = day.Length == 2 && int.TryParse(day, out _);

		return dayFormatIsValid;
	}

	private static DateTime GetPuzzleDateFor(Type solverType)
	{
		if (solverType == null)
		{
			throw new ArgumentNullException(nameof(solverType));
		}

		if (string.IsNullOrEmpty(solverType.FullName))
		{
			throw new ArgumentNullException(nameof(solverType) + "." + nameof(solverType.FullName));
		}
		
		var yearString = solverType.FullName
			.Split('.')
			.First(dirName => dirName.Contains(Year))
			.Without(Year);

		var dayString = solverType.Name
			.Without(Day)
			.Without(Solver);

		var temp1 = int.TryParse(dayString, out var day) && day is > 0 and <= 25;
		var temp2 = int.TryParse(yearString, out var year) && year >= DateTime.MinValue.Year;

		if (temp1 && temp2)
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

	public DateTime Date { get; }
	public Type Type { get; }
}
