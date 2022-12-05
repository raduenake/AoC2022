// See https://aka.ms/new-console-template for more information
using System.Collections.Immutable;

var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd()
    .Split("\r\n")
    .Where(l => !string.IsNullOrEmpty(l))
    .ToImmutableList();

var rpsPlayMap = new[] { ("A", 0), ("B", 1), ("C", 2), ("X", 0), ("Y", 1), ("Z", 2) }
    .ToDictionary(w => w.Item1, w => w.Item2);

var part1 = input.Sum(l =>
{
    var rpsPlay = l.Split(' ').Select(l => rpsPlayMap[l]).ToArray();

    return (rpsPlay[1] + 1) + Math.Abs(rpsPlay[1] - rpsPlay[0]) switch
    {
        0 => 3,
        1 => rpsPlay[1] == rpsPlay.Max() ? 6 : 0,
        _ => rpsPlay[1] == rpsPlay.Min() ? 6 : 0
    };
});

Console.WriteLine($"Part 1: {part1}");

var part2 = input.Sum(l =>
{
    var rpsPlayIntermediary = l.Split(' ').ToArray();
    rpsPlayIntermediary[1] = rpsPlayIntermediary[1] switch
    {
        "X" => rpsPlayMap.First(mv => mv.Value == (3 + (rpsPlayMap[rpsPlayIntermediary[0]] - 1)) % 3).Key,
        "Y" => rpsPlayMap.First(mv => mv.Value == rpsPlayMap[rpsPlayIntermediary[0]]).Key,
        "Z" => rpsPlayMap.First(mv => mv.Value == (3 + (rpsPlayMap[rpsPlayIntermediary[0]] + 1)) % 3).Key,
        _ => ""
    };

    var rpsPlay = rpsPlayIntermediary.Select(play => rpsPlayMap[play]).ToArray();
    return (rpsPlay[1] + 1) + Math.Abs(rpsPlay[1] - rpsPlay[0]) switch
    {
        0 => 3,
        1 => rpsPlay[1] == rpsPlay.Max() ? 6 : 0,
        _ => rpsPlay[1] == rpsPlay.Min() ? 6 : 0
    };
});

Console.WriteLine($"Part 2: {part2}");