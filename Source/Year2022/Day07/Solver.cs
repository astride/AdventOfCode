using Common.Interfaces;

namespace Year2022;

public class Day07Solver : IPuzzleSolver
{
    public string Title => "No Space Left On Device";

    public string Part1Solution { get; set; } = string.Empty;
    public string Part2Solution { get; set; } = string.Empty;

    private const string RootDirSign = "/";
    private const string RootDirName = "root";

    public void SolvePuzzle(string[] input)
    {
        var totalFileSizeByDirNameDict = GetTotalFileSizeByDirNameDict(input);
        
        Part1Solution = SolvePart1(totalFileSizeByDirNameDict).ToString();
        Part2Solution = SolvePart2(totalFileSizeByDirNameDict).ToString();
    }

    private static double SolvePart1(Dictionary<string, double> totalFileSizeByDirName)
    {
        const double maxSizeOfRelevantDirectories = 100000;
        
        var totalFileSizesOfRelevantDirectories = totalFileSizeByDirName
            .Values
            .Where(val => val <= maxSizeOfRelevantDirectories);

        return totalFileSizesOfRelevantDirectories.Sum();
    }

    private static double SolvePart2(Dictionary<string, double> totalFileSizeByDirName)
    {
        const double availableDiskSpace = 70000000;
        const double neededUnusedDiskSpace = 30000000;
        
        var spaceNeededToBeDeleted = neededUnusedDiskSpace - (availableDiskSpace - totalFileSizeByDirName["/root"]);

        var temp = totalFileSizeByDirName
            .Values
            .Where(val => val > spaceNeededToBeDeleted)
            .Min();
        
        return temp;
    }

    private static Dictionary<string, double> GetTotalFileSizeByDirNameDict(IEnumerable<string> terminalOutput)
    {
        const string changeDirCommandPrefix = "$ cd";
        const string goToParentDirCommand = changeDirCommandPrefix + " ..";
        const string listContent = "$ ls";
        const string dirPrefix = "dir";

        var directoryStack = new Stack<string>();
        var totalFileSizeByDirName = new Dictionary<string, double>();

        bool IsFileInfo(string line) => int.TryParse(line[..1], out _);

        var terminalLinesOfInterest = terminalOutput
            .Where(line => !line.StartsWith(dirPrefix))
            .Where(line => line != listContent);

        foreach (var line in terminalLinesOfInterest)
        {
            if (line == goToParentDirCommand)
            {
                directoryStack.Pop();
                
                continue;
            }
            
            if (line.StartsWith(changeDirCommandPrefix))
            {
                var dirName = line.Split(' ')[2];

                if (dirName == RootDirSign)
                {
                    dirName = RootDirName;
                }
                
                directoryStack.Push(dirName);

                continue;
            }
            
            if (IsFileInfo(line))
            {
                var fileSize = double.Parse(line.Split(' ')[0]);

                var dirPath = string.Empty;

                foreach (var dir in directoryStack.Reverse())
                {
                    dirPath += "/" + dir;
                    
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
        }

        return totalFileSizeByDirName;
    }
}
