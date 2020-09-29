using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Model;

namespace Controller
{
    public class Race
    {
        public Track Track { get; set; }
        public List<IParticipant> Participants { get; set; }
        public DateTime StartTime { get; set; }
        public bool Running { get; private set; } = false;

        private Random _random;
        private Dictionary<Section, SectionData> _positions;
        private Timer _timer;
        private Driver _winner;
        private List<IParticipant> _finished = new List<IParticipant>();

        public event EventHandler<DriversChangedEventArgs> DriversChanges;
        public event EventHandler<GameWonEventArgs> GameWon;

        public Race(Track track, List<IParticipant> participants)
        {
            Track = track;
            Participants = new List<IParticipant>();
            foreach(IParticipant participant in participants)
            {
                Driver driver = (Driver)participant;
                driver.CurrentRound = 0;
                driver.DistanceTraveled = 0;
                Participants.Add(driver);
            }

            _random = new Random(DateTime.Now.Millisecond);
            _positions = new Dictionary<Section, SectionData>();

            RandomizeEquipment();
            PlaceParticipants();

            _timer = new Timer(500);
            _timer.Elapsed += OnTimedEvent;
        }

        public SectionData GetSectionData(Section section)
        {
            if (!_positions.ContainsKey(section))
                _positions.Add(section, new SectionData());

            return _positions.GetValueOrDefault(section);
        }

        public Section GetSection(SectionData data)
        {
            if (_positions.ContainsValue(data))
            {
                foreach(KeyValuePair<Section, SectionData> pair in _positions)
                {
                    if (pair.Value == data)
                        return pair.Key;
                }
            }
            return null;
        }

        public void RandomizeEquipment()
        {
            Participants.ForEach(participant =>
            {
                participant.Equipment.Performance = _random.Next(5,9) + 1;
                participant.Equipment.Quality = _random.Next(5,9) + 1;
                participant.Equipment.Speed = CalculateSpeed(participant.Equipment.Performance, participant.Equipment.Quality);
            });
        }

        public void PlaceParticipants()
        {
            List<Section> startSections = GetStartSections();
            int maxParticipants = startSections.Count * 2;
            maxParticipants = maxParticipants > Participants.Count ? Participants.Count : maxParticipants;

            for(int index = 0; index < maxParticipants; index++)
            {
                SectionData sectionData = GetSectionData(startSections[index / 2]);
                ((Driver)Participants[index]).DistanceTraveled = 0 - (index / 2);
                if (index % 2 > 0)
                {
                    sectionData.Left = Participants[index];
                }
                else
                {
                    sectionData.Right = Participants[index];
                }
            }
        }

        public List<Section> GetStartSections()
        {
            List<Section> startSections = new List<Section>();

            foreach(Section section in Track.Sections)
            {
                if (section.SectionType == SectionTypes.StartGrid)
                    startSections.Add(section);
            }

            Section first = startSections[0];
            startSections.RemoveAt(0);
            startSections.Add(first);

            startSections.Reverse();

            return startSections;
        }

        public void Start()
        {
            _timer.Start();
            Running = true;
        }

        public void Stop()
        {
            _timer.Stop();
            Running = false;

            DriversChanges = null;
        }

        public void OnTimedEvent(object sender, EventArgs args)
        {
            

            foreach(IParticipant participant in GetOrderedParticipants())
            {
                if (_random.Next(0, 100) >= 90)
                {
                    participant.Equipment.IsBroken = !participant.Equipment.IsBroken;
                    if(participant.Equipment.IsBroken)
                        participant.Equipment.Performance = _random.Next(5, 9) + 1;
                }


                if (!participant.Equipment.IsBroken)
                    AddDistanceToParticipant(participant, participant.Equipment.Speed);

            }

            DriversChanges?.Invoke(this, new DriversChangedEventArgs() { Track = Track });
        }

        public List<IParticipant> GetOrderedParticipants()
        {
            List<IParticipant> participants = Participants.OrderBy(s => ((Driver)s).DistanceTraveled).ToList();
            participants.Reverse();
            return participants;
        }

