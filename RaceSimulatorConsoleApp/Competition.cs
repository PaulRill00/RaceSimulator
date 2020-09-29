using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Competition
    {
        public List<IParticipant> Participants { get; set; } = new List<IParticipant>();
        public Queue<Track> Tracks { get; set; } = new Queue<Track>();
        public RaceInfo<DriverPoints> Points { get; set; } = new RaceInfo<DriverPoints>();

        public Track NextTrack()
        {
            if (Tracks.TryDequeue(out Track track))
                return track;
            else
                return null;
        }

        public void SetPoints(List<DriverPoints> points)
        {
            foreach(DriverPoints point in points)
            {
                DriverPoints _current = GetPoints(point.Name);
                if (_current != null)
                    _current.Points = point.Points;
                else
                    Points.AddToList(point);
            }
        }

        public DriverPoints GetPoints(string name)
        {
            return Points.GetList().Find(x => x.Name == name);
        }
    }
}
