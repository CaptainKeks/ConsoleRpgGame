
using Game.Helper;
using System;
using System.ComponentModel.Design;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;
using System.Security;
using System.Security.AccessControl;
using System.Threading;

namespace Game.Charakters;

public enum BaseValue
{
    Attack,
    Defense,
    Wisdom
}

class Charakter : IEntity
{
    public Charakter() { }
    public string Name { get; set; }
    public double BaseAttack { get; set; } = 4;
    public double BaseDefence { get; set; } = 3;
    public double BaseWisdom { get; set; } = 0;
    public double MaxHealth { get; set; } = 30;
    public double CurrentHealth { get; set; }
    public bool InDefensePosition { get; set; } = false;
    public Class Class { get; set; }
    public bool IsLoadedFromFile { get; set; } = false;
    public List<Item> Inventory { get; set; } = [];
    public MetaProgression MetaProgression { get; set; } = new MetaProgression(0, 0, 0, 0, 0, 0, 20);

    /// <summary>
    /// Attackiert den mitgegeben Charakter und zieht die Defence von der Attacke ab.
    /// </summary>
    /// <param name="defender"></param>
    public void Attack(Charakter defender)
    {
        var realDamage = GetAttackValue() - defender.GetDefenceValue();
        realDamage = defender.InDefensePosition ? realDamage / 2 : realDamage;
        realDamage = realDamage < 0 ? 0 : realDamage;
        defender.CurrentHealth -= realDamage;
        defender.CurrentHealth = defender.CurrentHealth < 0 ? 0 : defender.CurrentHealth;
        Console.WriteLine($"Ich {Name} greife mit {GetAttackValue():F2} AttackDamage {defender.Name} an!");
        PrintAttackMessage(defender, realDamage);
    }

    public void SpecialAttack(Charakter defender)
    {
        var realDamage = (GetSpecialAttackValue() - defender.GetDefenceValue());
        realDamage = defender.InDefensePosition ? realDamage / 2 : realDamage;
        realDamage = realDamage < 0 ? 0 : realDamage;
        defender.CurrentHealth -= realDamage;
        defender.CurrentHealth = defender.CurrentHealth < 0 ? 0 : defender.CurrentHealth;
        Console.WriteLine($"Ich {Name} führe meine SPEZIALATTACKE aus auf {defender.Name} mit {GetSpecialAttackValue():F2} AttackDamage!");
        PrintAttackMessage(defender, realDamage);
    }
    public void GetInDefensePosition()
    {
        InDefensePosition = true;
        Console.WriteLine($"Ich {Name} gehe in die AbwehrPosition und bekomme bei meinem nächsten Angriff nur 50% Schaden.");
    }

    private void PrintAttackMessage(Charakter defender, double realDamage)
    {
        Console.Write($"{defender.Name} bekommt ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"{realDamage:F2} ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("AttackDamage!");
        Console.WriteLine();
    }

    public double GetAttackValue()
    {
        return (BaseAttack + MetaProgression.Attack + Class.AttackModifier + CurrentHealth * 0.1) * GetWisdomValue();
    }

    public double GetSpecialAttackValue()
    {
        return (BaseAttack + MetaProgression.Attack + Class.SpecialAttackModifier + CurrentHealth * 0.1) * GetWisdomValue();
    }

    public double GetDefenceValue()
    {
        return (BaseDefence + MetaProgression.Defense + Class.DefenceModifier) * GetWisdomValue();
    }

    public double GetMaxHealthValue()
    {
        return (MaxHealth + Class.HealthModifier) * GetWisdomValue();
    }
    public double GetWisdomValue()
    {
        return (BaseWisdom + MetaProgression.Wisdom + Class.WisdomModifier);
    }

    public void UseItem(Item item, out bool noItemUsed)
    {
        noItemUsed = true;
        double healed = MaxHealth - CurrentHealth > item.Value ? item.Value : MaxHealth - CurrentHealth;
        if (item.Type == ItemType.HeilTrank)
        {
            if (item.Count > 0)
            {
                if (CurrentHealth != MaxHealth)
                {
                    CurrentHealth += healed;
                    item.Count--;
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
            }
        }
    }

    public void UpgradeBaseValue(BaseValue baseValue)
    {
        if (MetaProgression.Gold < MetaProgression.Price)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Du hast nicht genügend Gold");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey();
            return;
        }

        switch (baseValue)
        {
            case BaseValue.Attack:
                UpgradeBaseValues(MetaProgression, m => m.Attack, (m, v) => m.Attack = v, "Attack");
                Console.ReadKey();
                break;
            case BaseValue.Defense:
                UpgradeBaseValues(MetaProgression, m => m.Defense, (m, v) => m.Defense = v, "Defense");
                Console.ReadKey();
                break;
            case BaseValue.Wisdom:
                UpgradeBaseValues(MetaProgression, m => m.Wisdom, (m, v) => m.Wisdom = v, "Wisdom");
                Console.ReadKey();
                break;
            default:
                break;
        }
    }

    private void UpgradeBaseValues(MetaProgression meta, Func<MetaProgression, double> getter, Action<MetaProgression, double> setter, string label)
    {
        var current = getter(meta);
        setter(meta, current + 1);
        meta.Gold -= meta.Price;
        meta.Price += 15;
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"Deine {label} wurde auf {getter(meta)} erhöht.");
        Console.ForegroundColor = ConsoleColor.White;
    }
}