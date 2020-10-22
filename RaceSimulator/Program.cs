using System.Threading;
using Controller;

namespace RaceSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            new TrackVisuals();

            Data.Initialize();
            Data.NextRace();

            for (; ; )
            {
                Thread.Sleep(100);
            }
        }
    }
}
