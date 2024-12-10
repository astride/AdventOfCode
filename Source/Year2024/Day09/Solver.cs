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

		var checksum = compactedFiles.Select((id, index) => 1L * id * index).Sum();

		return checksum;
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		var fileCountInBlock = new List<int>();
		var freeSpaceCountInBlock = new List<int>();

		var diskMap = input.Single();

		for (var i = 0; i < diskMap.Length; i++)
		{
			var blockCount = int.Parse(diskMap[i].ToString());

			if (i % 2 == 0)
			{
				fileCountInBlock.Add(blockCount);
			}
			else
			{
				freeSpaceCountInBlock.Add(blockCount);
			}
		}

		var fileBlockIdToMove = fileCountInBlock.Count - 1;
		var firstBlockIdWithFreeSpace = freeSpaceCountInBlock.FindIndex(count => count > 0);

		var removedFileCountInBlock = fileCountInBlock.Select(_ => 0).ToList();
		var contentInFreeSpaceBlock = freeSpaceCountInBlock.Select(_ => new List<int>()).ToList();
		
		while (fileBlockIdToMove > firstBlockIdWithFreeSpace)
		{
			var fileBlockCount = fileCountInBlock[fileBlockIdToMove];

			var freeSpaceTargetId = freeSpaceCountInBlock.FindIndex(count => count >= fileBlockCount);

			// Move file blocks if possible, but only forwards
			if (freeSpaceTargetId > -1 && freeSpaceTargetId < fileBlockIdToMove)
			{
				var fileCount = fileCountInBlock[fileBlockIdToMove];

				fileCountInBlock[fileBlockIdToMove] = 0;
				removedFileCountInBlock[fileBlockIdToMove] = fileCount;
				contentInFreeSpaceBlock[freeSpaceTargetId].AddRange(Enumerable.Range(0, fileCount).Select(_ => fileBlockIdToMove));
				freeSpaceCountInBlock[freeSpaceTargetId] -= fileCount;
				
				if (freeSpaceTargetId == firstBlockIdWithFreeSpace)
				{
					firstBlockIdWithFreeSpace = freeSpaceCountInBlock.FindIndex(count => count > 0);
				}
			}

			fileBlockIdToMove--;
		}

		var compactedFiles = new List<int>();
		
		for (var i = 0; i < fileCountInBlock.Count; i++)
		{
			for (var _ = 0; _ < fileCountInBlock[i]; _++)
			{
				compactedFiles.Add(i);
			}

			for (var _ = 0; _ < removedFileCountInBlock[i]; _++)
			{
				compactedFiles.Add(0);
			}

			if (i >= freeSpaceCountInBlock.Count)
			{
				break;
			}
			
			compactedFiles.AddRange(contentInFreeSpaceBlock[i]);

			for (var _ = 0; _ < freeSpaceCountInBlock[i]; _++)
			{
				compactedFiles.Add(0);
			}
		}

		var checksum = compactedFiles.Select((id, index) => 1L * id * index).Sum();

		return checksum;
	}
}