using System.Collections.Generic;

namespace Infrastructure.Dao
{
    public interface IPlaylistDao
    {
        string GetPlaylistIdFromName(string name);

        List<string> NameOfAllPlaylists();
    }
}