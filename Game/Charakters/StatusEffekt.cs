
namespace Game.Charakters;

public class StatusEffekt
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Duration { get; set; }
    public double Value { get; set; }

    public StatusEffekt(string name, string description, int duration, double value)
    {
        Name = name;
        Description = description;
        Duration = duration;
        Value = value;
    }

    public void ApplyStatusAffect(Charakter attacker, Charakter defender)
    {
        if (Duration < 1)
            return;
        double damage = defender.CurrentHealth > Value ? Value : defender.CurrentHealth;
        defender.CurrentHealth -= damage;
        Console.Write($"Ich {attacker.Name} habe {defender.Name} ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($"{damage}");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($" Giftschaden hinzugefügt. ({Duration})");
        Console.WriteLine();
        Duration--;
    }

    public override string ToString()
    {
        return Name + " (" + Duration + ")" + " mit " + Value + " Schaden.";
    }
}