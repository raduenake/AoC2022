
// See https://aka.ms/new-console-template for more information
using System.Collections.Immutable;
using System.Numerics;

var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd()
    .Split("\n")
    .Where(l => !string.IsNullOrEmpty(l))
    .Select(l =>
    {
        var split = l.Split(" ");
        return (split[0], int.Parse(split[1]));
    })
    .ToImmutableArray();

var dirs = new[] { new Vector2(-1, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(0, -1) };
Func<IEnumerable<(string, int)>, int, int> solve = (ops, ropeLength) =>
{
    var rope = Enumerable.Range(0, ropeLength).Select(r => new Vector2(0, 0)).ToArray();
    var tailVisited = new HashSet<Vector2>();

    foreach (var operation in ops)
    {
        tailVisited.Add(rope[rope.Length - 1]);
        var move = operation.Item1 switch
        {
            "L" => dirs[0],
            "R" => dirs[1],
            "U" => dirs[2],
            "D" => dirs[3],
            _ => new Vector2(0, 0)
        };

        for (int i = 0; i < operation.Item2; i++)
        {
            rope[0] += move;
            for (int r = 1; r < rope.Length; r++)
            {
                var prevKnot = rope[r - 1];
                var knot = rope[r];

                var knotDifference = prevKnot - knot;
                var knotMove = new Vector2(0, 0);

                if (Math.Abs(knotDifference.X) > 1 || Math.Abs(knotDifference.Y) > 1)
                {
                    knotMove += prevKnot.X.CompareTo(knot.X) switch
                    {
                        1 => dirs[1],
                        -1 => dirs[0],
                        _ => new Vector2(0, 0),
                    };
                    knotMove += prevKnot.Y.CompareTo(knot.Y) switch
                    {
                        1 => dirs[2],
                        -1 => dirs[3],
                        _ => new Vector2(0, 0)
                    };

                }

                rope[r] += knotMove;
                tailVisited.Add(rope[rope.Length - 1]);
            }
        }
    }

    return tailVisited.Count();
};


Console.WriteLine($"Part 1: {solve(input, 2)}");
Console.WriteLine($"Part 2: {solve(input, 10)}");