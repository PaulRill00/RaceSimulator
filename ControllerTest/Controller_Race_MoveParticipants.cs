using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Controller;
using Model;
using System.Linq;

namespace Tests
{
    class Controller_Race_MoveParticipants
    {
        private Race _race;

        [SetUp]
        public void SetUp()
        {
            Track track = new Track("test", new SectionTypes[] { SectionTypes.StartGrid, SectionTypes.Finish });
            _race = new Race(track, new List<IParticipant> {
                new Driver("a", TeamColors.Red),
                new Driver("b", TeamColors.Yellow),
                new Driver("c", TeamColors.Green),
                new Driver("d", TeamColors.Blue),
                new Driver("e", TeamColors.Grey)
            });
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

        [TestCase(50)]
        [TestCase(99)]
        [TestCase(0)]
        public void AddDistanceToParticipant_WithingSectionLength(int distance)
        {
            IParticipant participant = _race.Participants.ElementAt(0);
            SectionData current = _race.GetSectionDataForParticipant(participant);
            int currentIndex = _race.GetIndexOfSection(current);

            _race.AddDistanceToParticipant(participant, distance);
            SectionData result = _race.GetSectionDataForParticipant(participant);
            int currentResult = _race.GetIndexOfSection(result);

            Assert.AreEqual(currentIndex, currentResult);
        }

        [TestCase(110)]
        [TestCase(101)]
        public void AddDistanceToParticipant_MoveToNext(int distance)
        {
            IParticipant participant = _race.Participants.ElementAt(0);
            SectionData current = _race.GetSectionDataForParticipant(participant);
            int currentIndex = _race.GetIndexOfSection(current);

            _race.AddDistanceToParticipant(participant, distance);
            SectionData result = _race.GetSectionDataForParticipant(participant);
            int currentResult = _race.GetIndexOfSection(result);

            Assert.AreEqual(currentIndex, currentResult);
        }
    }
}
