// See https://aka.ms/new-console-template for more information
using System.Collections.Immutable;
using System.Text.RegularExpressions;

var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd()
    .Split("\n")
    .Where(l => !string.IsNullOrEmpty(l))
    .ToImmutableArray();

const string rootDirName = "/";
var regex = new Regex("^(?>\\$ (cd) (\\S*))|(?>\\$ (ls))|(?>(dir) (\\S*))|(?>([0-9]*) (\\S*))$");
var rootDir = new AoCDir() { Name = rootDirName };
AoCDir currentDir = null;

var cmdEnumerator = input.GetEnumerator();
while (cmdEnumerator.MoveNext())
{
    var cmd = cmdEnumerator.Current;
    var match = regex.Match(cmd);

    if (!match.Success)
    {
        continue;
    }

    //$ cd xyz
    if (match.Groups[1].Success)
    {
        var aocDirName = match.Groups[2].Value;
        currentDir = aocDirName switch
        {
            rootDirName => rootDir,
            ".." => currentDir.Parent,
            _ => currentDir.AoCSubDirs.First(aocD => aocD.Name == aocDirName)
        };
    }
    //$ ls
    else if (match.Groups[3].Success)
    {
        // no op
    }
    //dir abc
    else if (match.Groups[4].Success)
    {
        var aocDirName = match.Groups[5].Value;
        if (!currentDir.AoCSubDirs.Any(aocD => aocD.Name == aocDirName))
        {
            currentDir.AoCSubDirs.Add(new AoCDir() { Name = aocDirName, Parent = currentDir });
        }
    }
    //1234 file.name
    else if (match.Groups[6].Success)
    {
        var fileName = match.Groups[7].Value;
        if (!currentDir.AoCFiles.Any(aocf => aocf.Name == fileName))
        {
            currentDir.AoCFiles.Add(new AoCFile() { Name = fileName, Size = int.Parse(match.Groups[6].Value), Parent = currentDir });
        }
    }
}

Func<AoCDir, int, int> traverseSumOfMaxSize = (cd, maxSize) => { return 0; };
traverseSumOfMaxSize = (cd, maxSize) =>
{
    int s = cd.Size < maxSize ? cd.Size : 0;
    s += cd.AoCSubDirs.Sum(aocd => traverseSumOfMaxSize(aocd, maxSize));
    return s;
};

Console.WriteLine($"Part 1: {traverseSumOfMaxSize(rootDir, 100000)}");

Func<AoCDir, List<AoCDir>> flatten = (cd) => { return new List<AoCDir>(); };
flatten = (cd) =>
{
    var r = new List<AoCDir>() { cd };
    r.AddRange(cd.AoCSubDirs.SelectMany(aocd => flatten(aocd)));
    return r;
};

var unused = 70_000_000 - rootDir.Size;
var toDelete = 30_000_000 - unused;
var flatDirList = flatten(rootDir).OrderBy(d => d.Size).ToImmutableArray();
var sizeOfDirToDelete = flatDirList.First(d => d.Size >= toDelete).Size;

Console.WriteLine($"Part 2: {sizeOfDirToDelete}");
