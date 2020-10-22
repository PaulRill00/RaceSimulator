using System.Collections.Generic;

namespace Model
{
    public class Competition
    {
        public List<IParticipant> Participants { get; set; } = new List<IParticipant>();
        public Queue<Track> Tracks { get; set; } = new Queue<Track>();
        public RaceInfo<DriverPoints> Points { get; set; } = new RaceInfo<DriverPoints>();
        public RaceInfo<DriverTime> SectionTimes { get; set; } = new RaceInfo<DriverTime>();
        public RaceInfo<DriverBroken> BrokenTimes { get; set; } = new RaceInfo<DriverBroken>();
        public RaceInfo<DriverTopSpeed> TopSpeeds { get; set; } = new RaceInfo<DriverTopSpeed>();

        public Track NextTrack()
        {
            if (Tracks.TryDequeue(out Track track))
                return track;
            return null;
        }

        public void SetPoints(List<DriverPoints> points)
        {
            foreach(DriverPoints point in points)
            {
                Points.AddToList(point);                    
            }
        }
    }
}
