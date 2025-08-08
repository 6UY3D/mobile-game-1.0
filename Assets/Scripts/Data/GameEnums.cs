// Filename: GameEnums.cs
namespace IdleShopkeeping.Data
{
    /// <summary>
    /// Defines the different moods a record can impart on the store.
    /// </summary>
    public enum StoreMood
    {
        None,
        Chill,
        Upbeat,
        Spooky,
        Nostalgic,
        Energetic,
        Melancholy
    }

    /// <summary>
    /// Defines the type of an item for categorization.
    /// </summary>
    public enum ItemType
    {
        Decoration,
        Plant,
        Record
    }
}
