﻿using Game.Charakters;
using Game.Combat;

namespace Game.Items;

public abstract class Item
{
    public abstract string Name { get; set; }
    public abstract string Description { get; set; }
    public abstract int Count { get; set; }
    public abstract double Value { get; set; }

    public abstract void UseItem(Charakter player, Charakter enemy, out bool noItemUsed);

    public Item(string name, int count, double value)
    {
        Name = name;
        Count = count;
        Value = value;
    }
}