using Game.Charakters;
using Game.Menus;
using System.Diagnostics.Contracts;

namespace Game.Combat;
class UseItemMenu : Menu
{
    public override void DisplayMenu()
    {
        Console.WriteLine("Inventar: ");
        Console.WriteLine("----------");
        Console.WriteLine();
        Console.WriteLine("wähle ein Item aus:");
    }

    public UseItemMenu(Charakter player, out bool noItemUse)
    {
        for (int i = 0; i < player.Inventory.Count; i++)
            Console.WriteLine($"[{i}] {player.Inventory[i].Name} ({player.Inventory[i].Count}x) [{player.Inventory[i].Type}] <{player.Inventory[i].Description}> ");
        Console.WriteLine($"[{player.Inventory.Count}] zurück zum Kampf");
        HandleInput(player, out noItemUse);
    }

    private void HandleInput(Charakter player, out bool noItemUse)
    {
        noItemUse = true;
        string input = "";
        bool validInput = false;

        while (!validInput)
        {
            Console.Write("> ");
            input = Console.ReadLine();

            switch (input)
            {
                case "0":
                    player.UseItem(player.Inventory[0], out noItemUse);
                    validInput = true;
                    break;
                case "1":
                    player.UseItem(player.Inventory[1], out noItemUse);
                    validInput = true;
                    break;
                case "2":
                    noItemUse = true;
                    validInput = true;
                    break;
                default:
                    validInput = false;
                    break;
            }
        }
    }
}