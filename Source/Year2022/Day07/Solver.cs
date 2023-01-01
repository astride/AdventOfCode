using Common.Interfaces;

namespace Year2022;

public class Day07Solver : IPuzzleSolver
{
    public string Title => "No Space Left On Device";

    public object? Part1Solution { get; set; }
    public object? Part2Solution { get; set; }

    private const string RootDirSign = "/";
    private const string RootDirName = "root";

    public object GetPart1Solution(string[] input, bool isExampleInput)
    {
        var totalFileSizeByDirName = GetTotalFileSizeByDirNameDict(input);
        
        const double maxSizeOfRelevantDirectories = 100000;
        
        var totalFileSizesOfRelevantDirectories = totalFileSizeByDirName
            .Values
            .Where(val => val <= maxSizeOfRelevantDirectories);

        return totalFileSizesOfRelevantDirectories.Sum();
    }

    public object GetPart2Solution(string[] input, bool isExampleInput)
    {
        var totalFileSizeByDirName = GetTotalFileSizeByDirNameDict(input);
        
        const double totalDiskSpace = 70000000;
        const double neededUnusedDiskSpace = 30000000;

        var availableDiskSpace = totalDiskSpace - totalFileSizeByDirName[RootDirName];
        var spaceNeededToBeDeleted = neededUnusedDiskSpace - availableDiskSpace;

        var totalFileSizeOfDirectoryToDelete = totalFileSizeByDirName
            .Values
            .Where(val => val > spaceNeededToBeDeleted)
            .Min();
        
        return totalFileSizeOfDirectoryToDelete;
    }

    private static Dictionary<string, double> GetTotalFileSizeByDirNameDict(IEnumerable<string> terminalOutput)
    {
        const string changeDirCommandPrefix = "$ cd";
        const string goToParentDirCommand = changeDirCommandPrefix + " ..";
        const string listContent = "$ ls";
        const string dirPrefix = "dir";

        var dirBranch = new List<string>();
        var totalFileSizeByDirName = new Dictionary<string, double>();

        var terminalLinesOfInterest = terminalOutput
            .Where(line => !line.StartsWith(dirPrefix))
            .Where(line => line != listContent);

        foreach (var line in terminalLinesOfInterest)
        {
            if (line == goToParentDirCommand)
            {
                dirBranch.RemoveAt(dirBranch.Count - 1);
                
                continue;
            }
            
            if (line.StartsWith(changeDirCommandPrefix))
            {
                var dirName = line.Split(' ')[2];

                if (dirName == RootDirSign)
                {
                    dirName = RootDirName;
                }
                
                dirBranch.Add(dirName);

                continue;
            }
            
            // At this point, the only possible line type is a file info line; treat it as such
            
            var fileSize = double.Parse(line.Split(' ')[0]);

            var dirPath = string.Empty;

            foreach (var dirName in dirBranch)
            {
                if (dirPath.Length > 0)
                {
                    dirPath += "/";
                }
                
                dirPath += dirName;
                
                if (totalFileSizeByDirName.ContainsKey(dirPath))
                {
                    totalFileSizeByDirName[dirPath] += fileSize;
                }
                else
                {
                    totalFileSizeByDirName[dirPath] = fileSize;
                }
            }
        }

        return totalFileSizeByDirName;
    }
}