        public void AddDistanceToParticipant(IParticipant participant, int distance)
        {
            SectionData data = GetSectionDataForParticipant(participant);
            if (data != null)
            {
                if (data.Left == participant)
                {
                    int newValue = data.DistanceLeft + distance;
                    if (newValue >= Data.SectionLength)
                    {
                        MoveToNextSection(participant, data, true, newValue % Data.SectionLength);
                    }
                    else
                    {
                        data.DistanceLeft += distance;
                    }
                }
                else
                {
                    int newValue = data.DistanceRight + distance;
                    if (newValue >= Data.SectionLength)
                    {
                        MoveToNextSection(participant, data, false, newValue % Data.SectionLength);
                    }
                    else
                    {
                        data.DistanceRight += distance;
                    }
                }
            }
        }

        public void MoveToNextSection(IParticipant participant, SectionData data, bool left, int distance)
        {
            SectionData nextData = GetNextData(data);

            if (nextData == null)
                return;

            bool canBeMoved = (nextData.Left == null || nextData.Right == null);
            bool isDone = RoundTest(data, ((Driver)participant));

            if (Participants.Count == 0)
                return;

            if (canBeMoved)
            {
                Driver driver = (Driver)participant;
                driver.DistanceTraveled++;;


                if (left)
                {
                    data.DistanceLeft = 0;
                    data.Left = null;
                }
                else
                {
                    data.DistanceRight = 0;
                    data.Right = null;
                }

                if (!isDone)
                {

                    if (nextData.Left == null)
                    {
                        nextData.Left = participant;
                        nextData.DistanceLeft = distance;
                    }
                    else if (nextData.Right == null)
                    {
                        nextData.Right = participant;
                        nextData.DistanceRight = distance;
                    }
                }
            }
        }

        public bool RoundTest(SectionData data, Driver driver)
        {
            Section section = GetSection(data);
            if (section.SectionType == SectionTypes.Finish)
                driver.CurrentRound++;

            if (driver.CurrentRound - 1 == Data.RoundsToWin)
            {
                _finished.Add(driver);
                Participants.Remove(driver);

                if (Participants.Count == 0)
                {
                    Stop();
                    Data.Competition.SetPoints(CalculateEndPoints());

                    Data.NextRace();
                    GameWon?.Invoke(this, new GameWonEventArgs() { Winner = _winner, Track = Track });
                }
                return true;
            }
            return false;
        }

        public int GetPointForPosition(int position)
        {
            switch(position)
            {
                case 1:
                    return 25;
                case 2:
                    return 20;
                case 3:
                    return 15;
                case 4:
                    return 13;
            }
            int points = 16 - position;
            return points > 0 ? points : 1;
        }

        public List<DriverPoints> CalculateEndPoints()
        {
            List<DriverPoints> points = new List<DriverPoints>();

            for(int i = 0; i < _finished.Count; i++)
            {
                IParticipant participant = _finished.ElementAt(i);
                DriverPoints _points = new DriverPoints() { Name = participant.Name, Points = GetPointForPosition(i + 1) };
                points.Add(_points);
            }
            return points;
        }

        public List<IParticipant> GetStandings()
        {
            return _finished;
        }

        public SectionData GetNextData(SectionData prev)
        {
            int index = GetIndexOfSection(prev);

            index++;
            if (index == Track.Sections.Count)
                index = 0;

            Section nextSection = Track.Sections.ElementAt(index);
            return _positions.GetValueOrDefault(nextSection);
        }

        public SectionData GetSectionDataForParticipant(IParticipant participant)
        {
            return _positions.Values.Where(data => (data.Left != null && data.Left.Equals(participant)) || (data.Right != null && data.Right.Equals(participant))).FirstOrDefault<SectionData>();
        }

        public int GetIndexOfSection(SectionData data)
        {
            Section section = _positions.Where(x => x.Value == data).FirstOrDefault().Key;
            return Track.Sections.TakeWhile(x => x != section).Count();
        }

        public int CalculateSpeed(int performance, int quality)
        {

            return (int)performance * quality;
        }
    }
}
