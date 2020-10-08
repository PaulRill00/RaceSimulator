namespace Model
{
    public class Driver : IParticipant
    {

        private string _name;
        private int _fireCount;
        public string Name { 
            get => Equipment.IsBroken ? $"{_name} (Broken)" : _name;
            set => _name = value;
        }
        public int Points { get; set; } = 0;
        public IEquipment Equipment { get; set; }
        public TeamColors TeamColor { get; set; }
        public int DistanceTraveled { get; set; }
        public int CurrentRound { get; set; }
        public bool Placed { get; set; }

        public int FireCount
        {
            get => _fireCount;
            set
            {
                if (value > 0 && value < 5)
                    _fireCount = value;
                else
                    _fireCount = 0;
            }
        }

        public Driver(string name, TeamColors teamColor)
        {
            Name = name;
            TeamColor = teamColor;
            Equipment = new Car();
            Reset();
        }

        public void Reset()
        {
            CurrentRound = 0;
            DistanceTraveled = -10;
            Placed = false;
        }
    }
}
