using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Models;
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
        public void ServiceLoadsCorrectSongsWhenHaveSongs()
        {
            //arrange
            var conn = ConnectionHelper.CreateMemoryTable();
            var service = new YoutubePlaylistService(conn);
            conn.InsertTestData("Insert into YoutubeSong values ('song1',1,100,'title1')");
            conn.InsertTestData("Insert into YoutubeSong values ('song2',1,200,'title2')");

            //act
            var songs = service.LoadSongsBelongingToPlaylist(200);

            //assert
            var firstSong = (YoutubeSong) songs.First();
            Assert.AreEqual("song2",firstSong.VideoId);
        }

        [Test]
        public void ServiceAddCorrectSongsOnSave()
        {
            //arrange
            var conn = ConnectionHelper.CreateMemoryTable();
            var service = new YoutubePlaylistService(conn);
            var song = new YoutubeSong
            {
                Title = "test Song",
                VideoId = "testVideoId",

            };

            //act
            service.SaveSong(song,2,4);

            //assert
            var res = service.LoadSongsBelongingToPlaylist(4);
            Assert.AreEqual("test Song",res.First().Title);
        }

        [Test]
        public void ServiceReplaceCorrectSongsOnSave()
        {
            //arrange
            var conn = ConnectionHelper.CreateMemoryTable();
            var service = new YoutubePlaylistService(conn);
            var song = new YoutubeSong
            {
                Title = "test Song",
                VideoId = "testVideoId",

            };

            //act
            service.SaveSong(song, 2, 4);
            service.SaveSong(song, 2, 4);

            //assert
            var res = service.LoadSongsBelongingToPlaylist(4);
            Assert.AreEqual("test Song", res.First().Title);
            Assert.AreEqual(1,res.Count);
        }
    }
}
