
namespace Game.Charakters
{
    internal interface IEntity
    {
        double BaseAttack { get; set; }
        double BaseDefence { get; set; }
        double BaseWisdom { get; set; }
        Class Class { get; set; }
        double CurrentHealth { get; set; }
        List<Item> Inventory { get; set; }
        bool IsLoadedFromFile { get; set; }
        double MaxHealth { get; set; }
        MetaProgression MetaProgression { get; set; }
        string Name { get; set; }

        void Attack(Charakter defender);
        double GetAttackValue();
        double GetDefenceValue();
        double GetMaxHealthValue();
        double GetSpecialAttackValue();
        double GetWisdomValue();
        void SpecialAttack(Charakter defender);
        void UpgradeBaseValue(BaseValue baseValue);
        void UseItem(Item item, out bool noItemUsed);
    }
}