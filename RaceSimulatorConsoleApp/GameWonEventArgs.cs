using System;

namespace Model
{
    public class GameWonEventArgs : EventArgs
    {
        public IParticipant Winner { get; set; }
        public Track Track { get; set; }
    }
}
