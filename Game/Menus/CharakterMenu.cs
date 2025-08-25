
using Game.Charakters;

namespace Game.Menus;

class CharakterMenu : Menu
{
    public override void DisplayMenu()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Charakter Erstellen:");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("--------------------");
        Console.WriteLine();
        Console.WriteLine("Name: Aria");
        Console.WriteLine("Klasse Wählen:");
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("[1] Warrior  HP: Hoch, Blocken, Waffe: Schwert");
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("[2] Mage     HP: Mittel, Waffe: Feuerball, Ressource: Mana");
        Console.ForegroundColor = ConsoleColor.White;
    }

    public CharakterMenu(Charakter player)
    {
        HandleInput(player);
    }

    private void HandleInput(Charakter player)
    {
        Menu nextMenu;
        string input;
        bool validInput = false;

        while (true)
        {
            Console.Write("> ");
            input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    if (!player.IsLoadedFromFile)
                        InitializePlayer(player, new Warrior());
                    nextMenu = new FightMenu(player);
                    validInput = true;
                    break;
                case "2":
                    if (!player.IsLoadedFromFile)
                        InitializePlayer(player, new Mage());
                    nextMenu = new FightMenu(player);
                    validInput = true;
                    break;
                case "3":
                    validInput = true;
                    break;
                default:
                    validInput = false;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: Falscher Input");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
            if (validInput)
                break;
        }
    }

    private void InitializePlayer(Charakter player, Class @class)
    {
        player.Name = "Aria";
        player.Class = @class;
        player.Inventory = @class.Inventory;
        player.MaxHealth = player.GetMaxHealthValue();
        player.CurrentHealth = player.MaxHealth;
    }
}