namespace Common.Extensions;

public static class ArrayExtensions
{
    public static void Print<T>(this T[,] matrix)
    {
        foreach (var y in Enumerable.Range(0, matrix.GetLength(1)))
        {
            foreach (var x in Enumerable.Range(0, matrix.GetLength(0)))
            {
                Console.Write(matrix[x, y]);
            }

            Console.WriteLine();
        }
    }
}
