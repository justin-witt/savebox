static class Constants
{
    public const String SaveFolderName = "savesholder";

    public static readonly Game[] Games =
    {
        new Game(
            "Incursion Red River",
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Test_C", "Saved", "SaveGames"),
            file => !file.Equals("GameSettingsSaveGame.sav", StringComparison.OrdinalIgnoreCase)
            ),
        new Game(
            "The Forever Winter",
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ForeverWinter", "Saved", "SaveGames"),
            file => file.EndsWith(".dat", StringComparison.OrdinalIgnoreCase)
            )
    };
}
