using System;
using System.Collections.Generic;

static class App
{
    private static List<Game> AvailableGames = new();

    public static void Initialize()
    {
        AvailableGames = GameManager.DetectGames();
    }

    public static void Run()
    {
        while (true)
        {
            Console.WriteLine("Available Games: ");

            for (int i = 0; i < AvailableGames.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {AvailableGames[i].Name}");
            }
            Console.WriteLine($"{AvailableGames.Count + 1}. Exit");

            Console.Write("Chose a game: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= AvailableGames.Count + 1)
            {
                if (choice == AvailableGames.Count + 1) break;
                GameManager.GameMenu(AvailableGames[choice - 1]);
            }
        }
    }
}