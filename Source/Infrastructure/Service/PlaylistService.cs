﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using Infrastructure.Dao;
using Infrastructure.Models;

namespace Infrastructure.Service
{
    /// <summary>
    /// Uses other objects to gather i info about playlists, across 
    /// modules and db tables.
    /// </summary>
    public class PlaylistService : IPlaylistService
    {
        private readonly IPlaylistDao _playlistDao;
        public List<ISongServicePlaylistSaver> Services { get; }
        

        public PlaylistService(List<ISongServicePlaylistSaver> services,
            IPlaylistDao playlistDao)
        {
            _playlistDao = playlistDao;
            this.Services = services;
        }

        public void SavePlaylist(Playlist playlist)
        {
            //TODO:handle new playlist
            var playlistId = _playlistDao.GetPlaylistIdFromName(playlist.Name);
            
            foreach (var ssps in Services)
            {
                var songs = playlist.Songs.Where(t => ssps.CanSave(t)).ToArray();
                for (int index = 0; index < songs.Length; index++)
                {
                    var song = songs[index];
                    ssps.SaveSong(song,playlist);
                }

                var removedSongs = playlist.SongsRemoved.Where(t => ssps.CanSave(t)).ToArray();
                for (int i = 0; i < removedSongs.Length; i++)
                {
                    var song = removedSongs[i];
                    ssps.RemoveSong(song,playlist);
                }
            }

            playlist.SongsRemoved.Clear();
        }

        public Playlist LoadPlaylist(string name)
        {
            var res = new Playlist
            {
                Name = name,
                Id = _playlistDao.GetPlaylistIdFromName(name)
            };

            foreach (var songServicePlaylistSaver in Services)
            {
                var songs = songServicePlaylistSaver.
                    LoadSongsBelongingToPlaylist(int.Parse(res.Id));
                res.Songs.AddRange(songs);
            }

            res.Songs = new ObservableCollection<BaseSong>(res.Songs.OrderBy(x => x.Order).ToList());

            return res;
        }

        public List<string> NameOfAllPlaylists()
        {
            return _playlistDao.NameOfAllPlaylists();
        }
    }
}