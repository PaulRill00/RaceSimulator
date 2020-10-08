namespace Model
{
    public interface IEquipment
    {
        int Quality { get; set; }
        int Performance { get; set; }
        public int Speed => Performance * Quality;
        bool IsBroken { get; set; }
    }
}
