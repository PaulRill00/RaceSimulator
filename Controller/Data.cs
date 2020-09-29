using System;
using System.Collections.Generic;
using System.Text;
using Model;

namespace Controller
{
    public static class Data
    {
        public static int SectionLength { get; private set; } = 100;
        public static int RoundsToWin { get; private set; } = 2;

        public static Competition Competition { get; set; }
        public static Race CurrentRace { get; set; }

        public static event EventHandler NextTrack;

        public static void Initialize()
        {
            Competition = new Competition();
            AddParticipants();
            AddTracks();
        }

        public static void AddParticipants()
        {
            Competition.Participants.AddRange(new List<IParticipant> {
                new Driver("a", TeamColors.Red),
                new Driver("b", TeamColors.Yellow),
                new Driver("c", TeamColors.Green),
                new Driver("d", TeamColors.Blue),
                new Driver("e", TeamColors.Grey)
            });
        }

        public static void AddTracks()
        {
            Competition.Tracks.Enqueue(new Track("track01", new SectionTypes[] { SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.Finish, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.LeftCorner, SectionTypes.RightCorner }));

            Track elburg = new Track("Circuit Elburg", new SectionTypes[] { SectionTypes.StartGrid, SectionTypes.Finish, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.StartGrid, SectionTypes.StartGrid });
            Competition.Tracks.Enqueue(elburg);
        }

        public static void NextRace()
        {
            Track track = Competition.NextTrack();
            if (track != null)
            {
                CurrentRace = null;
                CurrentRace = new Race(track, Competition.Participants);
                NextTrack?.Invoke(null, new EventArgs());

                CurrentRace.Start();
            }
        }
    }
}
