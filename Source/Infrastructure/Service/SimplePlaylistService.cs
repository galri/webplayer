using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Models;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Converters;

namespace Infrastructure.Service
{
    public class TestSong : BaseSong
    {

    }

    //TODO: error handling....
    public class SimplePlaylistService :  IPlaylistService
    {
        private const string SAVELOCATION = "Playlists";

        public Playlist LoadPlaylist(string name)
        {
            CheckSaveLocation();

            var path = Path.Combine(SAVELOCATION, name + ".json");
            var jsonData = File.ReadAllText(path);
            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
            };
            var data = JsonConvert.DeserializeObject<Playlist>(jsonData,new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
            return data;
        }

        public List<string> NameOfAllPlaylists()
        {
            CheckSaveLocation();

            var files = Directory.EnumerateFiles(SAVELOCATION).ToList();
            var filesInfo = files.Select(t => new FileInfo(t).Name);
            var names = filesInfo.Select(t => t.Substring(0, t.LastIndexOf(".")));
            return names.ToList();
        }

        public void SavePlaylist(Playlist playlist)
        {
            CheckSaveLocation();

            var jsonData = JsonConvert.SerializeObject(playlist,Formatting.Indented,new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
            var path = Path.Combine(SAVELOCATION, playlist.Name + ".json");
            File.WriteAllText(path, jsonData);
        }

        private void CheckSaveLocation()
        {
            Directory.CreateDirectory(SAVELOCATION);
        }
    }
}
