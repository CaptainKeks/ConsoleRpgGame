
using Game.Charakters;

namespace Game.Items;

class GiftTrank : Item
{
    public GiftTrank(string name, int count, int duration, double value) : base(name, count, value)
    {
        Duration = duration;
    }

    public static int Duration { get; set; }
    public override string Name { get; set; }
    public override string Description { get; set; } = $"Ein Grün Blubberndes Getränk das 5 Leben pro Runde schaden macht für {Duration} Runden.";
    public override int Count { get; set; }
    public override double Value { get; set; }

    public override void UseItem(Charakter player, Charakter enemy, out bool noItemUsed)
    {
        // MetaProgression Werte addieren
        var value = Value + player.MetaProgression.PoisonPotion;
        if (Count > 0)
        {
            if (enemy.StatusEffekts.Count < 1)
            {
                noItemUsed = false;
                enemy.StatusEffekts.Add(new StatusEffekt("Gift", "Fügt jede Runde dem Gegner 5 Schaden zu kann gestäckt werden.", Duration, 5));
                Count--;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Wirke GiftTrank auf {enemy.Name} für {Duration} Runden.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.ReadKey();
            }
            else
            {
                var query = enemy.StatusEffekts.Where(e => e.Name == "Gift");
                foreach (var effekt in query)
                    effekt.Value += Value;
                noItemUsed = false;
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Du kannst niemanden vergiften du hast keine Tränke mehr.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey();
            noItemUsed = true;
        }
    }
}