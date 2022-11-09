using Common.Interfaces;

namespace Year2020;

public class Day02Solver : IPuzzleSolver
{
    public string Title => "Password Philosophy";
    public string Part1Solution { get; set; } = string.Empty;
    public string Part2Solution { get; set; } = string.Empty;

    public void SolvePuzzle(string[] input)
    {
        var passwordPolicies = input
            .Select(line => line.Split(':'))
            .Select(lineParts => new PasswordPolicy(
                lineParts[1],
                lineParts[0].AsPolicy()))
            .ToList();

        Part1Solution = SolvePart1(passwordPolicies).ToString();
        Part2Solution = SolvePart2(passwordPolicies).ToString();
    }

    private static int SolvePart1(IEnumerable<PasswordPolicy> passwordPolicies)
    {
        var validPasswords = passwordPolicies
            .Select(pp => (
                RequiredCharacterCount: pp.Password.Count(ch => ch.Equals(pp.Policy.RequiredCharacter)),
                MinRequiredOccurrences: pp.Policy.FirstRequirement,
                MaxRequiredOccurrences: pp.Policy.SecondRequirement))
            .Count(passwordProperties =>
                passwordProperties.RequiredCharacterCount >= passwordProperties.MinRequiredOccurrences &&
                passwordProperties.RequiredCharacterCount <= passwordProperties.MaxRequiredOccurrences);
        
        return validPasswords;
    }

    private static int SolvePart2(IEnumerable<PasswordPolicy> passwordPolicies)
    {
        var validPasswords = passwordPolicies
            .Select(pp => (
                RequiredCharIsAtFirstSpecifiedIndex:
                    pp.Password[pp.Policy.FirstRequirement].Equals(pp.Policy.RequiredCharacter),
                RequiredCharIsAtSecondSpecifiedIndex:
                    pp.Password[pp.Policy.SecondRequirement].Equals(pp.Policy.RequiredCharacter)))
            .Count(passwordProperties =>
                passwordProperties.RequiredCharIsAtFirstSpecifiedIndex != passwordProperties.RequiredCharIsAtSecondSpecifiedIndex);

        return validPasswords;
    }

    private record PasswordPolicy(
        string Password,
        Policy Policy);

    public record Policy(
        char RequiredCharacter,
        int FirstRequirement,
        int SecondRequirement);
}

internal static class Helpers
{
    public static Day02Solver.Policy AsPolicy(this string policyString)
    {
        var policyAndRequiredCharacter = policyString.Split(' ');
        
        var requirements = policyAndRequiredCharacter[0].Split('-');
        var requiredCharacter = char.Parse(policyAndRequiredCharacter[1]);

        return new Day02Solver.Policy(
            requiredCharacter,
            int.Parse(requirements[0]),
            int.Parse(requirements[1]));
    }
}
