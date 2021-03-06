﻿using System.Collections.Generic;
using Infrastructure.Models;

namespace Infrastructure.Service
{
    /// <summary>
    /// Service used to save or retrive playlist and songs in it.
    /// </summary>
    public interface ISongServicePlaylistSaver
    {
        /// <summary>
        /// save <param name="song"></param> data with 
        /// <param name="playlistNr"></param>
        /// </summary>
        /// <param name="song"></param>
        /// <param name="playlist"></param>
        void SaveSong(BaseSong song, Playlist playlist);

        /// <summary>
        /// Determines if song can be saved with this service.
        /// 
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        bool CanSave(BaseSong song);

        /// <summary>
        /// Returns all song registerd to a playlist. not in order.
        /// </summary>
        /// <param name="playlistId"></param>
        /// <returns>songs in no special order.</returns>
        List<BaseSong> LoadSongsBelongingToPlaylist(int playlistId);

        /// <summary>
        /// removes song.
        /// </summary>
        /// <param name="song"></param>
        /// <param name="playlist"></param>
        void RemoveSong(BaseSong song, Playlist playlist);

        /// <summary>
        /// Id of song type
        /// </summary>
        string SongId { get; }
    }
}