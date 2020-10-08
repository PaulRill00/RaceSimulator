using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Controller;
using Model;

namespace Visuals
{
    public class DataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public static DataContext Instance { get; set; }

        public List<DriverPoints> PlayerPoints => Data.Competition.Points.GetList().OrderByDescending(x => x.Points).ToList();

        public List<Driver> PlayerRounds => Data.Competition.Participants.OfType<Driver>()
            .OrderByDescending(x => x.CurrentRound).OrderByDescending(x => x.DistanceTraveled)
            .ToList();

        public List<Driver> PlayerSpeed =>
            Data.Competition.Participants.OfType<Driver>().OrderByDescending(x => ((Car)x.Equipment).Speed).ToList();

        public List<Driver> PlayerDistanceDriven =>
            Data.Competition.Participants.OfType<Driver>().OrderByDescending(x => x.DistanceTraveled).ToList();

        public string TrackName => Data.CurrentRace == null ? "" : Data.CurrentRace.Track.Name;

        public DataContext()
        {
            Instance = this;
        }   

        public void OnDriversChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }

        public void OnNextTrack()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}
