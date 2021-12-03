using AdventOfCode.Common;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AdventOfCode.Runner
{
	// https://adventofcode.com/
	class Program
	{
		private const int RequestLimit = 10;

		static void Main(string[] args)
		{
			//TODO Ask if wants today's puzzle first
			var year = RequestYear();
			var day = RequestDay();

			if (PuzzleSolverExistsFor(year, day, out Type solverType))
			{
				var testMode = RequestTestMode();

				var input = GetInputFor(year, day, testMode);
				//TODO valiate input file exists and has content

				var solver = (IPuzzleSolver)Activator.CreateInstance(solverType);

				solver.SolvePuzzle(input);

				Console.WriteLine($"{nameof(solver.Part1Solution)}: {solver.Part1Solution}");
				Console.WriteLine($"{nameof(solver.Part2Solution)}: {solver.Part2Solution}");
			}

			//TODO Inform that doesn't exist and ask for input again
		}

		static int RequestYear()
		{
			string input;
			int fallbackValue = DateTime.Today.Year;

			foreach (var i in Enumerable.Range(0, RequestLimit))
			{
				Console.WriteLine(i == 0 ? "Year? (leave blank for this year)" : "Year needs to contain exactly four digits. Try again!");
				input = Console.ReadLine();

				if (string.IsNullOrEmpty(input))
				{
					return fallbackValue;
				}
				else if (input.Length == 4
					&& int.TryParse(input, out var value))
				{
					return int.Parse(input);
				}
			}

			Console.WriteLine($"This is taking too long. Let's go for this year ({fallbackValue})");
			return fallbackValue;
		}

		static int RequestDay()
		{
			string input;
			int fallbackValue = DateTime.Today.Day;

			foreach (var i in Enumerable.Range(0, RequestLimit))
			{
				Console.WriteLine(i == 0 ? "Day? (leave blank for this day)" : "Day needs to be a digit between 1 and 25. Try again!");
				input = Console.ReadLine();

				if (string.IsNullOrEmpty(input))
				{
					return fallbackValue;
				}
				else if (int.TryParse(input, out var value)
					&& value >= 1
					&& value <= 25)
				{
					return int.Parse(input);
				}
			}

			Console.WriteLine($"This is taking too long. Let's go for this day ({fallbackValue})");
			return fallbackValue;
		}

		static bool RequestTestMode()
		{
			Console.WriteLine("Test mode? (leave blank for 'yes')");
			var input = Console.ReadLine();

			return string.IsNullOrEmpty(input);
		}

		static string[] GetInputFor(int year, int day, bool testMode)
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

			var filePath = Path.Combine(puzzleRootLocation, DirectoryNameForYear(year), DirectoryNameForDay(day), InputFileName(day, testMode));

			if (!File.Exists(filePath))
			{
				throw new Exception($"No input file found when searching for {filePath}");
			}

			return File.ReadAllLines(filePath, Encoding.Default);
		}



		private static bool PuzzleSolverExistsFor(int year, int day, out Type solverType)
		{
			var yearAssembly = year switch
			{
				2021 => typeof(Y2021.Day01).Assembly,
				2020 => typeof(Y2020.Day01).Assembly,
				_ => null
			};

			if (yearAssembly == null)
			{
				//TODO Inform: no solver exists for requested puzzle year
				solverType = null;
				return false;
			}

			solverType = yearAssembly
				.GetTypes()
				.FirstOrDefault(type => type.Name == DirectoryNameForDay(day));

			return solverType is Type;
		}

		private static string InputFileName(int day, bool testMode) => testMode
			? $"{InputFileNameBaseForDay(day)}Example.txt"
			: $"{InputFileNameBaseForDay(day)}.txt";

		private static string InputFileNameBaseForDay(int day) => $"{DirectoryNameForDay(day)}Input";

		private static string DirectoryNameForYear(int year) => $"AdventOfCode.{year}";

		private static string DirectoryNameForDay(int day) => $"Day{day:D2}";
	}
}
