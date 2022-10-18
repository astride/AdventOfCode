namespace Common.Extensions;

public static class StringExtensions
{		public static string Without(this string input, string substring)
    {
        var output = input;

        while (output.Contains(substring))
        {
            if (output == substring) return string.Empty;

            var startIndex = output.IndexOf(substring, StringComparison.InvariantCulture);

            output = output.Substring(0, startIndex) + output.Substring(startIndex + substring.Length);
        }

        return output;
    }
}
