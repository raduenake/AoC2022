// See https://aka.ms/new-console-template for more information
using System.Collections.Immutable;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd()
    .Split("\n\n")
    .Select(l =>
    {
        var ll = l.Split("\n")
            .Select(l1 => new Packet(JsonConvert.DeserializeObject<JToken>(l1)))
            .ToArray();
        return (First: ll[0], Second: ll[1]);
    })
    .ToImmutableList();

Func<Packet, Packet, int> comparePcks = (first, second) => 0;
comparePcks = (first, second) =>
{
    var comparisonResult = 0;
    if (first.PacketType == second.PacketType)
    {
        if (first.PacketType == PacketDataType.Integer)
        {
            comparisonResult = (first.PacketValue ?? -1).CompareTo(second.PacketValue ?? -1);
        }
        else
        {
            var i = 0;
            while (i < first.DataItems.Count() && i < second.DataItems.Count())
            {
                comparisonResult = comparePcks(first.DataItems[i], second.DataItems[i]);
                i++;

                if (comparisonResult != 0)
                {
                    break;
                }
            }

            if (first.DataItems.Count() == second.DataItems.Count() &&
                i == first.DataItems.Count())
            {
                // no op
            }
            else
            {
                if (comparisonResult == 0)
                {
                    comparisonResult = (i == first.DataItems.Count()) ? -1 : 1;
                }
            }
        }
    }
    else
    {
        var firstBoxed = first.PacketType == PacketDataType.Integer ? new Packet(first) : first;
        var secondBoxed = second.PacketType == PacketDataType.Integer ? new Packet(second) : second;
        comparisonResult = comparePcks(firstBoxed, secondBoxed);
    }

    return comparisonResult;
};

var p1 = input.Select((pckPair, i) => comparePcks(pckPair.First, pckPair.Second) <= 0 ? i + 1 : 0).Sum();
Console.WriteLine($"Part 1: {p1}");

var decPck1 = new Packet(JsonConvert.DeserializeObject<JToken>("[[2]]"));
var decPck2 = new Packet(JsonConvert.DeserializeObject<JToken>("[[6]]"));

var flatPcks = input.SelectMany(inp => new[] { inp.First, inp.Second }).ToList();
flatPcks.AddRange(new[] { decPck1, decPck2 });
flatPcks.Sort((x, y) => comparePcks(x, y));
Console.WriteLine($"Part 2: {(flatPcks.IndexOf(decPck1) + 1) * (flatPcks.IndexOf(decPck2) + 1)}");