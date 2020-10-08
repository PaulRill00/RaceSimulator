using System;
using System.Collections.Generic;
using Controller;
using Model;
using NUnit.Framework;
using RaceSimulator;

namespace Tests
{
    [TestFixture]
    public class View_TrackVisuals_TrackCalculating
    {
        private Competition Competition { get; set; }
        private Track Track { get; set; }

        [SetUp]
        public void SetUp()
        {
            Competition = new Competition();
            Competition.Participants.AddRange(new List<IParticipant> {
                new Driver("a", TeamColors.Red),
                new Driver("b", TeamColors.Yellow),
                new Driver("c", TeamColors.Green),
                new Driver("d", TeamColors.Blue),
                new Driver("e", TeamColors.Grey)
            });

            Track = new Track("testTrack", new SectionTypes[] { SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.LeftCorner });
            Competition.Tracks.Enqueue(Track);
            Data.Competition = Competition;
            Data.NextRace();
        }

        [Test]
        public void CalculateCoords()
        {
            Data.CurrentRace.CalculateCoords();

            Assert.IsNotNull(Track.Sections.First.Value.X);
        }

        [Test]
        public void GetOffset_NegativeXAndY()
        {
            Data.CurrentRace.CalculateCoords();
            Data.CurrentRace.ApplyOffset();

            int[] offset = TrackVisuals.Instance.GetOffset(Track.Sections);

            Assert.GreaterOrEqual(offset[0], 0);
            Assert.GreaterOrEqual(offset[1], 0);
        }

        [TestCase(SectionTypes.RightCorner, new int[] { 1, 0 }, ExpectedResult = new int[] { 0, 1 })]
        [TestCase(SectionTypes.LeftCorner, new int[] { 1, 0 }, ExpectedResult = new int[] { 0, -1 })]
        [TestCase(SectionTypes.Finish, new int[] { 1, 0 }, ExpectedResult = new int[] { 1, 0 })]
        [TestCase(null, new int[] { 1, 0 }, ExpectedResult = new int[] { 1, 0 })]
        public int[] GetDirection(SectionTypes sectionTypes, int[] direction)
        { 
            return Data.CurrentRace.GetDirection(direction, sectionTypes);
        }

        [Test]
        public void FlipVertical_ArrayReverse()
        {
            string[] input = new string[] { "3333", "2222", "1111" };
            string[] output = new string[] { "1111", "2222", "3333" };
            string[] result = TrackVisuals.Instance.FlipVertical(input);

            Assert.AreEqual(result, output);
        }

        [Test]
        public void FlipHorizontal_StringsReverse()
        {
            string[] input = new string[] { "1234", "5678", "9012" };
            string[] output = new string[] { "4321", "8765", "2109" };
            string[] result = TrackVisuals.Instance.FlipHorizontal(input);

            Assert.AreEqual(result, output);
        }

        [TestCase(new int[] { 0, 1}, SectionTypes.Straight, ExpectedResult = new string[] { "|  |", "|21|", "|  |", "|  |" })]
        [TestCase(new int[] { 1, 0 }, SectionTypes.RightCorner, ExpectedResult = new string[] { @"--\ ", @"  2\", " 1 |", @"\  |" })]
        [TestCase(new int[] { -1, 0 }, SectionTypes.LeftCorner, ExpectedResult = new string[] { " /--", "/2  ", "| 1 ", "|  /" })]
        public string[] GetSectionVisual(int[] direction, SectionTypes sectionTypes)
        {
            return TrackVisuals.Instance.GetSectionVisual(direction, sectionTypes);
        }
        
        [TestCase(TeamColors.Blue, ExpectedResult = ConsoleColor.Blue)]
        [TestCase(TeamColors.Green, ExpectedResult = ConsoleColor.Green)]
        [TestCase(TeamColors.Grey, ExpectedResult = ConsoleColor.White)]
        [TestCase(TeamColors.Red, ExpectedResult = ConsoleColor.Red)]
        [TestCase(TeamColors.Yellow, ExpectedResult = ConsoleColor.Yellow)]
        public ConsoleColor GetParticipantColor(TeamColors driverColor)
        {
            Driver driver = new Driver("", driverColor);
            return TrackVisuals.Instance.GetParticipantColor(driver);
        }

        [Test]
        public void GetParticipantColor_DriverNull()
        {
            var result = TrackVisuals.Instance.GetParticipantColor(null);
            Assert.AreEqual(result, ConsoleColor.White);
        }
    }
}
