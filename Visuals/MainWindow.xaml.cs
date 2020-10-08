using System;
using System.Windows;
using Controller;
using Model;
using DispatcherPriority = System.Windows.Threading.DispatcherPriority;

namespace Visuals
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public CompetitionStats CompetitionStats { get; set; }
        public RaceStats RaceStats { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Data.NextTrack += OnNextTrack;
            Data.Initialize();
            Data.NextRace();
            Title = "Racebaan Simulator";
        }

        public void OnDriverChanged(object sender, DriversChangedEventArgs args)
        {
            TrackImage.Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new Action(() =>
                {
                    TrackImage.Source = null;
                    TrackImage.Source = Visualize.DrawTrack(args.Track);
                })
            );

            Visuals.DataContext.Instance.OnDriversChanged();
        }

        public void OnNextTrack(object sender, EventArgs args)
        {
            ImageLoader.ClearCache();
            Visuals.DataContext.Instance.OnDriversChanged();
            
            if (Data.CurrentRace == null) return;

            Data.CurrentRace.DriversChanges += OnDriverChanged;
            Data.CurrentRace.CalculateCoords();
            Data.CurrentRace.ApplyOffset();
        }

        private void OnCompetitionStatsOpen(object sender, RoutedEventArgs e)
        {
            if (CompetitionStats == null)
            {
                CompetitionStats = new CompetitionStats();
                CompetitionStats.Closed += (o, args) => CompetitionStats = null;
                CompetitionStats.Show();
            }

            CompetitionStats.Focus();
        }

        private void OnRaceStatsOpen(object sender, RoutedEventArgs e)
        {
            if (RaceStats == null)
            {
                RaceStats = new RaceStats();
                RaceStats.Closed += (o, args) => RaceStats = null;
                RaceStats.Show();
            }

            
            RaceStats.Focus();
        }

        private void OnExit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
