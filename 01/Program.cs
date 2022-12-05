// See https://aka.ms/new-console-template for more information
using System.Collections.Immutable;

var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd()
    .Split("\r\n\r\n")
    .ToImmutableList();

var elfCalories = input.Select(elf =>
    elf.Split("\r\n")
        .Where(l => !string.IsNullOrEmpty(l))
        .Select(l => int.Parse(l)))
    .Select(ec => ec.Sum());

elfCalories = elfCalories.Order().ToImmutableList();

Console.WriteLine($"Max Elf: {elfCalories.Last()}");
Console.WriteLine($"Top three Elfs: {elfCalories.TakeLast(3).Sum()}");