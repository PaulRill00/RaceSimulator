using System.Linq;
using Model;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    class Model_ParticipantScores
    {
        private RaceInfo<DriverPoints> Points { get; set; }

        [SetUp]
        public void SetUp()
        {
            Points = new RaceInfo<DriverPoints>();
        }

        [Test]
        public void DriverPoints_Add_AddToList()
        {
            Points.AddToList(new DriverPoints() { Name = "a", Points = 10 });
            Points.AddToList(new DriverPoints() { Name = "b", Points = 10 });

            Assert.AreEqual(Points.GetList().Count, 2);
        }

        [Test]
        public void DriverPoints_Add_SumScored()
        {
            Points.AddToList(new DriverPoints() { Name = "a", Points = 10 });
            Points.AddToList(new DriverPoints() { Name = "a", Points = 10 });

            Assert.AreEqual(Points.GetList().ElementAt(0).Points, 20);
        }
    }
}
