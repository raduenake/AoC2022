// See https://aka.ms/new-console-template for more information
using System.Collections.Immutable;

var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd()
    .Split("\r\n")
    .Where(l => !string.IsNullOrEmpty(l))
    .ToImmutableArray();

var ranges = input.Select(l =>
{
    var s = l.Split(',');
    var s1 = s[0].Split('-').Select(c => int.Parse(c)).ToArray();
    var s2 = s[1].Split('-').Select(c => int.Parse(c)).ToArray();
    return (left: (s1[0], s1[1]), right: (s2[0], s2[1]));
}).ToImmutableArray();

var x = ranges.Sum(r =>
    ((r.left.Item1 <= r.right.Item1 && r.left.Item2 >= r.right.Item2) ||
    (r.right.Item1 <= r.left.Item1 && r.right.Item2 >= r.left.Item2))
        ? 1 : 0
);

Console.WriteLine($"Part 1: {x}");

var y = ranges.Sum(r =>
{
    var one = Enumerable.Range(r.left.Item1, (r.left.Item2 - r.left.Item1) + 1).ToArray();
    var two = Enumerable.Range(r.right.Item1, (r.right.Item2 - r.right.Item1) + 1).ToArray();
    return one.Intersect(two).Any() ? 1 : 0;
});

Console.WriteLine($"Part 2: {y}");