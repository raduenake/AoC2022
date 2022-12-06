// See https://aka.ms/new-console-template for more information
using System.Collections.Immutable;
using System.Text.RegularExpressions;

var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd()
    .Split("\r\n")
    .Where(l => !string.IsNullOrEmpty(l))
    .ToImmutableArray();

var crates = input.Take(8).Reverse().ToImmutableArray();
var startStacks = Enumerable.Range(0, 9).Select(i => new List<string>()).ToImmutableArray();

foreach (var crateLine in crates)
{
    var stack = 0;
    for (int i = 2; i < crateLine.Length; i += 3)
    {
        var crate = string.Join("", crateLine.Skip(i - (3 - 1)).Take(3))
            .Replace("[", "")
            .Replace("]", "");

        if (!string.IsNullOrWhiteSpace(crate))
        {
            startStacks[stack].Add(crate);
        }
        // increment stack
        stack++;
        // skip empty space
        i++;
    }
}

Func<string, (int, int, int)> parseInstruction = (instr) =>
{
    var rx = new Regex(@"^move (.*) from (.*) to (.*)$");
    var match = rx.Match(instr);

    var amount = int.Parse(match.Groups[1].Value);
    var source = int.Parse(match.Groups[2].Value) - 1;
    var dest = int.Parse(match.Groups[3].Value) - 1;

    return (amount, source, dest);
};

var stacks1 = startStacks.Select(s => new Stack<string>(s)).ToImmutableArray();
foreach (var instr in input.Skip(9).Where(l => !string.IsNullOrWhiteSpace(l)))
{
    var pInstr = parseInstruction(instr);
    for (int i = 0; i < pInstr.Item1; i++)
    {
        stacks1[pInstr.Item3].Push(stacks1[pInstr.Item2].Pop());
    }
}
Console.WriteLine($"Part1 : {string.Join("", stacks1.Select(s => s.Peek()))}");

var stacks2 = startStacks.Select(s => new Stack<string>(s)).ToImmutableArray();
foreach (var instr in input.Skip(9).Where(l => !string.IsNullOrWhiteSpace(l)))
{
    var pInst = parseInstruction(instr);

    var cratesPopped = new List<string>();
    for (int i = 0; i < pInst.Item1; i++)
    {
        cratesPopped.Add(stacks2[pInst.Item2].Pop());
    }

    cratesPopped.Reverse();
    foreach (var el in cratesPopped)
    {
        stacks2[pInst.Item3].Push(el);
    }
}
Console.WriteLine($"Part1 : {string.Join("", stacks2.Select(s => s.Peek()))}");
