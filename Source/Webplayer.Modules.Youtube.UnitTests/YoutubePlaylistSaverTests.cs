using NUnit.Framework;

namespace Webplayer.Modules.Youtube.Tests.Unit
{
    [TestFixture]
    class YoutubePlaylistSaverTests
    {
        //private IDbService CreateDbService()
        //{
        //    return Substitute.For<IDbService>();
        //}

        //private Mock<IDbService> CreateDbService2()
        //{
        //    return new Mock<IDbService>();
        //}

        //[Test]
        //public void Savesong_Calls_Replace()
        //{
        //    var dbService = CreateDbService2();
        //    var saver = new YoutubePlaylistSaver(dbService.Object);
        //    var song = new YoutubeSong
        //    {
        //        Artist = "artist",
        //        Title = "title"
        //    };
        //    dbService.Setup(t => t.Replace<YoutubeSong>
        //        (It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(),null)).
        //        Returns(1);

        //    saver.SaveSong(song, 2);

        //    var anyDict = Arg.Any<Dictionary<string, string>>();
        //    dbService.Verify(t => t.Replace<YoutubeSong>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), null));
        //}

        //[Test]
        //public void Loadsong_returns_right_song()
        //{
        //    var dbService = CreateDbService();
        //    var loader = new YoutubePlaylistSaver(dbService);
        //    var song = new YoutubeSong();
        //    var res = new List<YoutubeSong> {song};
        //    dbService.Select<YoutubeSong>("", "", new Dictionary<string, string>(), (reader => { return null; })).
        //        ReturnsForAnyArgs(res);

        //    var returned = loader.LoadSong(Arg.Any<string>());

        //    Assert.AreSame(song,returned);
        //}
    }
}
