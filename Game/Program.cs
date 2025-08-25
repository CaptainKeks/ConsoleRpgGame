
using Game.Charakters;
using Game.Combat;
using Game.Helper;
using Game.Menus;

class Programm
{
    public static void Main()
    {
        var player = SaveAndLoadJson.LoadGame();
        Menu startMenu = new StartMenu(player);
        HandleInput(player);
    }

    public static void HandleInput(Charakter player)
    {
        Charakter newPlayer = new Charakter();
        CombatValues cmb = new CombatValues();
        newPlayer.MetaProgression = player.MetaProgression;
        newPlayer.CurrentHealth = newPlayer.MaxHealth;
        List<Charakter> enemies;
        string input;
        while (true)
        {
            Console.Write("> ");
            input = Console.ReadLine();
            bool validInput = false;
            switch (input)
            {
                case "1":
                    validInput = true;
                    Menu nextMenu = new CharakterMenu(newPlayer);
                    break;
                case "2":
                    player = SaveAndLoadJson.LoadGame();
                    cmb = SaveAndLoadJson.LoadFight();
                    player.IsLoadedFromFile = true;
                    nextMenu = new FightMenu(player, cmb);
                    validInput = true;
                    break;
                case "3":
                    nextMenu = new UpgradeMenu(player);
                    validInput = true;
                    break;
                case "4":
                    SaveAndLoadJson.SaveGame(player);
                    Environment.Exit(0);
                    validInput = true;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: Falscher Input");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
            if (validInput)
                break;
        }
    }
}