using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

        private bool trackLoaded;
        public MainWindow()
        {
            InitializeComponent();
            Data.NextTrack += OnNextTrack;
            Data.Initialize();
            Data.NextRace();
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
        }

        public void OnNextTrack(object sender, EventArgs args)
        {
            ImageLoader.ClearCache();
            trackLoaded = false;
            if (Data.CurrentRace != null)
            {
                Data.CurrentRace.DriversChanges += OnDriverChanged;
                //Data.CurrentRace.GameWon += OnGameWon;
                Data.CurrentRace.CalculateCoords();
                Data.CurrentRace.ApplyOffset();
                trackLoaded = true;
            }
        }
    }
}
