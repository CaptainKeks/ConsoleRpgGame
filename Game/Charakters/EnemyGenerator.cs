
using Game.Combat;

namespace Game.Charakters;

class EnemyGenerator
{
    public List<Charakter> Enemies { get; private set; } = [];
    public EnemyGenerator(Class @class, int amount, CombatValues cmb)
    {
        for (int i = 0; i < amount; i++)
        {
            Charakter enemie = new Charakter();
            InitializeEnemie(enemie, @class, cmb);
            Enemies.Add(enemie);
        }
    }

    private void InitializeEnemie(Charakter enemie, Class @class, CombatValues cmb)
    {
        Random rnd = new Random();
        enemie.Name = "Orga";
        enemie.Class = @class;
        enemie.MaxHealth = (enemie.GetMaxHealthValue() - 8) + (4 * cmb.Level);
        enemie.CurrentHealth = enemie.MaxHealth;
        enemie.Class.AttackModifier = rnd.Next((int)(enemie.Class.AttackModifier + (cmb.Level * 1.5) - 2), (int)(enemie.Class.AttackModifier + (cmb.Level * 1.5) + 2));
    }
}