// See https://aka.ms/new-console-template for more information
using System.Collections.Immutable;

var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd()
    .Split("\n")
    .Where(l => !string.IsNullOrEmpty(l))
    .ToImmutableArray();

AoCDir root = new AoCDir() { Name = "/" };

AoCDir currentDir = null;
var cmdEnumerator = input.GetEnumerator();
while (cmdEnumerator.MoveNext())
{
    var cmd = cmdEnumerator.Current;
    if (cmd.StartsWith("$ cd"))
    {
        var dirName = cmd.Replace("$ cd ", "");
        if (dirName == "..")
        {
            currentDir = currentDir.Parent;
        }
        else
        {
            currentDir = root.Name == dirName ? root : currentDir.AoCDirs.First(aocd => aocd.Name == dirName);
        }
    }
    else if (cmd.StartsWith("$ ls"))
    {
        // no op
    }
    else
    {
        if (cmd.StartsWith("dir"))
        {
            var dirName = cmd.Replace("dir ", "");
            var dir = currentDir.AoCDirs.FirstOrDefault(aocd => aocd.Name == dirName);
            if (dir == null)
            {
                currentDir.AoCDirs.Add(new AoCDir() { Name = dirName, Parent = currentDir });
            }
        }
        else
        {
            var fileInfo = cmd.Split(" ");
            currentDir.AoCFiles.Add(new AoCFile() { Name = fileInfo[1], Size = int.Parse(fileInfo[0]), Parent = currentDir });
        }
    }
}

Func<AoCDir, int, int> traverseSumOfMaxSize = (cd, maxSize) => { return 0; };
traverseSumOfMaxSize = (cd, maxSize) =>
{
    int s = cd.Size < maxSize ? cd.Size : 0;
    s += cd.AoCDirs.Sum(aocd => traverseSumOfMaxSize(aocd, maxSize));
    return s;
};

Console.WriteLine($"Part 1: {traverseSumOfMaxSize(root, 100000)}");

Func<AoCDir, List<AoCDir>> flatten = (cd) => { return new List<AoCDir>(); };
flatten = (cd) =>
{
    var r = new List<AoCDir>() { cd };
    r.AddRange(cd.AoCDirs.SelectMany(aocd => flatten(aocd)));
    return r;
};

var unused = 70000000 - root.Size;
var toDelete = 30000000 - unused;
var flatDirList = flatten(root).OrderBy(d => d.Size).ToImmutableArray();
var sizeOfDirToDelete = flatDirList.First(d => d.Size >= toDelete).Size;

Console.WriteLine($"Part 2: {sizeOfDirToDelete}");
