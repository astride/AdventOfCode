using System.Linq;

namespace AdventOfCode.Common.Helpers
{
	public static class Helpers
	{
		public static string Without(this string input, string substring)
		{
			var output = input;

			while (output.Contains(substring))
			{
				if (output == substring) return string.Empty;

				var startIndex = output.IndexOf(substring);

				output = output.Substring(0, startIndex) + output.Substring(startIndex + substring.Length);
			}

			return output;
		}
	}
}
