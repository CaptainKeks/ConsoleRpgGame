using Game.Charakters;
using Game.Combat;
using Game.Helper;
using Game.Items;
using Game.Menus.FinishMenus;
using System.Numerics;

namespace Game.Menus;
class FightMenu : Menu
{
    private CombatValues cmb = new CombatValues();
    public override void DisplayMenu() { }

    private void DisplayMenu(Charakter player, List<Charakter> enemies)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Kampf beginnt:");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("--------------");
        Console.WriteLine();
        Console.WriteLine($"Spieler: {player.Name} ({player.Class.ClassName}) HP: {player.CurrentHealth:F2}/{player.MaxHealth:F2}");

        foreach (var enemie in enemies)
            Console.WriteLine($"Gegner: {enemie.Name} ({enemie.Class.ClassName}) HP: {enemie.CurrentHealth:F2}/{enemie.MaxHealth:F2}");

        Console.WriteLine();
        Console.WriteLine("----------");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"Gold: {player.MetaProgression.Gold}");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("----------");
        Console.WriteLine();
    }
    public FightMenu(Charakter player)
    {
        Random rnd = new Random();
        cmb.EnemyCount = rnd.Next(1, 3);
        bool isFinished = false;
        bool isLevelFinished = true;
        List<Charakter> newEnemies = [];
        while (!isFinished)
        {
            if (isLevelFinished)
            {
                var generatedEnemies = new EnemyGenerator(new Ork(), cmb.EnemyCount, cmb);
                newEnemies = generatedEnemies.Enemies;
            }
            isLevelFinished = false;
            cmb.Turn = 0;
            cmb.Turn++;
            cmb.Round++;
            FightLevel(player, newEnemies, cmb, out isFinished, out isLevelFinished);
            if (isLevelFinished)
            {
                cmb.Level++;
                cmb.Turn = 0;
                cmb.Round = 0;
            }
        }
    }
    public FightMenu(Charakter player, CombatValues newCmb)
    {
        this.cmb = newCmb;
        bool isFinished = false;
        bool isLevelFinished = true;
        List<Charakter> newEnemies = [];
        while (!isFinished)
        {
            if (isLevelFinished)
            {
                Random rnd = new Random();
                var generatedEnemies = new EnemyGenerator(new Ork(), cmb.EnemyCount, cmb);
                newEnemies = generatedEnemies.Enemies;
            }
            isLevelFinished = false;
            cmb.Turn = 0;
            cmb.Turn++;
            FightLevel(player, newEnemies, cmb, out isFinished, out isLevelFinished);
            if (isLevelFinished)
            {
                cmb.Level++;
                cmb.Turn = 0;
                cmb.Round = 0;
            }
        }
    }

    private void FightLevel(Charakter player, List<Charakter> enemies, CombatValues cmb, out bool isFinished, out bool isLevelFinished)
    {
        DisplayMenu(player, enemies);
        PrintMenuRoundAndTurn(cmb);
        PrintStatusEffekts(player, enemies);
        PrintMenuAndPlayerMove(player, enemies);
        (isFinished, isLevelFinished) = TryIfHealthIsZero(player, enemies, cmb);
        Console.Clear();

        DisplayMenu(player, enemies);
        cmb.Turn++;
        PrintMenuRoundAndTurn(cmb);
        PrintMenuAndEnemyMove(player, enemies);
        (isFinished, isLevelFinished) = TryIfHealthIsZero(player, enemies, cmb);
        player.InDefensePosition = false;
        SaveAndLoadJson.SaveGame(player);
        Console.ReadKey();
        Console.Clear();
    }

    private void PrintStatusEffekts(Charakter player, List<Charakter> enemies)
    {
        foreach (var enemy in enemies)
        {
            foreach (StatusEffekt effekt in player.StatusEffekts)
                effekt.ApplyStatusAffect(enemy, player);

            foreach (StatusEffekt effekt in enemy.StatusEffekts)
                effekt.ApplyStatusAffect(player, enemy);
        }
    }

    private void PrintMenuRoundAndTurn(CombatValues cmb)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"Level: {cmb.Level}");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("---------");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"Runde: {cmb.Round} Zug: {cmb.Turn}                          ");
        Console.ForegroundColor = ConsoleColor.White;
    }
    private void PrintMenuAndEnemyMove(Charakter player, List<Charakter> enemies)
    {
        Console.WriteLine("Der/Die Gegner ist/sind am Zug.");
        Console.WriteLine();
        foreach (var enemie in enemies)
            enemie.Attack(player);
        if (player.StatusEffekts.Count < 1)
            player.StatusEffekts.Add(new StatusEffekt("Gift", "Fügt jede Runde dem Gegner 5 Schaden zu kann gestäckt werden.", 1, 5));
        Console.Write("Drücke [Enter] für den nächsten Zug.");
    }

    private void PrintMenuAndPlayerMove(Charakter player, List<Charakter> enemies)
    {
        Console.WriteLine("Dein Zug. Wähle eine Aktion: ");
        Console.WriteLine("[1] Angreifen");
        Console.WriteLine("[2] Spezialattacke/Zauberspruch");
        Console.WriteLine("[3] Item benutzen");
        Console.WriteLine("[4] Verteidigen");
        Console.WriteLine("[5] Fliehen");
        HandleInput(player, enemies);
    }

    private (bool isPlayerFinished, bool isLevelFinished) TryIfHealthIsZero(Charakter player, List<Charakter> enemies, CombatValues cmb)
    {
        int maxRound = 3;
        foreach (var enemy in enemies)
            if (enemy.CurrentHealth <= 0)
            {
                player.MetaProgression.Gold += 12;
                enemies.Remove(enemy);
                break;
            }

        bool playerLooses = player.CurrentHealth <= 0;
        bool playerLevelWin = enemies.Count == 0;
        bool playerWins = playerLevelWin && cmb.Level == maxRound;

        if (playerWins)
        {
            player.MetaProgression.Wins += 1;
            SaveAndLoadJson.SaveGame(player);
            Menu nextMenu = new WinnerMenu();
            return (true, true);
        }

        if (playerLevelWin)
            return (false, true);

        if (playerLooses)
        {
            player.MetaProgression.Losses += 1;
            SaveAndLoadJson.SaveGame(player);
            Menu nextMenu = new LoosingMenu();
            return (true, true);
        }
        else
            return (false, false);
    }

    private void HandleInput(Charakter player, List<Charakter> enemies)
    {
        bool validInput = false;
        string input;
        while (!validInput)
        {
            Console.Write("> ");
            input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    player.Attack(enemies.Last());
                    Console.Write("Drücke [Enter] für den nächsten Zug.");
                    Console.ReadKey();
                    validInput = true;
                    break;
                case "2":
                    player.SpecialAttack(enemies.Last());
                    Console.Write("Drücke [Enter] für den nächsten Zug.");
                    Console.ReadKey();
                    validInput = true;
                    break;
                case "3":
                    Menu nextMenu = new UseItemMenu(player, enemies.Last(), out bool noItemUsed);
                    DisplayMenu(player, enemies);
                    if (noItemUsed)
                    {
                        PrintMenuRoundAndTurn(cmb);
                        PrintMenuAndPlayerMove(player, enemies);
                    }
                    validInput = true;
                    break;
                case "4":
                    player.GetInDefensePosition();
                    validInput = true;
                    break;
                case "5":
                    SaveAndLoadJson.SaveGame(player);
                    SaveAndLoadJson.SaveFight(cmb);
                    nextMenu = new StartMenu(player);
                    Programm.Main();
                    Console.ReadKey();
                    validInput = true;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: Falscher Input");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
        }
    }
}