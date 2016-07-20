using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Dao;
using NUnit.Framework.Internal;
using NUnit.Framework;
using Infrastructure.IntegrationTests.Helper;
using Infrastructure.Tests.Integration.Helper;

namespace Infrastructure.Tests.Integration
{
    [TestFixture]
    class PlaylistDaoTests
    {
        [Test]
        public void ReturnAllPlaylistNamesReturnCorrect()
        {
            //Arrange
            var conn = SQliteConnectionHelper.CreateMemoryPlaylistTable();
            conn.InsertTestData("insert into playlist values (1,'first')");
            conn.InsertTestData("insert into playlist values (2,'second')");
            var service = new PlaylistDao(conn);

            //act
            var playlistNames = service.NameOfAllPlaylists();

            //assert
            Assert.AreEqual("first",playlistNames[0]);
            Assert.AreEqual("second",playlistNames[1]);
        }

        [TestCase(1,"first")]
        [TestCase(2, "second")]
        public void ReturnCorrectPlaylistIdFromPlayloistName(int expect, string name)
        {
            //arragne
            var conn = SQliteConnectionHelper.CreateMemoryPlaylistTable();
            conn.InsertTestData("insert into playlist values (1,'first')");
            conn.InsertTestData("insert into playlist values (2,'second')");
            var service = new PlaylistDao(conn);

            //assert
            var id = service.GetPlaylistIdFromName(name);

            Assert.AreEqual(expect,id);
        }
    }
}
