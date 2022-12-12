// See https://aka.ms/new-console-template for more information
using System.Collections.Immutable;
using System.Text.RegularExpressions;

var r = new Regex("^Monkey (\\d*):\\s*Starting items: (.*)\\s*Operation: new = old ([^\\d|.]) (.*)\\s*Test: divisible by (\\d*)\\s*If true: throw to monkey (\\d*)\\s*If false: throw to monkey (\\d)$");

var file = System.IO.File.OpenText("test.txt");
var input = file.ReadToEnd()
    .Split("\n\n")
    .Select(l =>
    {
        var match = r.Match(l);
        // assume it's match :)
        var opTerm = (Func<ulong, ulong>)(match.Groups[4].Value switch
        {
            "old" => (x) => x,
            _ => (x) => ulong.Parse(match.Groups[4].Value)
        });
        var op = (Func<ulong, ulong>)(match.Groups[3].Value.Trim() switch
        {
            "+" => (x) => x + opTerm(x),
            "*" => (x) => x * opTerm(x),
            _ => (x) => x
        });

        return (
            Monkey: int.Parse(match.Groups[1].Value),
            Items: new Queue<ulong>(match.Groups[2].Value.Split(",").Select(v => ulong.Parse(v.Trim()))),
            Operation: op,
            Divide: ulong.Parse(match.Groups[5].Value),
            ThrowTrue: int.Parse(match.Groups[6].Value),
            ThrowFalse: int.Parse(match.Groups[7].Value)
        );
    });

Func<IList<(int Monkey, Queue<ulong> Items, Func<ulong, ulong> Operation, ulong Divide, int ThrowTrue, int ThrowFalse)>, ulong, Func<ulong, ulong>, ulong> monkeyBusiness = (monkeyIn, rounds, wlDiv) =>
{
    var monkeyInspect = Enumerable.Range(0, monkeyIn.Count()).Select(x => (ulong)0).ToArray();

    var round = 0UL;
    while (round < rounds)
    {
        foreach (var monkey in monkeyIn)
        {
            ulong item;
            while (monkey.Items.TryDequeue(out item))
            {
                monkeyInspect[monkey.Monkey]++;
                var wl = monkey.Operation(item);
                wl = wlDiv(wl);
                if (wl % monkey.Divide == 0)
                {
                    monkeyIn[monkey.ThrowTrue].Items.Enqueue(wl);
                }
                else
                {
                    monkeyIn[monkey.ThrowFalse].Items.Enqueue(wl);
                }
            }
        }
        round++;
    }

    var busy = monkeyInspect.OrderByDescending(x => x).ToArray();
    return busy[0] * busy[1];
};
Console.WriteLine($"Part 1: {monkeyBusiness(input.ToImmutableList(), 20, (x) => (ulong)Math.Floor(x / 3.0))}");

// "floor" the numbers at the product of divisors
var floor = input.Aggregate((ulong)1, (acc, monkey) => acc * monkey.Divide);
Console.WriteLine($"Part 1: {monkeyBusiness(input.ToImmutableList(), 10_000, (x) => x % floor)}");