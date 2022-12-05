// See https://aka.ms/new-console-template for more information
using System.Collections.Immutable;

var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd()
    .Split("\r\n")
    .Where(l => !string.IsNullOrEmpty(l))
    .ToImmutableArray();

var common = input.Select(l =>
{
    var lChar = l.Select(c => c);
    var left = lChar.Take(lChar.Count() / 2).ToImmutableArray();
    var right = lChar.Skip(lChar.Count() / 2).ToImmutableArray();
    return left.First(c => right.Any(rc => rc == c));
}).ToImmutableArray();

var p1 = common.Sum(c => Char.IsLower(c) switch { true => 1 + (c - 'a'), false => 27 + (c - 'A') });
Console.WriteLine($"Part 1: {p1}");

var chunkedCommon = input.Chunk(3).Select(l => l[0].First(c => l[1].Any(rc => rc == c && l[2].Any(rcc => rcc == c))))
    .ToImmutableArray();
var p2 = chunkedCommon.Sum(c => Char.IsLower(c) switch { true => 1 + (c - 'a'), false => 27 + (c - 'A') });

Console.WriteLine($"Part 2: {p2}");
