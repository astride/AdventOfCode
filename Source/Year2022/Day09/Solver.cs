using Common.Interfaces;

namespace Year2022;

public class Day09Solver : IPuzzleSolver
{
    public string Title => "Rope Bridge";

    public bool UsePartSpecificExampleInputFiles => true;

    public object? Part1Solution { get; set; }
    public object? Part2Solution { get; set; }

    private const string Right = "R";
    private const string Left = "L";
    private const string Up = "U";
    private const string Down = "D";

    public object GetPart1Solution(string[] input, bool isExampleInput)
    {
        var head = (Row: 0, Col: 0);
        var tail = (Row: 0, Col: 0);

        bool HeadIsDirectlyToTheRight() => head.Row == tail.Row && head.Col == tail.Col + 1;
        bool HeadIsDirectlyToTheLeft() => head.Row == tail.Row && head.Col == tail.Col - 1;
        bool HeadIsDirectlyAbove() => head.Row == tail.Row + 1 && head.Col == tail.Col;
        bool HeadIsDirectlyBelow() => head.Row == tail.Row - 1 && head.Col == tail.Col;

        bool HeadIsDirectlyAboveToTheRight() => head.Row == tail.Row + 1 && head.Col == tail.Col + 1;
        bool HeadIsDirectlyAboveToTheLeft() => head.Row == tail.Row + 1 && head.Col == tail.Col - 1;
        bool HeadIsDirectlyBelowToTheRight() => head.Row == tail.Row - 1 && head.Col == tail.Col + 1;
        bool HeadIsDirectlyBelowToTheLeft() => head.Row == tail.Row - 1 && head.Col == tail.Col - 1;

        void MoveHead(string direction)
        {
            if (direction == Up) head.Row++;
            if (direction == Down) head.Row--;
            if (direction == Right) head.Col++;
            if (direction == Left) head.Col--;
        }

        void MoveTail(string direction)
        {
            if (direction == Up) tail.Row++;
            if (direction == Down) tail.Row--;
            if (direction == Right) tail.Col++;
            if (direction == Left) tail.Col--;
        }

        var tailVisitLocations = new HashSet<(int Row, int Col)> { tail };

        var instructions = input
            .Select(item => item.Split(' '))
            .Select(parts => (Direction: parts[0], Steps: int.Parse(parts[1])))
            .ToList();

        foreach (var line in instructions)
        {
            for (var i = 0; i < line.Steps; i++)
            {
                var directionsForTailToMove = line.Direction switch
                {
                    // Straight movement
                    Up when HeadIsDirectlyAbove() => new[] { Up },
                    Down when HeadIsDirectlyBelow() => new[] { Down },
                    Left when HeadIsDirectlyToTheLeft() => new[] { Left },
                    Right when HeadIsDirectlyToTheRight() => new[] { Right },
                    // Diagonal movement
                    Up or Left when HeadIsDirectlyAboveToTheLeft() => new[] { Up, Left },
                    Up or Right when HeadIsDirectlyAboveToTheRight() => new[] { Up, Right },
                    Down or Left when HeadIsDirectlyBelowToTheLeft() => new[] { Down, Left },
                    Down or Right when HeadIsDirectlyBelowToTheRight() => new[] { Down, Right },
                    // No movement (head and tail are overlapping)
                    _ => Array.Empty<string>(),
                };
                
                MoveHead(line.Direction);

                foreach (var direction in directionsForTailToMove)
                {
                    MoveTail(direction);
                }
                
                tailVisitLocations.Add(tail);
            }
        }
        
        return tailVisitLocations.Count;
    }
    
    public object GetPart2Solution(string[] input, bool isExampleInput)
    {
        const int knots = 10;
        
        var rope = Enumerable.Repeat((Row: 0, Col: 0), knots).ToList();

        var tailVisitLocations = new HashSet<(int Row, int Col)>
        {
            rope.Last()
        };

        bool AreAdjacent((int Row, int Col) firstKnot, (int Row, int Col) secondKnot)
        {
            var rowOffset = Math.Abs(firstKnot.Row - secondKnot.Row);
            var colOffset = Math.Abs(firstKnot.Col - secondKnot.Col);

            if (rowOffset > 1 || colOffset > 1)
            {
                return false;
            }

            return true;
        }

