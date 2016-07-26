using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SpotifyAPI.Web;
using Webplayer.Modules.Spotify.Services;

namespace Webplayer.Modules.Spotify.Tests.Integration
{
    [TestFixture]
    public class NewSpotifySongSearch
    {
        private NewSpotifySongService CreateSearchService()
        {
            var api = new SpotifyWebAPI();
            api.UseAuth = false;

            return new NewSpotifySongService(api);
        }

        [Test]
        public async Task TrackSearchFirstSuccesfull()
        {
            //Arrange
            var service = CreateSearchService();
            service.Query = "metallica";

            //act
            var res = await service.FetchAsync();

            //Assert
            Assert.AreNotEqual(0,res.Count);
        }

        [Test]
        public async Task TrackSearchSecondSuccesfull()
        {
            //Arrange
            var service = CreateSearchService();
            service.Query = "metallica";

            //act
            var res = await service.FetchAsync();
            var res2 = await service.FetchAsync();

            //Assert
            Assert.AreNotEqual(0, res.Count);
            Assert.AreNotEqual(0, res2.Count);
        }
    }
}
