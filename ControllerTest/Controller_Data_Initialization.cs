using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
