using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Controller;
using Model;
using System.Linq;

namespace Tests
{
    class Controller_Race_ParticipantStandings
    {
        private Race _race;

        [SetUp]
        public void SetUp()
        {
            Track track = new Track("test", new SectionTypes[] { SectionTypes.StartGrid, SectionTypes.Finish, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.StartGrid });
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
        public void GetOrderedParticpants()
        {
            IParticipant first = _race.Participants.ElementAt(1); // left racer instead of right

            List<IParticipant> list = _race.GetOrderedParticipants();

            Assert.AreEqual(first.Name, list.ElementAt(0).Name);
        }
    }
}
