// See https://aka.ms/new-console-template for more information
using System.Collections.Immutable;

var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd()
    .Split("\n")
    .Where(l => !string.IsNullOrEmpty(l))
    .Select(l => l.Select(c => int.Parse("" + c)).ToImmutableArray())
    .ToImmutableArray();

var totalVisible = 0;
for (int i = 0; i < input.Length; i++)
{
    var line = input[i];
    for (int j = 0; j < line.Length; j++)
    {
        var current = line[j];
        var column = Enumerable.Range(0, input.Length).Select(idx => input[idx][j]).ToImmutableArray();
        var isVisible = j == 0 || i == 0 || j == line.Length || i == input.Length ||
            !line[..j].Any(v => v >= current) ||
            !line[(j + 1)..].Any(v => v >= current) ||
            !column[..i].Any(v => v >= current) ||
            !column[(i + 1)..].Any(v => v >= current);
        totalVisible += isVisible ? 1 : 0;
    }
}
Console.WriteLine($"Part 1: {totalVisible}");

var maxScore = 0;
for (int i = 0; i < input.Length; i++)
{
    var line = input[i];
    for (int j = 0; j < line.Length; j++)
    {
        var column = Enumerable.Range(0, input.Length).Select(idx => input[idx][j]).ToImmutableArray();
        var current = line[j];

        var leftNeighbors = (j > 0 ? line[..j].Reverse() : ImmutableArray<int>.Empty).ToImmutableArray();
        var leftSeen = leftNeighbors.IndexOf(leftNeighbors.FirstOrDefault(v => v >= current));
        leftSeen = leftSeen < 0 ? leftNeighbors.Count() : leftSeen + 1;

        var rightNeighbors = (j < line.Length ? line[(j + 1)..] : ImmutableArray<int>.Empty).ToImmutableArray();
        var rightSeen = rightNeighbors.IndexOf(rightNeighbors.FirstOrDefault(v => v >= current));
        rightSeen = rightSeen < 0 ? rightNeighbors.Count() : rightSeen + 1;

        var topNeighbors = (i > 0 ? column[..i].Reverse() : ImmutableArray<int>.Empty).ToImmutableArray();
        var topSeen = topNeighbors.IndexOf(topNeighbors.FirstOrDefault(v => v >= current));
        topSeen = topSeen < 0 ? topNeighbors.Count() : topSeen + 1;

        var bottomNeighbors = (i < input.Length ? column[(i + 1)..] : ImmutableArray<int>.Empty).ToImmutableArray();
        var bottomSeen = bottomNeighbors.IndexOf(bottomNeighbors.FirstOrDefault(v => v >= current));
        bottomSeen = bottomSeen < 0 ? bottomNeighbors.Count() : bottomSeen + 1;

        var score = leftSeen * rightSeen * topSeen * bottomSeen;
        if (score > maxScore)
        {
            maxScore = score;
        }
    }
}

Console.WriteLine($"Part 2: {maxScore}");