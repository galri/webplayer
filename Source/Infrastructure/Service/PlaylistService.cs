using System;
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
            
            //set right order
            //foreach (var song in playlist.Songs)
            //{
            //    playlist.SongsRemoved.Add(song);
            //    song.PlaylistNr = playlist.Songs.IndexOf(song);
            //}

            foreach (var ssps in Services)
            {
                var removedSongs = playlist.SongsRemoved.Where(t => ssps.CanSave(t)).ToArray();
                foreach (var song in removedSongs)
                {
                    ssps.RemoveSong(song, playlist);
                }

                var songs = playlist.Songs.Where(t => ssps.CanSave(t)).ToArray();
                foreach (var song in songs)
                {
                    ssps.RemoveSong(song, playlist);
                    song.PlaylistNr = playlist.Songs.IndexOf(song);
                    ssps.SaveSong(song,playlist);
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
