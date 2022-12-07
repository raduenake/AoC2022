public class AoCDir
{
    public string Name;
    public AoCDir Parent = null;
    public List<AoCDir> AoCDirs = new List<AoCDir>();
    public List<AoCFile> AoCFiles = new List<AoCFile>();
    public int Size => AoCFiles.Sum(aocf => aocf.Size) + AoCDirs.Sum(aocd => aocd.Size);
}