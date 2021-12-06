using System.Linq;

namespace AdventOfCode.Common.Helpers
{
	public static class Helpers
	{
		public static string ExceptString(this string input, string substring)
		{
			return string.Join("", input.Except(substring));
		}
	}
}
