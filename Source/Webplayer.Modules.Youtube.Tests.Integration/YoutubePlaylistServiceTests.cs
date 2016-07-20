using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Internal;
using NUnit.Framework;
using Webplayer.Modules.Youtube.Models;
using Webplayer.Modules.Youtube.Services;
using Webplayer.Modules.Youtube.Tests.Integration.Helper;

namespace Webplayer.Modules.Youtube.Tests.Integration
{
    [TestFixture]
    class YoutubePlaylistServiceTests
    {
        [Test]
        public void ServiceLoadsCorrectSongs()
        {
            //arrange
            var conn = ConnectionHelper.CreateMemoryTable();
            var service = new YoutubePlaylistService(conn);
            conn.InsertTestData("Insert into YoutubeSong values ('song1',1,100)");
            conn.InsertTestData("Insert into YoutubeSong values ('song2',1,200)");

            //act
            var songs = service.LoadSongsBelongingToPlaylist(200);

            //assert
            var firstSong = (YoutubeSong) songs.First();
            Assert.AreEqual("song2",firstSong.VideoId);
        }
    }
}
