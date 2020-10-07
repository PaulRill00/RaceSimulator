namespace Model
{
    public interface IParticipant
    {
        string Name { get; set; }
        int Points { get; set; }
        IEquipment Equipment { get; set; }
        TeamColors TeamColor { get; set; }
    }

    public enum TeamColors
    {
        Red,
        Green,
        Yellow,
        Grey,
        Blue
    }
}
