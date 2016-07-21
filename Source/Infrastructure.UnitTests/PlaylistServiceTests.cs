using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Dao;
using Infrastructure.Models;
using Infrastructure.Service;
using Infrastructure.Tests.Unit.Helper;
using Moq;
using NUnit.Framework;

namespace Infrastructure.Tests.Unit
{
    [TestFixture]
    class PlaylistServiceTests
    {


        private PlaylistService CreateServiceWith2MockSubServices(out Mock<IPlaylistDao> mock)
        {
            var services = new List<ISongServicePlaylistSaver>();
            mock = new Mock<IPlaylistDao>();
            return new PlaylistService(services,mock.Object);
        }

        [Test]
        public void LoadPlaylistShouldUseSongservicesToPopulateList()
        {
            //arrange
            Mock<IPlaylistDao> mock;
            var sqliteService = CreateServiceWith2MockSubServices(out mock);
            var service1 = new Mock<ISongServicePlaylistSaver>();
            var service2 = new Mock<ISongServicePlaylistSaver>();
            var stubSong1 = new TestSong();
            var stubSong2 = new TestSong();
            service1.Setup(t => t.LoadSongsBelongingToPlaylist(It.IsAny<int>()))
                .Returns(new List<BaseSong>() {stubSong1});
            sqliteService.Services.Add(service1.Object);
            service2.Setup(t => t.LoadSongsBelongingToPlaylist(It.IsAny<int>()))
                .Returns(new List<BaseSong>() { stubSong2 });
            sqliteService.Services.Add(service2.Object);
            mock.Setup(t => t.GetPlaylistIdFromName(It.IsAny<string>())).Returns("1");

            //act
            var loadedPlaylist = sqliteService.LoadPlaylist("");

            //assert
            Assert.IsTrue(loadedPlaylist.Songs.Contains(stubSong1));
            Assert.IsTrue(loadedPlaylist.Songs.Contains(stubSong2));
        }
    }
}
