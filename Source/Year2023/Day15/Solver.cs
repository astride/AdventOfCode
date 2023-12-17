using Common.Helpers;
using Common.Interfaces;

namespace Year2023;

public class Day15Solver : IPuzzleSolver
{
	public string Title => "Lens Library";

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var resultByStepType = new Dictionary<string, int>();

		var resultAfterMultiplicationAndDivisionByValue = new Dictionary<int, int>();

		var resultSum = 0L;

		foreach (var step in input.Single().Split(','))
		{
			if (resultByStepType.TryGetValue(step, out var stepTypeResult))
			{
				resultSum += stepTypeResult;
				continue;
			}

			var currentValue = 0;

			foreach (var instruction in step)
			{
				currentValue += instruction;

				if (resultAfterMultiplicationAndDivisionByValue.TryGetValue(currentValue, out var result))
				{
					currentValue = result;
				}
				else
				{
					var resultAfterMultiplication = currentValue * 17;
					var resultAfterDivision = resultAfterMultiplication % 256;

					resultAfterMultiplicationAndDivisionByValue[currentValue] = resultAfterDivision;

					currentValue = resultAfterDivision;
				}
			}

			resultByStepType[step] = currentValue;

			resultSum += currentValue;
		}

		return resultSum;
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		var boxNumberByLabel = new Dictionary<string, int>();
		var resultAfterMultiplicationAndDivisionByValue = new Dictionary<int, int>();

		var boxes = new List<Box>();
		
		foreach (var step in input.Single().Split(','))
		{
			var label = RegexHelper.GetFirstWord(step);

			if (!boxNumberByLabel.TryGetValue(label, out var boxNumber))
			{
				foreach (var ch in label)
				{
					boxNumber += ch;

					if (!resultAfterMultiplicationAndDivisionByValue.TryGetValue(boxNumber, out var resultingBoxNumber))
					{
						var resultAfterMultiplication = boxNumber * 17;
						var resultAfterDivision = resultAfterMultiplication % 256;

						resultAfterMultiplicationAndDivisionByValue[boxNumber] = resultAfterDivision;

						resultingBoxNumber = resultAfterDivision;
					}

					boxNumber = resultingBoxNumber;
				}

				boxNumberByLabel[label] = boxNumber;
			}

			var box = boxes.SingleOrDefault(box => box.Number == boxNumber);
			var existingLens = box?.Lenses.SingleOrDefault(lens => lens.Label == label);

			var replaceLensIfExists = step.Contains('=');

			if (replaceLensIfExists)
			{
				var focalLength = RegexHelper.GetFirstNumberAsInt(step);

				if (existingLens != default)
				{
					existingLens.FocalLength = focalLength;
					continue;
				}

				var newLens = new Lens(label, null, focalLength);

				if (box == default)
				{
					boxes.Add(new Box(boxNumber, newLens));
				}
				else if (existingLens == default)
				{
					box.Lenses.Add(newLens);
				}
			}
			else
			{
				if (existingLens == default)
				{
					continue;
				}

				var succeedingLens = box.Lenses.SingleOrDefault(lens => lens.PrecedingLabel == label);

				if (succeedingLens != default)
				{
					succeedingLens.PrecedingLabel = existingLens.PrecedingLabel;
				}

				box.Lenses.Remove(existingLens);
			}
		}

		var focusingPowerTotal = 0;

		foreach (var box in boxes)
		{
			for (var iLens = 0; iLens < box.Lenses.Count; iLens++)
			{
				focusingPowerTotal += (1 + box.Number) * (1 + iLens) * box.Lenses[iLens].FocalLength;
			}
		}

		return focusingPowerTotal;
	}

	private class Lens
	{
		public Lens(string label, string? precedingLabel, int focalLength)
		{
			Label = label;
			PrecedingLabel = precedingLabel;
			FocalLength = focalLength;
		}

		public string Label { get; }
		public string? PrecedingLabel { get; set; }
		public int FocalLength { get; set; }
	}

	private class Box
	{
		public Box(int boxNumber, Lens? firstLens = null)
		{
			Number = boxNumber;
			Lenses = new List<Lens>();

			if (firstLens != null)
			{
				Lenses.Add(firstLens);
			}
		}
		public int Number { get; }
		public IList<Lens> Lenses { get; }
	}
}