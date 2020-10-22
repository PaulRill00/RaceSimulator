using NUnit.Framework;
using Model;

namespace Tests
{
    [TestFixture]
    public class Model_Competition_NextTrackShould
    {
        private Competition _competition;

        [SetUp]
        public void SetUp()
        {
            _competition = new Competition();
        }

        [Test]
        public void NextTrack_EmptyQueue_ReturnNull()
        {
            var result = _competition.NextTrack();
            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_OneInQueue_ReturnTrack()
        {
            Track track = new Track("testTrack", new SectionTypes[] { });
            _competition.Tracks.Enqueue(track);

            Track _testTrack = _competition.NextTrack();
            Assert.AreEqual(_testTrack, track);
        }

        [Test]
        public void NextTrack_OneInQueue_RemoveTrackFromQueue()
        {
            Track track1 = new Track("testTrack01", new SectionTypes[] { });
            _competition.Tracks.Enqueue(track1);

            var result = _competition.NextTrack();
            result = _competition.NextTrack();

            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_TwoInQueue_ReturnNextTrack()
        {
            Track track01 = new Track("track01", new SectionTypes[] { });
            Track track02 = new Track("track02", new SectionTypes[] { });

            _competition.Tracks.Enqueue(track01);
            _competition.Tracks.Enqueue(track02);

            var result01 = _competition.NextTrack();
            var result02 = _competition.NextTrack();

            Assert.AreNotEqual(result01, result02);
        }

    }
}

