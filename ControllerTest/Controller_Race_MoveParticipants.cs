using NUnit.Framework;
using Controller;
using Model;
using System.Linq;
using System.Timers;

namespace Tests
{
    [TestFixture]
    class Controller_Race_MoveParticipants
    {
        private Race _race;

        [SetUp]
        public void SetUp()
        {
            Data.Initialize();
            Data.NextRace();
            _race = Data.CurrentRace;
        }

        [Test]
        public void GetSectionData()
        {
            SectionData funcResult = _race.GetSectionDataForParticipant(_race.Participants[0]);
            SectionData expected = _race.GetSectionData(_race.Track.Sections.First.Value);

            Assert.AreEqual(funcResult, expected);
        }

        [Test]
        public void GetNextSectionData()
        {
            SectionData first = _race.GetSectionData(_race.Track.Sections.ElementAt(0));
            SectionData second = _race.GetSectionData(_race.Track.Sections.ElementAt(1));

            SectionData result = _race.GetNextData(first);

            Assert.AreEqual(result, second);
        }

        [Test]
        public void GetIndexOfSectionData()
        {
            SectionData data = _race.GetSectionData(_race.Track.Sections.ElementAt(0));
            Assert.AreEqual(_race.GetIndexOfSection(data), 0);
        }

        [TestCase(50, 0)]
        [TestCase(99, 1)]
        [TestCase(0, 0)]
        public void AddDistanceToParticipant_WithinSectionLength(int distance, int participantID)
        {
            IParticipant participant = _race.Participants.ElementAt(participantID);
            SectionData current = _race.GetSectionDataForParticipant(participant);
            int currentIndex = _race.GetIndexOfSection(current);

            _race.AddDistanceToParticipant(participant, distance);
            SectionData result = _race.GetSectionDataForParticipant(participant);
            int currentResult = _race.GetIndexOfSection(result);

            Assert.AreEqual(currentIndex, currentResult);
        }

        [Test]
        public void StartStopTimer()
        {
            Assert.IsTrue(Data.CurrentRace.Timer.Enabled);
            _race.Stop();
            Assert.IsFalse(Data.CurrentRace.Timer.Enabled);
        }

        //[TestCase(100, 1)]
        //[TestCase(110, 0)]
        //public void AddDistanceToParticipant_MoveToNext(int distance, int participantID)
        //{
        //    IParticipant participant = _race.Participants.ElementAt(participantID);
        //    SectionData current = _race.GetSectionDataForParticipant(participant);
        //    int currentIndex = _race.GetIndexOfSection(current);

        //    _race.AddDistanceToParticipant(participant, distance);
        //    SectionData result = _race.GetSectionDataForParticipant(participant);
        //    int currentResult = _race.GetIndexOfSection(result);

        //    Assert.AreNotEqual(currentIndex, currentResult);
        //}
    }
}