        (int Row, int Col) GetNewKnotLocation((int Row, int Col) currentKnotPosition, (int Row, int Col) currentKnotInFrontPosition)
        {
            var rowOffset = currentKnotInFrontPosition.Row - currentKnotPosition.Row;
            var colOffset = currentKnotInFrontPosition.Col - currentKnotPosition.Col;

            var knotInFrontIsMoreThanOneColumnAway = Math.Abs(colOffset) > 1;
            var knotInFrontIsMoreThanOneRowAway = Math.Abs(rowOffset) > 1;
            
            var needsToMoveUp = rowOffset > 1 || (rowOffset == 1 && knotInFrontIsMoreThanOneColumnAway);
            var needsToMoveDown = rowOffset < -1 || (rowOffset == -1 && knotInFrontIsMoreThanOneColumnAway);
            var needsToMoveRight = colOffset > 1 || (colOffset == 1 && knotInFrontIsMoreThanOneRowAway);
            var needsToMoveLeft = colOffset < -1 || (colOffset == -1 && knotInFrontIsMoreThanOneRowAway);

            if (needsToMoveUp && needsToMoveRight)
            {
                return (currentKnotPosition.Row + 1, currentKnotPosition.Col + 1);
            }
            if (needsToMoveUp && needsToMoveLeft)
            {
                return (currentKnotPosition.Row + 1, currentKnotPosition.Col - 1);
            }
            if (needsToMoveUp)
            {
                return (currentKnotPosition.Row + 1, currentKnotPosition.Col);
            }
            if (needsToMoveDown && needsToMoveRight)
            {
                return (currentKnotPosition.Row - 1, currentKnotPosition.Col + 1);
            }
            if (needsToMoveDown && needsToMoveLeft)
            {
                return (currentKnotPosition.Row - 1, currentKnotPosition.Col - 1);
            }
            if (needsToMoveDown)
            {
                return (currentKnotPosition.Row - 1, currentKnotPosition.Col);
            }
            if (needsToMoveRight)
            {
                return (currentKnotPosition.Row, currentKnotPosition.Col + 1);
            }
            if (needsToMoveLeft)
            {
                return (currentKnotPosition.Row, currentKnotPosition.Col - 1);
            }

            // Should never occur, but...
            return currentKnotPosition;
        }
        
        var instructions = input
            .Select(item => item.Split(' '))
            .Select(parts => (Direction: parts[0], Steps: int.Parse(parts[1])))
            .ToList();

        foreach (var line in instructions)
        {
            for (var step = 0; step < line.Steps; step++)
            {
                for (var knotIndex = 0; knotIndex < knots; knotIndex++)
                {
                    // If head: Move head --> continue
                    if (knotIndex == 0)
                    {
                        var currentHeadPosition = rope[knotIndex];
                        
                        if (line.Direction == Up)
                        {
                            rope[knotIndex] = (currentHeadPosition.Row + 1, currentHeadPosition.Col);
                        }
                        else if (line.Direction == Down)
                        {
                            rope[knotIndex] = (currentHeadPosition.Row - 1, currentHeadPosition.Col);
                        }
                        else if (line.Direction == Right)
                        {
                            rope[knotIndex] = (currentHeadPosition.Row, currentHeadPosition.Col + 1);
                        }
                        else if (line.Direction == Left)
                        {
                            rope[knotIndex] = (currentHeadPosition.Row, currentHeadPosition.Col - 1);
                        }

                        continue;
                    }

                    var knot = rope[knotIndex];
                    var knotInFront = rope[knotIndex - 1];

                    // If knot is adjacent to knot in front: no further knot moves
                    if (AreAdjacent(knotInFront, knot))
                    {
                        break;
                    }

                    // Move knot correctly, relative to knot in front of it
                    rope[knotIndex] = GetNewKnotLocation(knot, knotInFront);

                    // If last knot (tail): Add visited location
                    if (knotIndex == knots - 1)
                    {
                        tailVisitLocations.Add(rope[knotIndex]);
                    }
                }
            }
        }
        
        return tailVisitLocations.Count;
    }
}
