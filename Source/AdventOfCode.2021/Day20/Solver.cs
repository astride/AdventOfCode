using AdventOfCode.Common;
using AdventOfCode.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021
{
	public class Day20Solver : IPuzzleSolver
	{
		public string Part1Solution { get; set; }
		public string Part2Solution { get; set; }

		public void SolvePuzzle(string[] rawInput)
		{
			var enhancementAlgorithm = rawInput.First();

			var basicImage = rawInput
				.Skip(1)
				.Where(entry => !string.IsNullOrWhiteSpace(entry))
				.ToList();

			Part1Solution = SolvePart1(basicImage, enhancementAlgorithm).ToString();
		}

		private static int SolvePart1(List<string> imageInput, string algorithm)
		{
			int enhancements = 2;

			var image = imageInput.CreateImage(enhancements);

			image.Print();

			foreach (var enhancementCount in Enumerable.Range(1, enhancements))
			{
				image.Enhance(algorithm, enhancements - enhancementCount);

				Console.WriteLine($"\nAfter enhancement:\n");
				image.Print();
			}

			return image.LightPixelCount();
		}
	}

	public static class Day20Helpers
	{
		private static char DarkPixel = '.';
		private static char LightPixel = '#';

		private static IDictionary<char, int> ValueOfPos0 =
			new Dictionary<char, int> { [DarkPixel] = 0, [LightPixel] = 1 };
		private static IDictionary<char, int> ValueOfPos1 =
			new Dictionary<char, int> { [DarkPixel] = 0, [LightPixel] = 2 };
		private static IDictionary<char, int> ValueOfPos2 =
			new Dictionary<char, int> { [DarkPixel] = 0, [LightPixel] = 4 };
		private static IDictionary<char, int> ValueOfPos3 =
			new Dictionary<char, int> { [DarkPixel] = 0, [LightPixel] = 8 };
		private static IDictionary<char, int> ValueOfPos4 =
			new Dictionary<char, int> { [DarkPixel] = 0, [LightPixel] = 16 };
		private static IDictionary<char, int> ValueOfPos5 =
			new Dictionary<char, int> { [DarkPixel] = 0, [LightPixel] = 32 };
		private static IDictionary<char, int> ValueOfPos6 =
			new Dictionary<char, int> { [DarkPixel] = 0, [LightPixel] = 64 };
		private static IDictionary<char, int> ValueOfPos7 =
			new Dictionary<char, int> { [DarkPixel] = 0, [LightPixel] = 128 };
		private static IDictionary<char, int> ValueOfPos8 =
			new Dictionary<char, int> { [DarkPixel] = 0, [LightPixel] = 256 };

		public static char[,] CreateImage(this List<string> input, int extraLayers)
		{
			var inputWidth = input[0].Length;
			var inputHeight = input.Count;

			var padding = 1 + extraLayers;
			var expansion = 2 * padding;

			var image = CreateDarkImage(inputWidth + expansion, inputHeight + expansion);

			foreach (var x in Enumerable.Range(0, inputWidth))
			{
				foreach (var y in Enumerable.Range(0, inputHeight))
				{
					image[x + padding, y + padding] = input[y][x];
				}
			}

			return image;
		}

		private static char[,] CreateDarkImage(int width, int height)
		{
			var image = new char[width, height];

			foreach (var i in Enumerable.Range(0, image.GetLength(0)))
			{
				foreach (var j in Enumerable.Range(0, image.GetLength(1)))
				{
					image[i, j] = DarkPixel;
				}
			}

			return image;
		}

		public static void Enhance(this char[,] image, string enhancementAlgorithm, int remainingEnhancements)
		{
			var width = image.GetLength(0);
			var height = image.GetLength(1);

			var source = new char[width, height];

			Array.Copy(image, source, width * height);

			int value;

			foreach (var x in Enumerable.Range(1 + remainingEnhancements, width - (2 * (1 + remainingEnhancements))))
			{
				foreach (var y in Enumerable.Range(1 + remainingEnhancements, height - (2 * (1 + remainingEnhancements))))
				{
					value =
						ValueOfPos8[source[x - 1,	y - 1]] +
						ValueOfPos7[source[x,		y - 1]] +
						ValueOfPos6[source[x + 1,	y - 1]] +
						ValueOfPos5[source[x - 1,	y]] +
						ValueOfPos4[source[x,		y]] +
						ValueOfPos3[source[x + 1,	y]] +
						ValueOfPos2[source[x - 1,	y + 1]] +
						ValueOfPos1[source[x,		y + 1]] +
						ValueOfPos0[source[x + 1,	y + 1]];

					image[x, y] = enhancementAlgorithm[value];
				}
			}
		}

		public static int LightPixelCount(this char[,] image)
		{
			var count = 0;

			foreach (var pixel in image)
			{
				if (pixel == LightPixel)
				{
					count++;
				}
			}

			return count;
		}
	}
}
