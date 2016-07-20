using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using Infrastructure.Models;
using Infrastructure.Service;
using Moq;
using NUnit.Framework;
using Webplayer.Modules.Structure.UnitTests.Helper;
using Webplayer.Modules.Structure.ViewModels;

namespace Webplayer.Modules.Structure.UnitTests
{
    [TestFixture]
    class StatusViewModelTests
    {
        private StatusViewModel CreateVM(out Mock<IQueueController> mock)
        {
            mock = new Mock<IQueueController>();
            var p = new Playlist();
            return new StatusViewModel(mock.Object, p);
        }

        //[Test]
        //public void PlayPauseCommand_execute_ToggleIsPlaying()
        //{
        //    //Implement event for isplaying.
        //    var vm = CreateVM();
        //    vm.QueueController.CurrentSong = new TestSong();
        //    vm.QueueController.IsPlaying = Arg.Do<bool>(b =>
        //    {
        //        vm.QueueController.IsPlayingChangedEvent += 
        //        Raise.EventWith(vm.QueueController,new PlayingChangedEventArgs(b));
        //    });
        //    vm.PlayPauseCommand.Execute(null);

        //    //Making sure QueueController.isplaying was called with param true.
        //    vm.QueueController.Received().IsPlaying = true;
        //    //checking that vm fetched new value
        //    Assert.IsTrue(vm.IsPlaying);
        //}

        [Test]
        public void NextCommand_execute_DoesNothingWhenNoneIsNext()
        {
            Mock<IQueueController> mock;
            var vm = CreateVM(out mock);

            vm.NextCommand.Execute(null);
            
            Assert.IsNull(vm.CurrenSong);
            Assert.IsFalse(vm.IsPlaying);
            mock.VerifySet(t => t.IsPlaying = It.IsAny<bool>(),Times.Never);
        }

        [Test]
        public void NextCommand_execute_FindsNextSong()
        {
            Mock<IQueueController> mock;
            var vm = CreateVM(out mock);
            
            vm.NextCommand.Execute(null);
            
            mock.Verify(t=> t.NextSong(),Times.AtLeastOnce);
        }

        [Test]
        public void PreviousCommand_execute_FindsPreviousSong()
        {
            Mock<IQueueController> mock;
            var vm = CreateVM(out mock);

            vm.PreviousCommand.Execute(null);

            //current song was set to mockSong object.
            mock.Verify(t => t.PreviousSong(),Times.Once);
        }

        [Test]
        public void QueueController_IsPlayingEvent_IsPlayingChanged()
        {
            Mock<IQueueController> mock;
            var vm = CreateVM(out mock);
            
            mock.Raise(t => t.IsPlayingChangedEvent += null, 
                new PlayingChangedEventArgs(true) );

            Assert.IsTrue(vm.IsPlaying);
        }

        [Test]
        public void QueueController_IsPlayingEvent_SongChanged()
        {
            Mock<IQueueController> mock;
            var vm = CreateVM(out mock);
            var stub = new TestSong();

            mock.Raise(t => t.CurrentSongChangedEvent += null,
                new SongChangedEventArgs(stub));

            Assert.AreSame(stub,vm.CurrenSong);
        }
    }
}
