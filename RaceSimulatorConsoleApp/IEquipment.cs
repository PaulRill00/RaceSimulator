namespace Model
{
    public interface IEquipment
    {
        int Quality { get; set; }
        int Performance { get; set; }
        int Speed { get; set; }
        bool IsBroken { get; set; }

    }
}
