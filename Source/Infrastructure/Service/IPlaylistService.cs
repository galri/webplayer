using System.Collections.Generic;
using Infrastructure.Models;

namespace Infrastructure.Service
{
    /// <summary>
    /// Should use multiple <see cref="ISongServicePlaylistSaver"/> for fetching info.
    /// </summary>
    public interface IPlaylistService
    {
        void SavePlaylist(Playlist playlist);

        Playlist LoadPlaylist(string name);

        List<string> NameOfAllPlaylists();
    }
}