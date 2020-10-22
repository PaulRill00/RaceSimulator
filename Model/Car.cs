namespace Model
{
    public class Car : IEquipment
    {
        public int Quality { get; set; }
        public int Performance { get; set; }
        public int Speed => Performance * Quality;
        public bool IsBroken { get; set; }
    }
}
