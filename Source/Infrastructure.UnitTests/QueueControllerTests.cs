using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Models;
using Infrastructure.Service;
using Infrastructure.Tests.Unit.Helper;
using NUnit.Framework.Internal;
using NUnit.Framework;

namespace Infrastructure.Tests.Unit
{
    [TestFixture]
    class QueueControllerTests
    {
        [Test]
        public void IsPlaying_Set_Event()
        {
            var eventRaised = false;
            bool eventResult = false;
            IQueueController controller = new QueueController();
            controller.IsPlayingChangedEvent += delegate(object sender,PlayingChangedEventArgs status)
            {
                eventRaised = true;
                eventResult = status.IsPlaying;
            };

            controller.IsPlaying = true;

            Assert.AreEqual(eventRaised, true);
            Assert.AreEqual(eventResult, true);
        }

        [Test]
        public void IsPlaying_Set_EventNotCalled()
        {
            var eventRaised = false;
            IQueueController controller = new QueueController();
            controller.IsPlayingChangedEvent += delegate (object sender,PlayingChangedEventArgs status)
            {
                eventRaised = true;
            };

            controller.IsPlaying = false;

            Assert.AreEqual(eventRaised, false);
        }

        [Test]
        public void CurrentSong_set_NewRaiseEvent()
        {
            var eventRaised = false;
            BaseSong songSent = null;
            IQueueController controller = new QueueController();
            controller.CurrentSongChangedEvent += delegate(object s,SongChangedEventArgs model)
            {
                eventRaised = true;
                songSent = model.CurrentSong;
            };
            var songStub = new TestSong();

            controller.CurrentSong = songStub;

            Assert.IsTrue(eventRaised);
            Assert.AreSame(songSent, songStub);
        }
    }
}
