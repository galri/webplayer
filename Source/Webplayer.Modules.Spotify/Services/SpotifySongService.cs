using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using Webplayer.Modules.Spotify.Models;

namespace Webplayer.Modules.Spotify.Services
{
    /// <summary>
    /////how to search
    //https://developer.spotify.com/technologies/web-api/search/
    //how to find album picture
    //http://stackoverflow.com/questions/10123804/retrieve-cover-artwork-using-spotify-api
    /// </summary>

    class SpotifySongSearch
    {
        private int myQuantity;
        private string mySearch;
        private XDocument mySource;
        private int index = 0;
        private bool shallFetchThumbnail;

        #region Properties
        public int Quantity
        {
            get
            {
                return myQuantity;
            }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentException("Quantity must be greater then zero", "Quantity");
                }
                myQuantity = value;
            }
        }
        public string SearchWord
        {
            get
            {
                return mySearch;
            }
        }
        private XDocument Source
        {
            get { return mySource; }
        }
        public bool ShallFetchThumbnail
        {
            get
            {
                return shallFetchThumbnail;
            }
            set
            {
                shallFetchThumbnail = value;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quantity">number of search results to return each next</param>
        /// <param name="search">What to search after</param>
        /// <param name="fetchThumbnail">Wether to get album picture or not, not may save alot of time</param>
        public SpotifySongSearch(int quantity, string search, bool fetchThumbnail)
        {
            Quantity = quantity;
            mySearch = search;
            mySource = XDocument.Load("http://ws.spotify.com/search/1/track?q=" + SearchWord);
            shallFetchThumbnail = fetchThumbnail;
        }

        /// <summary>
        /// Returns next list of songs.
        /// </summary>
        /// <returns>List with number of search results, 0 in size if none found</returns>
        public List<SpotifySong> next()
        {
            //finds informasjons about tracks
            var tracks = getSearchResult(index, Quantity);

            //sets new index
            index = index + Quantity;

            //find album thumbnail for each track.
            return setPicture(tracks);
        }

        public List<SpotifySong> previous()
        {
            //finds informasjons about tracks
            var tracks = getSearchResult(index - Quantity, index);

            //sets new index
            index = index - Quantity;
            if (index < 0)
            {
                index = 0;
            }

            //find album thumbnail for each track.
            return setPicture(tracks);
        }

        /// <summary>
        /// get information about song uri and tittel, gets picture and creates objects.
        /// </summary>
        /// <param name="tracks"></param>
        /// <returns></returns>
        private List<SpotifySong> setPicture(dynamic tracks)
        {
            //to hold objects.
            List<SpotifySong> songs = new List<SpotifySong>();

            foreach (var node in tracks)
            {
                BitmapImage picture;
                if (ShallFetchThumbnail)
                {
                    ///find album info
                    //string thunbnailSearch = "https://embed.spotify.com/oembed/?url=spotify:track:07q6QTQXyPRCf7GbLakRPr" + node.uri;
                    //string thunbnailSearch = "https://embed.spotify.com/oembed/?url=" + node.uri;
                    picture = fetchThumbnail(node.uri);
                }
                else
                {
                    //Give a empty picture
                    picture = new BitmapImage();
                }
                //TODO: add real seconds value.
                SpotifySong newSong = new SpotifySong(node.name.ToString(), picture, TimeSpan.FromSeconds(1), new Uri(node.uri.ToString())) { Artist = node.author };

                songs.Add(newSong);
            }
            return songs;
        }

        /// <summary>
        /// returns info about song from the xml source document retrived in constructor.
        /// </summary>
        /// <param name="from">index position to start</param>
        /// <param name="to">index to end at.</param>
        /// <returns>Returns a new dynamic type compromising of name and uri.</returns>
        private dynamic getSearchResult(int from, int to)
        {
            //string searchUrl = "http://ws.spotify.com/search/1/track?q=" + SearchWord;
            return (from node in Source.Descendants("{" + "http://www.spotify.com/ns/music/1" + "}track")
                    select new
                    {
                        name = (string)node.Element("{" + "http://www.spotify.com/ns/music/1" + "}name"),
                        uri = (string)node.Attribute("href"),
                        author = (string)node.Element("{" + "http://www.spotify.com/ns/music/1" + "}artist").Element("{" + "http://www.spotify.com/ns/music/1" + "}name"),
                    }).Skip(from).Take(to);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thunbnailSearch">must be in the form of "spotify:track:############"</param>
        /// <returns>Picture repesenting a album/track</returns>
        public static BitmapImage fetchThumbnail(string trackUri)
        {
            Uri search = new Uri("https://embed.spotify.com/oembed/?url=" + trackUri);
            var json_data = string.Empty;
            //XDocument trackinfo = XDocument.Load(thunbnailSearch);
            //var uri = (from info in trackinfo.Descendants("thumbnail_url")
            //           select (string)info);

            using (var w = new WebClient())
            {
                // attempt to download JSON data as a string
                w.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
                json_data = w.DownloadString(search);

            }
            string myType = GetFirstInstance<string>("thumbnail_url", json_data);

            string uri = myType;
            /*
            //find album picture http://stackoverflow.com/questions/12142634/download-image-and-create-bitmap
            WebRequest req1 = WebRequest.Create(new Uri(uri));
            WebResponse res1 = req1.GetResponse();
            Stream stream1 = res1.GetResponseStream();
            BitmapImage picture = new BitmapImage();
            picture.BeginInit();
            picture.StreamSource = stream1;
            picture.EndInit();
            stream1.Close();

            picture.BaseUri = new Uri(uri);
            return picture;
             */
            return new BitmapImage(new Uri(uri));
        }

        /// <summary>
        /// http://stackoverflow.com/questions/19438472/json-net-deserialize-a-specific-property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        private static T GetFirstInstance<T>(string propertyName, string json)
        {
            using (var stringReader = new StringReader(json))
            using (var jsonReader = new JsonTextReader(stringReader))
            {
                while (jsonReader.Read())
                {
                    if (jsonReader.TokenType == JsonToken.PropertyName
                        && (string)jsonReader.Value == propertyName)
                    {
                        jsonReader.Read();

                        var serializer = new JsonSerializer();
                        return serializer.Deserialize<T>(jsonReader);
                    }
                }
                return default(T);
            }
        }
    }
}
