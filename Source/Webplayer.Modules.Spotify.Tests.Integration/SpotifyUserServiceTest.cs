using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webplayer.Modules.Spotify.Services;

namespace Webplayer.Modules.Spotify.Tests.Integration
{
    [TestFixture]
    public class SpotifyUserServiceTest
    {
        [Test]
        public async Task Test()
        {
            var service = new SpotifyUserService();
            await service.TryAuthenticate();

        }
    }
}
