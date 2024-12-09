using Common.Interfaces;

namespace Year2024;

public class Day09Solver : IPuzzleSolver
{
	public string Title => "Disk Fragmenter";

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var fileBlockCount = new List<int>();
		var freeSpaceBlockCount = new List<int>();

		var diskMap = input.Single();

		for (var i = 0; i < diskMap.Length; i++)
		{
			var blockCount = int.Parse(diskMap[i].ToString());

			if (i % 2 == 0)
			{
				fileBlockCount.Add(blockCount);
			}
			else
			{
				freeSpaceBlockCount.Add(blockCount);
			}
		}

		if (isExampleInput)
		{
			Console.WriteLine("File block count:");
			foreach (var count in fileBlockCount)
			{
				Console.Write(count);
				Console.Write(" ");
			}

			Console.WriteLine();
			Console.WriteLine("Free space block count:");
			foreach (var count in freeSpaceBlockCount)
			{
				Console.Write(count);
				Console.Write(" ");
			}
		}

		var compactedFiles = new List<int>();

		var fileBlockId = 0;
		var lastFileBlockId = fileBlockCount.Count - 1;

		while (fileBlockCount.Count > 0)
		{
			var remainingFileCountInCurrentBlock = fileBlockCount[0];

			while (remainingFileCountInCurrentBlock > 0)
			{
				compactedFiles.Add(fileBlockId);

				remainingFileCountInCurrentBlock--;
			}
			
			fileBlockCount.RemoveAt(0);
			fileBlockId++;

			if (freeSpaceBlockCount.Count is 0)
			{
				continue;
			}

			var remainingFreeSpaceCountInCurrentBlock = freeSpaceBlockCount[0];

			while (remainingFreeSpaceCountInCurrentBlock > 0)
			{
				if (fileBlockCount.Count is 0)
				{
					break;
				}

				while (fileBlockCount[^1] is 0)
				{
					fileBlockCount.RemoveAt(fileBlockCount.Count - 1);
					lastFileBlockId--;
				}
				
				compactedFiles.Add(lastFileBlockId);
				fileBlockCount[^1]--;

				remainingFreeSpaceCountInCurrentBlock--;
			}

			freeSpaceBlockCount.RemoveAt(0);
		}

		Console.WriteLine();
		Console.WriteLine("Compacted files:");
		foreach (var digit in compactedFiles)
		{
			Console.Write(digit);
		}
		Console.WriteLine();
		
		var checksum = 0L;
		
		for (var i = 0; i < compactedFiles.Count; i++)
		{
			checksum += i * compactedFiles[i];
		}

		return checksum;
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		return 0;
	}
}