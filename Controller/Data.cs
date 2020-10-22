using System;
using System.Collections.Generic;
using Model;

namespace Controller
{
    public static class Data
    {
        public static int SectionLength { get; } = 100;
        public static int RoundsToWin { get; } = 2;

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
                new Driver("red", TeamColors.Red),
                new Driver("yellow", TeamColors.Yellow),
                new Driver("green", TeamColors.Green),
                new Driver("blue", TeamColors.Blue),
                new Driver("grey", TeamColors.Grey)
            });
        }

        public static void AddTracks()
        {
            Competition.Tracks.Enqueue(new Track("track01", new [] { SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.Finish, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.LeftCorner, SectionTypes.RightCorner }));

            Track elburg = new Track("Circuit Elburg", new [] { SectionTypes.StartGrid, SectionTypes.Finish, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.StartGrid, SectionTypes.StartGrid });
            Competition.Tracks.Enqueue(elburg);

            Track oostendorp = new Track("Oostendorp",
                new [] { SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.StartGrid,SectionTypes.Finish, SectionTypes.Straight,
                    SectionTypes.LeftCorner, SectionTypes.Straight,SectionTypes.Straight,SectionTypes.Straight,SectionTypes.RightCorner,SectionTypes.Straight,SectionTypes.Straight,SectionTypes.RightCorner,SectionTypes.Straight, SectionTypes.RightCorner,
                    SectionTypes.Straight,SectionTypes.Straight,SectionTypes.Straight,SectionTypes.Straight,SectionTypes.Straight,SectionTypes.Straight,SectionTypes.Straight,SectionTypes.Straight,SectionTypes.Straight,
                    SectionTypes.LeftCorner,SectionTypes.Straight,SectionTypes.Straight,SectionTypes.Straight,SectionTypes.Straight, SectionTypes.LeftCorner,SectionTypes.Straight,SectionTypes.Straight,SectionTypes.Straight, SectionTypes.RightCorner,
                    SectionTypes.RightCorner, SectionTypes.LeftCorner, SectionTypes.LeftCorner, SectionTypes.RightCorner, SectionTypes.RightCorner,SectionTypes.Straight,SectionTypes.Straight,SectionTypes.Straight,SectionTypes.Straight,SectionTypes.Straight,SectionTypes.Straight,
                    SectionTypes.RightCorner,SectionTypes.Straight,SectionTypes.Straight,SectionTypes.Straight,SectionTypes.Straight,SectionTypes.Straight, SectionTypes.RightCorner,SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight});
            Competition.Tracks.Enqueue(oostendorp);
        }

        public static void NextRace()
        {
            var track = Competition.NextTrack();
            CurrentRace = null;

            if (track == null) return;

            CurrentRace = new Race(track, Competition.Participants);
            NextTrack?.Invoke(null, new EventArgs() );

            CurrentRace.Start();
        }
    }
}
