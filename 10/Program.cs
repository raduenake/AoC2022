// See https://aka.ms/new-console-template for more information
using System.Collections.Immutable;

var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd()
    .Split("\n")
    .Where(l => !string.IsNullOrEmpty(l))
    .Select(l =>
    {
        var split = l.Split(" ");
        return (split[0], split.Length > 1 ? int.Parse(split[1]) : 0);
    })
    .ToImmutableArray();

var history = new Dictionary<int, int>();
var ssCycles = new[] { 20, 60, 100, 140, 180, 220 };
var screenPixels = Enumerable.Range(0, 240).Select(c => ".").ToArray();

var tick = 0;
var opTicksRemaining = 0;
var x = 1;
var currentOp = ("", 0);

var instrEnum = input.GetEnumerator();
var done = false;
while (!done)
{
    // start of tick-th cycle
    if (ssCycles.Contains(tick))
    {
        history.Add(tick, x);
    }

    if (opTicksRemaining == 0)
    {
        x += currentOp.Item1 switch
        {
            "addx" => currentOp.Item2,
            _ => 0
        };

        done = !instrEnum.MoveNext();
        if (!done)
        {
            currentOp = instrEnum.Current;
            opTicksRemaining = currentOp.Item1 switch
            {
                "noop" => 1,
                "addx" => 2,
                _ => 0
            };
        }
    }

    // end of tick-th cycle
    // screen
    if (Enumerable.Range(x - 1, 3).Contains(tick % 40))
    {
        screenPixels[tick] = "#";
    }

    opTicksRemaining--;
    tick++;
}

Console.WriteLine($"Part 1: {ssCycles.Sum(c => c * history[c])}");

Console.WriteLine($"Part 2:");
for (int i = 0; i < screenPixels.Length; i++)
{
    if (i > 0)
    {
        // we know there are eight letters (40 / 8)
        if (i % 5 == 0)
        {
            Console.Write(" ");
        }
        // each line has 40 pixels
        if (i % 40 == 0)
        {
            Console.WriteLine();
        }
    }
    Console.Write(screenPixels[i]);
}