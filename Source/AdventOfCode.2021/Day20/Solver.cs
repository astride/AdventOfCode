using AdventOfCode.Common;
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
			Part2Solution = SolvePart2(basicImage, enhancementAlgorithm).ToString();
		}

		private static int SolvePart1(List<string> imageInput, string algorithm)
		{
			var lightPixelCount = imageInput.GetFinalLightPixelCount(algorithm, 2);

			return lightPixelCount;
		}

		private static int SolvePart2(List<string> imageInput, string algorithm)
		{
			var lightPixelCount = imageInput.GetFinalLightPixelCount(algorithm, 50);

			return lightPixelCount;
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

		public static int GetFinalLightPixelCount(this List<string> imageInput, string algorithm, int enhancements)
		{
			var image = imageInput.CreateImage(enhancements);

			foreach (var enhancementCount in Enumerable.Range(1, enhancements))
			{
				image.Enhance(algorithm, enhancements - enhancementCount);
			}

			return image.LightPixelCount();
		}

		private static char[,] CreateImage(this List<string> input, int padding)
		{
			var inputWidth = input[0].Length;
			var inputHeight = input.Count;

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

		private static void Enhance(this char[,] image, string enhancementAlgorithm, int remainingEnhancements)
		{
			var width = image.GetLength(0);
			var height = image.GetLength(1);

			var source = new char[width, height];

			Array.Copy(image, source, width * height);

			int value;
			int xLeftOf;
			int xRightOf;
			int yAbove;
			int yBelow;

			foreach (var x in Enumerable.Range(0, width))
			{
				xLeftOf = Math.Max(x - 1, 0);
				xRightOf = Math.Min(x + 1, width - 1);

				foreach (var y in Enumerable.Range(0, height))
				{
					yAbove = Math.Max(y - 1, 0);
					yBelow = Math.Min(y + 1, height - 1);

					value =
						ValueOfPos8[source[xLeftOf, yAbove]] +
						ValueOfPos7[source[x, yAbove]] +
						ValueOfPos6[source[xRightOf, yAbove]] +
						ValueOfPos5[source[xLeftOf,	y]] +
						ValueOfPos4[source[x,		y]] +
						ValueOfPos3[source[xRightOf,	y]] +
						ValueOfPos2[source[xLeftOf, yBelow]] +
						ValueOfPos1[source[x, yBelow]] +
						ValueOfPos0[source[xRightOf, yBelow]];

					image[x, y] = enhancementAlgorithm[value];
				}
			}
		}

		private static int LightPixelCount(this char[,] image)
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
