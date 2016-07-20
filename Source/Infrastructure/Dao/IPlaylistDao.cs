using System.Collections.Generic;

namespace Infrastructure.Dao
{
    public interface IPlaylistDao
    {
        int GetPlaylistIdFromName(string name);

        List<string> NameOfAllPlaylists();
    }
}