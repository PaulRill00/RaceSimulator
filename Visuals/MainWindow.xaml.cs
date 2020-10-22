using System;
using System.Windows;
using System.Windows.Controls;
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
        public bool MultiWindows { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            Data.NextTrack += OnNextTrack;
            Data.Initialize();
            Data.NextRace();
            Title = "Racebaan Simulator";
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;
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
            Visuals.DataContext.Instance.OnNextTrack();
            
            if (Data.CurrentRace == null) return;

            Data.CurrentRace.DriversChanges += OnDriverChanged;
            Data.CurrentRace.CalculateCoords();
            Data.CurrentRace.ApplyOffset();
        }

        private void OnCompetitionStatsOpen(object sender, RoutedEventArgs e)
        {
            OpenWindow<CompetitionStats>();
        }

        private void OnRaceStatsOpen(object sender, RoutedEventArgs e)
        {
            OpenWindow<RaceStats>();
        }

        public void OpenWindow<T>() where T : Window, new()
        {

            if (!MultiWindows)
            {
                WrapPanel panel = typeof(T) == typeof(RaceStats) ? RaceStatsPanel : CompetitionStatsPanel;
                panel.Visibility = panel.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                RaceStatsPanel.SetValue(Grid.RowSpanProperty,
                    CompetitionStatsPanel.Visibility == Visibility.Visible ? 1 : 2);
                CompetitionStatsPanel.SetValue(Grid.RowProperty,
                    RaceStatsPanel.Visibility == Visibility.Visible ? 2 : 1);
                CompetitionStatsPanel.SetValue(Grid.RowSpanProperty,
                    RaceStatsPanel.Visibility == Visibility.Visible ? 1 : 2);

                RaceStats?.Close();
                RaceStats = null;

                CompetitionStats?.Close();
                CompetitionStats = null;
            }
            else
            {
                RaceStatsPanel.Visibility = Visibility.Collapsed;
                CompetitionStatsPanel.Visibility = Visibility.Collapsed;

                Window newWindow = typeof(T) == typeof(RaceStats) ? (Window)RaceStats : (Window)CompetitionStats;

                if (newWindow == null)
                {
                    newWindow = new T();
                    RaceStats = typeof(T) == typeof(RaceStats) ? (RaceStats)newWindow : RaceStats;
                    CompetitionStats = typeof(T) == typeof(CompetitionStats) ? (CompetitionStats)newWindow : CompetitionStats;

                    if (RaceStats != null)
                        RaceStats.Closed += (o, args) => RaceStats = null;
                    if (CompetitionStats != null)
                        CompetitionStats.Closed += (o, args) => CompetitionStats = null;

                    newWindow.Show();
                }

                newWindow.Focus();
            }
        }

        private void OnExit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        public void OnToggleWindows(object sender, RoutedEventArgs e)
        {
            MultiWindows = !MultiWindows;
            Visuals.DataContext.Instance.ShowMultiWindow = MultiWindows;
        }
    }
}
