namespace Game.Charakters
{
    public enum ItemType
    {
        HeilTrank,
        Stab
    }

    public class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ItemType Type { get; set; }
        public int Count { get; set; }
        public int Value { get; set; }

        public Item(string name, string description, ItemType type, int count, int value)
        {
            Name = name;
            Description = description;
            Type = type;
            Count = count;
            Value = value;
        }
    }
}