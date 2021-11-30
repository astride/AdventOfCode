namespace AdventOfCode.Common
{
	public interface IPuzzleSolver<T>
	{
		T Part1Solution { get; set; }

		T Part2Solution { get; set; }
		
		void SolvePuzzle(string[] input);
	}
}
