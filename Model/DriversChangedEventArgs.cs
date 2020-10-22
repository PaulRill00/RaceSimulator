using System;

namespace Model
{
    public class DriversChangedEventArgs : EventArgs
    {
        public Track Track { get; set; }
        public RaceInfo<DriverPoints> Points { get; set; }
    }
}
