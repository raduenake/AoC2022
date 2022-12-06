// See https://aka.ms/new-console-template for more information
using System.Collections.Immutable;

var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd()
    .Split("\r\n")
    .Where(l => !string.IsNullOrEmpty(l))
    .ToImmutableArray();

// the message line
var line = input.First();

Func<string, int, int, bool> solution = (line, start, length) =>
{
    var chs = line.Skip(start - (length - 1)).Take(length);
    return !chs.Where((c, i) => chs.Where((c1, i1) => i1 != i && c1 == c).Any()).Any();
};

var idx = 3;
for (int i = idx; i < line.Length; i++)
{
    var done = solution(line, i, 4);
    if (done)
    {
        idx = i;
        break;
    }
}
Console.WriteLine($"Part 1: {idx + 1}");

// 13 chars 
idx = 13;
for (int i = idx; i < line.Length; i++)
{
    var done = solution(line, i, 14);
    if (done)
    {
        idx = i;
        break;
    }
}

Console.WriteLine($"Part 2: {idx + 1}");