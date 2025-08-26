
namespace Game.Charakters;

interface IEntity
{
    string Name { get; set; }
    double BaseAttack { get; set; }
    double BaseDefence { get; set; }
    double BaseWisdom { get; set; }
    double CurrentHealth { get; set; }
    bool IsLoadedFromFile { get; set; }
    double MaxHealth { get; set; }
    bool InDefensePosition { get; set; }
    Class Class { get; set; }
    List<Item> Inventory { get; set; }
    MetaProgression MetaProgression { get; set; }

    void Attack(Charakter defender);
    double GetAttackValue();
    double GetDefenseValue();
    double GetMaxHealthValue();
    double GetSpecialAttackValue();
    double GetWisdomValue();
    void SpecialAttack(Charakter defender);
    void UpgradeBaseValue(BaseValue baseValue);
    void UseItem(Item item, out bool noItemUsed);
}