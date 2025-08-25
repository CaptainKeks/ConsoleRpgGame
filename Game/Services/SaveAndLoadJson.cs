using Game.Charakters;
using Game.Combat;
using Newtonsoft.Json;

namespace Game.Helper;

static class SaveAndLoadJson
{
    public static void SaveGame(Charakter player)
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };
        string text = JsonConvert.SerializeObject(player, settings);
        File.WriteAllText(AppContext.BaseDirectory + "savegame.json", text);
    }

    public static void SaveFight(CombatValues cmb)
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };
        string text = JsonConvert.SerializeObject(cmb, settings);
        File.WriteAllText(AppContext.BaseDirectory + "saveFight.json", text);
    }

    public static CombatValues LoadFight()
    {
        try
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
            var cmb = new CombatValues();
            string text = File.ReadAllText(AppContext.BaseDirectory + "saveFight.json");
            cmb = JsonConvert.DeserializeObject<CombatValues>(text, settings);
            return cmb;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new CombatValues();
        }
    }

    public static Charakter LoadGame()
    {
        try
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
            var player = new Charakter();
            string text = File.ReadAllText(AppContext.BaseDirectory + "savegame.json");
            player = JsonConvert.DeserializeObject<Charakter>(text, settings);
            return player;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new Charakter();
        }
    }
}