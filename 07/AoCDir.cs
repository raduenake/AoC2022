public class AoCDir
{
    public string Name;
    public AoCDir Parent = null;
    public List<AoCDir> AoCSubDirs = new List<AoCDir>();
    public List<AoCFile> AoCFiles = new List<AoCFile>();
    public int Size => AoCFiles.Sum(aocF => aocF.Size) + AoCSubDirs.Sum(aocD => aocD.Size);
}