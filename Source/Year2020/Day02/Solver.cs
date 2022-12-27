using Common.Interfaces;

namespace Year2020;

public class Day02Solver : IPuzzleSolver
{
    public string Title => "Password Philosophy";
    public object? Part1Solution { get; set; }
    public object? Part2Solution { get; set; }

    public object GetPart1Solution(string[] input)
    {
        var passwordPolicies = GetPasswordPolicies(input);
        
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

    public object GetPart2Solution(string[] input)
    {
        var passwordPolicies = GetPasswordPolicies(input);
        
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

    private IEnumerable<PasswordPolicy> GetPasswordPolicies(string[] input)
    {
        return input
            .Select(line => line.Split(':'))
            .Select(lineParts => new PasswordPolicy(
                lineParts[1],
                lineParts[0].AsPolicy()))
            .ToList();
    }
}

internal record PasswordPolicy(
    string Password,
    Policy Policy);

internal record Policy(
    char RequiredCharacter,
    int FirstRequirement,
    int SecondRequirement);

internal static class Day02Helpers
{
    public static Policy AsPolicy(this string policyString)
    {
        var policyAndRequiredCharacter = policyString.Split(' ');
        
        var requirements = policyAndRequiredCharacter[0].Split('-');
        var requiredCharacter = char.Parse(policyAndRequiredCharacter[1]);

        return new Policy(
            requiredCharacter,
            int.Parse(requirements[0]),
            int.Parse(requirements[1]));
    }
}
