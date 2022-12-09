using Common.Interfaces;

namespace Year2022;

public class Day09Solver : IPuzzleSolver
{
    public string Title => "Rope Bridge";

    public string Part1Solution { get; set; } = string.Empty;
    public string Part2Solution { get; set; } = string.Empty;

    private const string Right = "R";
    private const string Left = "L";
    private const string Up = "U";
    private const string Down = "D";

    public void SolvePuzzle(string[] input)
    {
        Part1Solution = SolvePart1(input).ToString();
        Part2Solution = SolvePart2(input).ToString();
    }

    private static int SolvePart1(IEnumerable<string> input)
    {
        var head = (Row: 0, Col: 0);
        var tail = (Row: 0, Col: 0);

        var tailVisitLocations = new HashSet<(int Row, int Col)> { tail };

        bool AreOverlapping() => head.Row == tail.Row && head.Col == tail.Col;
        
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

        var instructions = input
            .Select(item => item.Split(' '))
            .Select(parts => (Direction: parts[0], Steps: int.Parse(parts[1])))
            .ToList();

        foreach (var line in instructions)
        {
            foreach (var step in Enumerable.Range(1, line.Steps))
            {
                if (AreOverlapping())
                {
                    MoveHead(line.Direction);
                    // Tail does not move, no matter where head moves

                    continue;
                }

                if (HeadIsDirectlyAbove())
                {
                    MoveHead(line.Direction);
                    // Tail only moves if head moves upwards
                    if (line.Direction == Up)
                    {
                        MoveTail(Up);
                        tailVisitLocations.Add(tail);
                    }

                    continue;
                }

                if (HeadIsDirectlyBelow())
                {
                    MoveHead(line.Direction);
                    // Tail only moves if head moves downwards
                    if (line.Direction == Down)
                    {
                        MoveTail(Down);
                        tailVisitLocations.Add(tail);
                    }
                    
                    continue;
                }

                if (HeadIsDirectlyToTheRight())
                {
                    MoveHead(line.Direction);
                    // Tail only moves if head moves rightwards
                    if (line.Direction == Right)
                    {
                        MoveTail(Right);
                        tailVisitLocations.Add(tail);
                    }

                    continue;
                }

                if (HeadIsDirectlyToTheLeft())
                {
                    MoveHead(line.Direction);
                    // Tail only moves if head moves leftwards
                    if (line.Direction == Left)
                    {
                        MoveTail(Left);
                        tailVisitLocations.Add(tail);
                    }
                    
                    continue;
                }

                if (HeadIsDirectlyAboveToTheRight())
                {
                    MoveHead(line.Direction);
                    // Tail only moves if head moves upwards or rightwards
                    if (line.Direction == Up || line.Direction == Right)
                    {
                        MoveTail(Up);
                        MoveTail(Right);
                        tailVisitLocations.Add(tail);
                    }
                    
                    continue;
                }

                if (HeadIsDirectlyAboveToTheLeft())
                {
                    MoveHead(line.Direction);
                    // Tail only moves if head moves upwards or leftwards
                    if (line.Direction == Up || line.Direction == Left)
                    {
                        MoveTail(Up);
                        MoveTail(Left);
                        tailVisitLocations.Add(tail);
                    }

                    continue;
                }

                if (HeadIsDirectlyBelowToTheRight())
                {
                    MoveHead(line.Direction);
                    // Tail only moves if head moves downwards or rightwards
                    if (line.Direction == Down || line.Direction == Right)
                    {
                        MoveTail(Down);
                        MoveTail(Right);
                        tailVisitLocations.Add(tail);
                    }
                    
                    continue;
                }

                if (HeadIsDirectlyBelowToTheLeft())
                {
                    MoveHead(line.Direction);
                    // Tail only moves if head moves downwards or leftwards
                    if (line.Direction == Down || line.Direction == Left)
                    {
                        MoveTail(Down);
                        MoveTail(Left);
                        tailVisitLocations.Add(tail);
                    }
                }
            }
        }
        
        return tailVisitLocations.Count;
    }
    
    private static int SolvePart2(IEnumerable<string> input)
    {
        const int knots = 10;
        
        var rope = Enumerable.Repeat((Row: 0, Col: 0), knots).ToList();

        var tailVisitLocations = new HashSet<(int Row, int Col)>();

        tailVisitLocations.Add(rope.Last());
        
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
            
            var needsToMoveUp = rowOffset > 1 || (rowOffset == 1 && Math.Abs(colOffset) > 1);
            var needsToMoveDown = rowOffset < -1 || (rowOffset == -1 && Math.Abs(colOffset) > 1);
            var needsToMoveRight = colOffset > 1 || (colOffset == 1 && Math.Abs(rowOffset) > 1);
            var needsToMoveLeft = colOffset < -1 || (colOffset == -1 && Math.Abs(rowOffset) > 1);

            if (needsToMoveUp && needsToMoveRight) return (currentKnotPosition.Row + 1, currentKnotPosition.Col + 1);
            if (needsToMoveUp && needsToMoveLeft) return (currentKnotPosition.Row + 1, currentKnotPosition.Col - 1);
            if (needsToMoveUp) return (currentKnotPosition.Row + 1, currentKnotPosition.Col);
            if (needsToMoveDown && needsToMoveRight) return (currentKnotPosition.Row - 1, currentKnotPosition.Col + 1);
            if (needsToMoveDown && needsToMoveLeft) return (currentKnotPosition.Row - 1, currentKnotPosition.Col - 1);
            if (needsToMoveDown) return (currentKnotPosition.Row - 1, currentKnotPosition.Col);
            if (needsToMoveRight) return (currentKnotPosition.Row, currentKnotPosition.Col + 1);
            if (needsToMoveLeft) return (currentKnotPosition.Row, currentKnotPosition.Col - 1);

            // Should never occur, but...
            return currentKnotPosition;
        }
        
        var instructions = input
            .Select(item => item.Split(' '))
            .Select(parts => (Direction: parts[0], Steps: int.Parse(parts[1])))
            .ToList();

        foreach (var line in instructions)
        {
            foreach (var step in Enumerable.Range(1, line.Steps))
            {
                for (var i = 0; i < knots; i++)
                {
                    // If head: Move head --> continue
                    if (i == 0)
                    {
                        var currentHeadPosition = rope[i];
                        
                        if (line.Direction == Up)
                        {
                            rope[i] = (currentHeadPosition.Row + 1, currentHeadPosition.Col);
                        }
                        else if (line.Direction == Down)
                        {
                            rope[i] = (currentHeadPosition.Row - 1, currentHeadPosition.Col);
                        }
                        else if (line.Direction == Right)
                        {
                            rope[i] = (currentHeadPosition.Row, currentHeadPosition.Col + 1);
                        }
                        else if (line.Direction == Left)
                        {
                            rope[i] = (currentHeadPosition.Row, currentHeadPosition.Col - 1);
                        }

                        continue;
                    }

                    var knot = rope[i];
                    var knotInFront = rope[i - 1];

                    // If knot is adjacent to knot in front: no further knot moves
                    if (AreAdjacent(knotInFront, knot))
                    {
                        break;
                    }

                    // Move knot correctly, relative to knot in front of it
                    rope[i] = GetNewKnotLocation(knot, knotInFront);

                    // If last knot (tail): Add visited location
                    if (i == knots - 1)
                    {
                        tailVisitLocations.Add(rope[i]);
                    }
                }
            }
        }
        
        return tailVisitLocations.Count;
    }
}
