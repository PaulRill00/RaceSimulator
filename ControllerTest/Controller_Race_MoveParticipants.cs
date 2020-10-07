using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Controller;
using Model;
using System.Linq;

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
            _race = new Race(Data.Competition.NextTrack(), Data.Competition.Participants);
            _race.PlaceParticipants();
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
        public void AddDistanceToParticipant_WithingSectionLength(int distance, int participantID)
        {
            IParticipant participant = _race.Participants.ElementAt(participantID);
            SectionData current = _race.GetSectionDataForParticipant(participant);
            int currentIndex = _race.GetIndexOfSection(current);

            _race.AddDistanceToParticipant(participant, distance);
            SectionData result = _race.GetSectionDataForParticipant(participant);
            int currentResult = _race.GetIndexOfSection(result);

            Assert.AreEqual(currentIndex, currentResult);
        }

        [TestCase(110, 1)]
        [TestCase(101, 0)]
        public void AddDistanceToParticipant_MoveToNext(int distance, int participantID)
        {
            IParticipant participant = _race.Participants.ElementAt(participantID);
            SectionData current = _race.GetSectionDataForParticipant(participant);
            int currentIndex = _race.GetIndexOfSection(current);

            _race.AddDistanceToParticipant(participant, distance);
            SectionData result = _race.GetSectionDataForParticipant(participant);
            int currentResult = _race.GetIndexOfSection(result);

            Assert.AreNotEqual(currentIndex, currentResult);
        }
    }
}
