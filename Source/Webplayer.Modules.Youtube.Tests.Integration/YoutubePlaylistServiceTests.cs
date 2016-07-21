using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Models;
using Moq;
using NUnit.Framework.Internal;
using NUnit.Framework;
using Prism.Logging;
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
            var loggerStub = new Mock<ILoggerFacade>();
            var service = new YoutubePlaylistService(conn,loggerStub.Object);
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
            var loggerStub = new Mock<ILoggerFacade>();
            var service = new YoutubePlaylistService(conn, loggerStub.Object);
            var song = new YoutubeSong
            {
                Title = "test Song",
                VideoId = "testVideoId",
                PlaylistNr = 1,
            };
            var playlist = new Playlist
            {
                Id = "1",
            };

            //act
            service.SaveSong(song,playlist);

            //assert
            var res = service.LoadSongsBelongingToPlaylist(1);
            Assert.AreEqual("test Song",res.First().Title);
        }

        [Test]
        public void ServiceReplaceCorrectSongsOnSave()
        {
            //arrange
            var conn = ConnectionHelper.CreateMemoryTable();
            var loggerStub = new Mock<ILoggerFacade>();
            var service = new YoutubePlaylistService(conn, loggerStub.Object);
            var song = new YoutubeSong
            {
                Title = "test Song",
                VideoId = "testVideoId",

            };
            var playlist = new Playlist
            {
                Id = "4",
            };

            //act
            service.SaveSong(song, playlist);
            service.SaveSong(song, playlist);

            //assert
            var res = service.LoadSongsBelongingToPlaylist(4);
            Assert.AreEqual("test Song", res.First().Title);
            Assert.AreEqual(1,res.Count);
        }

        [Test]
        public void ServiceRemoveCorrectSongsOnRemove()
        {
            //arrange
            var conn = ConnectionHelper.CreateMemoryTable();
            conn.InsertTestData("Insert into YoutubeSong values ('song1',1,100,'title1')");
            conn.InsertTestData("Insert into YoutubeSong values ('song2',2,100,'title2')");
            var loggerStub = new Mock<ILoggerFacade>();
            var service = new YoutubePlaylistService(conn, loggerStub.Object);
            var song = new YoutubeSong
            {
                Title = "title1",
                VideoId = "song1",
                PlaylistNr = 1,
            };
            var playlist = new Playlist
            {
                Id = "100",
            };


            //act
            service.RemoveSong(song, playlist);
             
            //assert
            var res = service.LoadSongsBelongingToPlaylist(100);
            Assert.AreEqual(1, res.Count);
            var firstSongRes = (YoutubeSong)res.First();
            Assert.AreEqual("title2", firstSongRes.Title);
            Assert.AreEqual("song2", firstSongRes.VideoId);
        }


    }
}
