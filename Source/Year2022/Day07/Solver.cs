using Common.Interfaces;

namespace Year2022;

public class Day07Solver : IPuzzleSolver
{
    public string Title => "";

    public string Part1Solution { get; set; } = string.Empty;
    public string Part2Solution { get; set; } = string.Empty;

    public void SolvePuzzle(string[] input)
    {
        Part1Solution = SolvePart1(input).ToString();
        //Part2Solution = SolvePart2(input).ToString();
    }

    private static double SolvePart1(IEnumerable<string> terminalOutput)
    {
        const string changeDirCommandPrefix = "$ cd";
        const string goToParentDirCommand = changeDirCommandPrefix + " ..";
        const string listContent = "$ ls";
        const string dirPrefix = "dir";
        
        const double maxSizeOfRelevantDirectories = 100000;

        var directoryStack = new Stack<string>();
        var totalFileSizeByDirName = new Dictionary<string, double>();

        string GetDirPath() => string.Join("/", directoryStack);
        
        bool IsFileInfo(string line) => int.TryParse(line[..1], out _);

        var terminalLinesOfInterest = terminalOutput
            .Where(line => !line.StartsWith(dirPrefix))
            .Where(line => line != listContent);

        foreach (var line in terminalLinesOfInterest)
        {
            Console.WriteLine();
            Console.WriteLine(line);

            if (line == goToParentDirCommand)
            {
                directoryStack.Pop();
                
                Console.WriteLine("Current dir: '" + GetDirPath() + "'");
                
                continue;
            }
            
            if (line.StartsWith(changeDirCommandPrefix))
            {
                var dirName = line.Split(' ')[2];

                if (dirName == "/")
                {
                    dirName = "root";
                }
                
                directoryStack.Push(dirName);
                
                Console.WriteLine("Change to dir: '" + GetDirPath() + "'");

                continue;
            }
            
            if (IsFileInfo(line))
            {
                var fileSize = double.Parse(line.Split(' ')[0]);
                
                Console.WriteLine("'" + GetDirPath() + "' contains a file of size " + fileSize);

                foreach (var dir in directoryStack)
                {
                    if (totalFileSizeByDirName.ContainsKey(dir))
                    {
                        var previousTotalFileSize = totalFileSizeByDirName[dir];
                        totalFileSizeByDirName[dir] += fileSize;
                        
                        Console.WriteLine("--> '" + dir + "' total file size: (" + previousTotalFileSize + " + " + fileSize + " = )" + totalFileSizeByDirName[dir]);
                    }
                    else
                    {
                        totalFileSizeByDirName[dir] = fileSize;
                        
                        Console.WriteLine("--> '" + dir + "' total file size: " + totalFileSizeByDirName[dir]);
                    }
                }

                continue;
            }
        }
        
        var totalFileSizesOfRelevantDirectories = totalFileSizeByDirName
            .Values
            .Where(val => val <= maxSizeOfRelevantDirectories);

        var totalRelevantDirectoryFileSize = totalFileSizesOfRelevantDirectories.Sum();
        
        // 924098: Too low
        return totalRelevantDirectoryFileSize;
    }

    private static int SolvePart2(IEnumerable<string> input)
    {
        return 0;
    }
}
