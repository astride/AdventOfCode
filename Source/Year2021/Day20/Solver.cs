using Common.Interfaces;

namespace Year2021;

public class Day20Solver : IPuzzleSolver
{
	public string Title => "Trench Map";
	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var algorithm = GetEnhancementAlgorithm(input);
		var imageInput = GetBasicImage(input);
		
		var lightPixelCount = imageInput.GetFinalLightPixelCount(algorithm, 2);

		return lightPixelCount;
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		var algorithm = GetEnhancementAlgorithm(input);
		var imageInput = GetBasicImage(input);
		
		var lightPixelCount = imageInput.GetFinalLightPixelCount(algorithm, 50);

		return lightPixelCount;
	}

	private static string GetEnhancementAlgorithm(string[] input)
	{
		return input.First();
	}

	private static List<string> GetBasicImage(string[] input)
	{
		return input
			.Skip(1)
			.Where(entry => !string.IsNullOrWhiteSpace(entry))
			.ToList();
	}
}

internal static class Day20Helpers
{
	private const char DarkPixel = '.';
	private const char LightPixel = '#';

	private static readonly Dictionary<char, int> DecimalValueAtPos0 = new() { [DarkPixel] = 0, [LightPixel] = 1 };
	private static readonly Dictionary<char, int> DecimalValueAtPos1 = new() { [DarkPixel] = 0, [LightPixel] = 2 };
	private static readonly Dictionary<char, int> DecimalValueAtPos2 = new() { [DarkPixel] = 0, [LightPixel] = 4 };
	private static readonly Dictionary<char, int> DecimalValueAtPos3 = new() { [DarkPixel] = 0, [LightPixel] = 8 };
	private static readonly Dictionary<char, int> DecimalValueAtPos4 = new() { [DarkPixel] = 0, [LightPixel] = 16 };
	private static readonly Dictionary<char, int> DecimalValueAtPos5 = new() { [DarkPixel] = 0, [LightPixel] = 32 };
	private static readonly Dictionary<char, int> DecimalValueAtPos6 = new() { [DarkPixel] = 0, [LightPixel] = 64 };
	private static readonly Dictionary<char, int> DecimalValueAtPos7 = new() { [DarkPixel] = 0, [LightPixel] = 128 };
	private static readonly Dictionary<char, int> DecimalValueAtPos8 = new() { [DarkPixel] = 0, [LightPixel] = 256 };

	public static int GetFinalLightPixelCount(this List<string> imageInput, string algorithm, int enhancements)
	{
		var image = imageInput.CreateImage(enhancements);

		foreach (var _ in Enumerable.Range(1, enhancements))
		{
			image.Enhance(algorithm);
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

	private static void Enhance(this char[,] image, string enhancementAlgorithm)
	{
		var width = image.GetLength(0);
		var height = image.GetLength(1);

		var source = new char[width, height];

		Array.Copy(image, source, width * height);

		foreach (var x in Enumerable.Range(0, width))
		{
			var xLeftOf = Math.Max(x - 1, 0);
			var xRightOf = Math.Min(x + 1, width - 1);

			foreach (var y in Enumerable.Range(0, height))
			{
				var yAbove = Math.Max(y - 1, 0);
				var yBelow = Math.Min(y + 1, height - 1);

				var value =
					DecimalValueAtPos8[source[xLeftOf, yAbove]] +
					DecimalValueAtPos7[source[x, yAbove]] +
					DecimalValueAtPos6[source[xRightOf, yAbove]] +
					DecimalValueAtPos5[source[xLeftOf,	y]] +
					DecimalValueAtPos4[source[x,		y]] +
					DecimalValueAtPos3[source[xRightOf,	y]] +
					DecimalValueAtPos2[source[xLeftOf, yBelow]] +
					DecimalValueAtPos1[source[x, yBelow]] +
					DecimalValueAtPos0[source[xRightOf, yBelow]];

				image[x, y] = enhancementAlgorithm[value];
			}
		}
	}

	private static int LightPixelCount(this char[,] image)
	{
		return image
			.Cast<char>()
			.Count(pixel => pixel == LightPixel);
	}
}
