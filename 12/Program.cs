// See https://aka.ms/new-console-template for more information
using System.Numerics;
using System.Collections.Immutable;

var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd()
    .Split("\n")
    .Where(l => !string.IsNullOrEmpty(l))
    .SelectMany((l, i) => l.Select((c, j) => (new Vector2(i, j), c)))
    .ToDictionary(kv => kv.Item1, kv => kv.Item2);

var neighborDirs = new[] { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1) };

Func<Vector2, Vector2, Dictionary<Vector2, Vector2>, int> computePathLength = (start, end, parent) =>
{
    var pathLength = 0;
    var pathBuilder = end;
    while (pathBuilder != start)
    {
        pathLength++;
        pathBuilder = parent[pathBuilder];
    }
    return pathLength;
};

Func<Dictionary<Vector2, char>, Vector2, Vector2, int> bfs = (field, start, end) =>
{
    var toVisit = new Queue<Vector2>();
    var parent = new Dictionary<Vector2, Vector2>();
    var explored = new HashSet<Vector2>() { start };

    toVisit.Enqueue(start);
    while (toVisit.Any())
    {
        var current = toVisit.Dequeue();
        if (current == end)
        {
            return computePathLength(start, end, parent);
        }
        var neighborOptions = neighborDirs.Select(d => current + d)
            .Where(r =>
                field.ContainsKey(r) &&
                !explored.Contains(r) &&
                (field[r] >= field[current] || (field[current] - field[r] >= 0 && field[current] - field[r] <= 1))
            );
        foreach (var nbr in neighborOptions)
        {
            explored.Add(nbr);
            parent.Add(nbr, current);
            toVisit.Enqueue(nbr);
        }
    }

    return 0;
};

Func<Dictionary<Vector2, char>, Vector2, Vector2, int> reverseDijkstra = (field, start, end) =>
{
    var distances = new Dictionary<Vector2, int>();
    var parent = new Dictionary<Vector2, Vector2>();
    var toVisit = field.Select(kv => kv.Key).ToList();

    distances.Add(start, 0);
    while (toVisit.Any())
    {
        var current = toVisit.MinBy(v => !distances.ContainsKey(v) ? int.MaxValue : distances[v]);
        toVisit.Remove(current);
        if (current == end)
        {
            return computePathLength(start, end, parent);
        }
        var neighborOptions = neighborDirs.Select(d => current + d)
            .Where(r =>
                field.ContainsKey(r) &&
                toVisit.Contains(r) &&
                (field[r] >= field[current] || (field[current] - field[r] >= 0 && field[current] - field[r] <= 1))
            );
        foreach (var neighbor in neighborOptions)
        {
            // edge weight is 1 in our case
            var alt = distances[current] + 1;
            if (!distances.ContainsKey(neighbor) || alt < distances[neighbor])
            {
                distances[neighbor] = alt;
                parent[neighbor] = current;
            }
        }
    }

    return 0;
};

var start = input.First(kv => kv.Value == 'S').Key;
var end = input.First(kv => kv.Value == 'E').Key;
input[start] = 'a';
input[end] = 'z';

var p1 = bfs(input, end, start);
Console.WriteLine($"Part 1: {p1}");

// var p1D = reverseDijkstra(input, end, start);
// Console.WriteLine($"Part 1 Dijkstra: {p1D}");

var starts = input
    .Where(kv => kv.Value == 'a')
    .Select(kv => kv.Key)
    .ToImmutableArray();
var p2 = starts.AsParallel().Select(s => bfs(input, end, s)).Where(length => length > 0).Min();
Console.WriteLine($"Part 2: {p2}");