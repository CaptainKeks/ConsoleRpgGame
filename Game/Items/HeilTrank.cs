using Game.Charakters;

namespace Game.Items
{
    class HeilTrank : Item
    {
        public HeilTrank(string name, int count, double value) : base(name, count, value) { }

        public override string Name { get; set; } = "Heiltrank";
        public override string Description { get; set; } = "Ein Rotes Blubberndes Getränk das 30 Leben wiederherstellt.";
        public override int Count { get; set; } = 3;
        public override double Value { get; set; } = 30;

        public override void UseItem(Charakter player, Charakter enemy, out bool noItemUsed)
        {
            // MetaProgression werte addieren
            var value = Value + player.MetaProgression.HealthPotion;
            double healed = player.MaxHealth - player.CurrentHealth > value ? value : player.MaxHealth - player.CurrentHealth;
            if (Count > 0)
            {
                if (player.CurrentHealth != player.MaxHealth)
                {
                    player.CurrentHealth += healed;
                    Count--;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"Heiltrank hat mich um {healed:F2} geheilt.");
                    Console.ForegroundColor = ConsoleColor.White;
                    noItemUsed = false;
                    Console.ReadKey();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Du kannst dich nicht Heilen du hast schon volles Leben!");
                    Console.ForegroundColor = ConsoleColor.White;
                    noItemUsed = true;
                    Console.ReadKey();
                }
            }
            else
            {
                noItemUsed = true;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Du kannst dich nicht Heilen du hast keine Heiltränke mehr!");
                Console.ForegroundColor = ConsoleColor.White;
                Console.ReadKey();
            }
        }
    }
}