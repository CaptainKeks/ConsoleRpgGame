

namespace Game.Charakters;

class MetaProgression
{
    public double Attack { get; set; }
    public double Defense { get; set; }
    public double Wisdom { get; set; }
    public double Gold { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public double Price { get; set; }

    public MetaProgression(double attack, double defense, double wisdom, double gold, int wins, int losses, double price)
    {
        Attack = attack;
        Defense = defense;
        Wisdom = wisdom;
        Gold = gold;
        Wins = wins;
        Losses = losses;
        Price = price;
    }
}
