using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AdventOfCode.Common
{
	// https://adventofcode.com/
	class Program
	{
		private const int RequestLimit = 10;

		private const string InputDirectoryName = "Input";
		private readonly static string InputFileExtension = ".txt";

		static void Main(string[] args)
		{
			//TODO Ask if wants today's puzzle first
			var year = RequestYear();
			var day = RequestDay();
			var testMode = RequestTestMode();

			var input = GetInputFor(year, day, testMode);
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

			var filePath = Path.Combine(puzzleRootLocation, InputDirectoryName, DirectoryNameForYear(year), InputFileName(day, testMode));

			if (!File.Exists(filePath))
			{
				throw new Exception($"No input file found when searching for {filePath}");
			}

			return File.ReadAllLines(filePath, Encoding.Default);
		}

		private static string InputFileName(int day, bool testMode) => testMode
			? $"{DirectoryNameForDay(day)}Test{InputFileExtension}"
			: $"{DirectoryNameForDay(day)}{InputFileExtension}";

		private static string DirectoryNameForYear(int year) => year.ToString();

		private static string DirectoryNameForDay(int day) => $"Day{day:D2}";
	}
}
