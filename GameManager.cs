using System;
using System.Collections.Generic;
using System.IO;

static class GameManager
{
    public static List<Game> DetectGames()
    {
        var games = new List<Game>();

        foreach (var game in Constants.Games)
        {
            if (Directory.Exists(game.SavePath))
            {
                string holder = Path.Combine(game.SavePath, Constants.SaveFolderName);
                if (!Directory.Exists(holder)) Directory.CreateDirectory(holder);
                games.Add(game);
            }
            else
            {
                Console.WriteLine($"Save folder not found for: {game.Name}");
            }
        }
        return games;
    }

    public static void GameMenu(Game game)
    {
        while (true)
        {
            Console.WriteLine($"Managing saves for: {game.Name}");
            Console.WriteLine("1. Save Current Progress");
            Console.WriteLine("2. Load a Save");
            Console.WriteLine("3. Clear Save Data");
            Console.WriteLine("4. Delete a Save");
            Console.WriteLine("5. Back");
            Console.Write("Choice: ");

            switch (Console.ReadLine())
            {
                case "1": FileHelper.SaveGame(game); break;
                case "2": FileHelper.LoadGame(game); break;
                case "3": FileHelper.WipeCurrentSave(game); break;
                case "4": FileHelper.DeleteSave(game); break;
                case "5": return;
                default: Console.WriteLine("Something went wrong. Please try again..."); break;
            }
        }
    }
}
