using System;
using System.Collections.Generic;
using System.Text;
using Model;
using Controller;

namespace RaceSimulator
{
    public class TrackVisuals
    {
        #region graphics

        public string[] _finishHorizontal = { "----", " 2+ ", " 1+ ", "----" };
        public string[] _finishVertical = { "|  |", "|21|", "|++|", "|  |" };

        public string[] _straightHorizontal = { "----", " 1  ", " 2  ", "----" };
        public string[] _straightVertical = { "|  |", "|21|", "|  |", "|  |" };

        public string[] _startHorizontal = { "----", "  #1", "#2  ", "----" };
        public string[] _startVertical = { "|# |", "|1 |", "| #|", "| 2|" };

        public string[] _turnLeftUp = { "/  |", " 2 |", "  1/", @"--/ " };
        public string[] _turnLeftDown = { @"--\ ", @"  2\", " 1 |", @"\  |" };
        public string[] _turnRightUp = { @"|  \", "| 2 ", @"\1  ", @" \--" };
        public string[] _turnRightDown = { " /--", "/2  ", "| 1 ", "|  /" };

        #endregion

        #region settings

        public static int visualWidth  = 4;
        public static int visualHeight = 4;

        #endregion

        private bool trackLoaded = false;

        private static TrackVisuals _instance;
        public static TrackVisuals Instance
        {
            get
            {
                if (_instance == null)
                    new TrackVisuals();
                return _instance;
            }
        }

        public TrackVisuals()
        {
            _instance = this;
            Data.NextTrack += OnNextTrack;
        }

        public void InitializeTrack(Track track)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Data.CurrentRace.CalculateCoords();
            Data.CurrentRace.ApplyOffset();
            Console.Write(track.Name);
            Console.CursorVisible = false;
            trackLoaded = true;
        }

        public void DrawTrack(Track track)
        {
            if (!trackLoaded)
                return;

            int[] offset = GetOffset(track.Sections);

            foreach (Section section in track.Sections)
            {
                string[] visualSection = GetSectionVisual(section.Direction, section.SectionType);
                int startX = section.X * visualWidth        + offset[0];
                int startY = section.Y * visualHeight + 1   + offset[1];

                SectionData sectionData = Data.CurrentRace.GetSectionData(section);

                for(int index = 0; index < visualSection.Length; index++)
                {
                    Console.SetCursorPosition(startX, startY + index);

                    PrintSectionWithParticipants(visualSection[index], sectionData);
                }
            }

            Console.SetCursorPosition(0, 1);
        }
        public int[] GetOffset(LinkedList<Section> sections)
        {
            int minX = 0;
            int minY = 0;

            foreach (Section section in sections)
            {
                minX = section.X < minX ? section.X : minX;
                minY = section.Y < minY ? section.Y : minY;
            }

            minX = Math.Abs(minX);
            minY = Math.Abs(minY);

            return new int[] { minX * visualWidth, minY * visualHeight };
        }

        public void DrawScoreboard(RaceInfo<DriverPoints> data)
        {
            int x = 60;
            int y = 0;
            Console.SetCursorPosition(x, y);
            Console.Write("Points scoreboard:");

            foreach (DriverPoints points in data.GetList())
            {
                y++;
                Console.SetCursorPosition(x,y);
                Console.Write($"{points.Name,-10}: {points.Points,-3}");
            }
        }

        public void PrintSectionWithParticipants(string section, SectionData data)
        {
            char[] chars = section.ToCharArray();

            foreach(char c in chars) {

                string row = c.ToString();

                if (c.ToString().Equals("1"))
                {
                    row = row.Replace("1", data.Left == null ? " " : data.Left.Name);
                    Console.ForegroundColor = GetParticipantColor(data.Left);
                }

                if (c.ToString().Equals("2"))
                {
                    row = row.Replace("2", data.Right == null ? " " : data.Right.Name);
                    Console.ForegroundColor = GetParticipantColor(data.Right);
                }

                Console.Write(row);
                Console.ResetColor();
            }
        }

        public ConsoleColor GetParticipantColor(IParticipant participant)
        {
            if (participant != null)
            {
                switch (participant.TeamColor)
                {
                    case TeamColors.Blue:
                        return ConsoleColor.Blue;
                    case TeamColors.Green:
                        return ConsoleColor.Green;
                    case TeamColors.Red:
                        return ConsoleColor.Red;
                    case TeamColors.Yellow:
                        return ConsoleColor.Yellow;
                }
            }
            return ConsoleColor.White;
        }

        public string[] GetSectionVisual(int[] direction, SectionTypes type)
        {
            switch (type)
            {
                case SectionTypes.Finish:
                    {
                        if(direction[0] != 0)
                        {
                            return direction[0] > 0 ? FlipHorizontal(_finishHorizontal) : _finishHorizontal;
                        }
                        else
                        {
                            return direction[1] > 0 ? FlipVertical(_finishVertical) : _finishVertical;
                        }
                    }
                case SectionTypes.LeftCorner:
                    {
                        if (direction[0] == -1)
                            return _turnRightDown;

                        if (direction[0] == 1)
                            return _turnLeftUp;

                        if (direction[1] == 1)
                            return _turnRightUp;

                        return _turnLeftDown;
                    }
                case SectionTypes.RightCorner:
                    {
                        if (direction[0] == 1)
                            return _turnLeftDown;

                        if (direction[0] == -1)
                            return _turnRightUp;

                        if (direction[1] == 1)
                            return _turnLeftUp;

                        return _turnRightDown;
                    }
                case SectionTypes.StartGrid:
                    {
                        if (direction[0] != 0)
                        {
                            return direction[0] > 0 ? FlipHorizontal(_startHorizontal) : _startHorizontal;
                        }
                        else
                        {
                            return direction[1] > 0 ? FlipVertical(_startVertical) : _startVertical;
                        }
                    }
                case SectionTypes.Straight:
                    {
                        if (direction[0] != 0)
                            return _straightHorizontal;
                        return _straightVertical;
                    }
            }
            return null;
        }

        public string[] FlipHorizontal(string[] array)
        {
            string[] result = new string[array.Length];

            for (int index = 0; index < array.Length; index++)
            {
                var charArray = array[index].ToCharArray();
                Array.Reverse(charArray);
                result[index] = new string(charArray);
            }

            return result;
        }

        public string[] FlipVertical(string[] array)
        {
            string[] result = array;
            Array.Reverse(result);

            return result;
        }
        

        public void OnDriverChanged(object sender, DriversChangedEventArgs args)
        {
            if (Data.CurrentRace.Running)
            {
                DrawScoreboard(args.Points);
                DrawTrack(args.Track);
            }
        }

        public void OnGameWon(object sender, GameWonEventArgs args)
        {
            Console.Clear();
        }

        public void OnNextTrack(object sender, EventArgs args)
        {
            trackLoaded = false;
            if (Data.CurrentRace != null)
            {
                Data.CurrentRace.DriversChanges += OnDriverChanged;
                Data.CurrentRace.GameWon += OnGameWon;
                InitializeTrack(Data.CurrentRace.Track);
            }
        }
    }
}
