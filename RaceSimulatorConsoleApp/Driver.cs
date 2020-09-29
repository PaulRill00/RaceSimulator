using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Driver : IParticipant
    {

        private string _name;
        public string Name { 
            get {
                return Equipment.IsBroken ? "x" : _name;
            }
            set
            {
                _name = value;
            }
        }
        public int Points { get; set; } = 0;
        public IEquipment Equipment { get; set; }
        public TeamColors TeamColor { get; set; }
        public int DistanceTraveled { get; set; } = -10;
        public int CurrentRound { get; set; } = 0;

        public Driver(string name, TeamColors teamColor)
        {
            Name = name;
            TeamColor = teamColor;
            Equipment = new Car();
        }
    }
}
