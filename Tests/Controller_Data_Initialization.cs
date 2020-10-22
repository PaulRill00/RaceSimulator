using Controller;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    class Controller_Data_Initialization
    {
        [SetUp]
        public void SetUp()
        {
            Data.Initialize();
        }

        [Test]
        public void AddParticipants_Filled()
        {
            Data.AddParticipants();

            Assert.IsNotEmpty(Data.Competition.Participants);
        }

        [Test]
        public void NextRace()
        {
            Race first = Data.CurrentRace;
            Data.NextRace();
            Race next = Data.CurrentRace;

            Assert.AreNotEqual(first, next);
        }

        [Test]
        public void NextRace_NoNextTrack()
        {
            while(Data.Competition.Tracks.Count > 0)
            {
                Data.NextRace();
            }
            Data.NextRace();

            Assert.IsNull(Data.CurrentRace);
        }
    }
}
