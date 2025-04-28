class Game
{
    public string Name { get; }
    public string SavePath { get; }
    public Func<string, bool> SaveFileFilter { get; }

    public Game(string name, string savePath, Func<string, bool>? saveFileFilter = null)
    {
        Name = name;
        SavePath = savePath;
        SaveFileFilter = saveFileFilter ?? (file => true);
    }
}
