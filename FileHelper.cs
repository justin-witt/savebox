using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml.Linq;

static class FileHelper
{
    private static readonly char[] InvalidFileNameChars = Path.GetInvalidFileNameChars();

    public static void SaveGame(Game game)
    {
        string? name;

        do
        {
            Console.Write("Enter a name for the save: ");
            name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("!!!NAME CANNOT BE EMPTY PLEASE TRY AGAIN!!!");
            }
            else if (name.IndexOfAny(InvalidFileNameChars) != -1)
            {
                Console.WriteLine("!!!SAVE NAME CONTAINS INVALID CHARS PLEASE TRY AGAIN!!!");
                name = null;
            }
        }
        while (string.IsNullOrEmpty(name));

        string destFolder = Path.Combine(game.SavePath, Constants.SaveFolderName, name);

        if (Directory.Exists(destFolder))
        {
            Console.Write($"{destFolder} already exists. Overwite save data? (y/n): ");
            if (Console.ReadLine()?.ToLower().Trim() != "y") return;
            Directory.Delete(destFolder, true);
        }

        Directory.CreateDirectory(destFolder);

        foreach (var file in Directory.GetFiles(game.SavePath))
        {
            string fileName = Path.GetFileName(file);

            if (game.SaveFileFilter(fileName))

            {   
                File.Copy(file, Path.Combine(destFolder, fileName), true);
            }
        }

        Console.WriteLine("Save complete. Press Enter to continue...");
        Console.ReadLine();
    }

    public static void LoadGame(Game game)
    {
        string holder = Path.Combine(game.SavePath, Constants.SaveFolderName);
        string[] saves = Directory.GetDirectories(holder);

        if (saves.Length == 0)
        {
            Console.WriteLine("No saves found. Press Enter to continue...");
            Console.ReadLine();
            return;
        }

        int choice = 0;

        do
        {
            for (int i = 0; i < saves.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {Path.GetFileName(saves[i])}");
            }

            Console.Write("Choose a save to load: ");
            if (!int.TryParse(Console.ReadLine(), out choice) || choice <= 0 || choice > saves.Length)
            {
                Console.WriteLine("!!!PLEASE SELECT A VALID SAVE NUMBER!!!");
                choice = 0;
            }
        }
        while (choice == 0);

        foreach (var file in Directory.GetFiles(saves[choice - 1]))
        {
            var source = new FileInfo(file);
            string fileName = Path.GetFileName(file);
            source.CopyTo(Path.Combine(game.SavePath, fileName), true);
        }

        Console.WriteLine("Save loaded. Press Enter to continue...");
        Console.ReadLine();
    }

    public static void WipeCurrentSave(Game game)
    {
        Console.Write("Save before wiping? (y/n): ");
        if (Console.ReadLine()?.ToLower().Trim() == "y") SaveGame(game);

        Console.Write("Are you sure you want to clear the save data? (y/n): ");
        if (Console.ReadLine()?.ToLower().Trim() != "y") return;

        foreach (var file in Directory.GetFiles(game.SavePath))
        {
            if (game.SaveFileFilter(Path.GetFileName(file))) File.Delete(file);
        }

        Console.WriteLine("Save wiped. Press Enter to continue...");
        Console.ReadLine(); 
    }

    public static void DeleteSave(Game game)
    {
        string holder = Path.Combine(game.SavePath, Constants.SaveFolderName);
        string[] saves = Directory.GetDirectories(holder);

        if (saves.Length == 0)
        {
            Console.WriteLine("No saves found. Press Enter to continue...");
            Console.ReadLine();
            return;
        }

        int choice = 0;

        do
        {
            for (int i = 0; i < saves.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {Path.GetFileName(saves[i])}");
            }

            Console.WriteLine($"{saves.Length + 1}. Back");

            Console.Write("Choose a save to delete: ");
            if (!int.TryParse(Console.ReadLine(), out choice) || choice <= 0 || choice > saves.Length + 1)
            {
                Console.WriteLine("!!!PLEASE SELECT A VALID SAVE NUMBER!!!");
                choice = 0;
            }
        }
        while (choice == 0);

        if (choice == saves.Length + 1) return;

        Console.Write($"Are you sure you want delete '{Path.GetFileName(saves[choice - 1])}' (y/n): ");
        if (Console.ReadLine()?.ToLower().Trim() != "y") return;

        try
        {
            Directory.Delete(saves[choice - 1], true);
            Console.WriteLine("Save deleted. Press Enter to continue...");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to delete save. Error: {ex.Message}");
            Console.WriteLine("Press Enter to continue...");
        }
    }
}
