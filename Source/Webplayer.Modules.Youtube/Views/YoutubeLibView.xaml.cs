using Infrastructure.Service;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VideoLibrary;
using Webplayer.Modules.Youtube.ViewModels;
using Timer = System.Timers.Timer;

namespace Webplayer.Modules.Youtube.Views
{
    /// <summary>
    /// Interaction logic for YoutubeLibView.xaml
    /// </summary>
    public partial class YoutubeLibView : IVIdeoInfoView
    {
        private const string Tag = "YoutubeLibView";
        private ILoggerFacade _logger;
        private IVideoInfoViewModel _vm;
        private IDialogService _dialogService;
        private Timer _progressTimer;

        public YoutubeLibView(ILoggerFacade logger, IDialogService dialogService)
        {
            _logger = logger;
            _dialogService = dialogService;
            DataContextChanged += YoutubeLibView_DataContextChanged;
            InitializeComponent();
            Player.MediaEnded += Player_MediaEnded;
            BusyIndicator.Visibility = Visibility.Hidden;
            Player.MediaOpened += Player_MediaOpened;

            Progress.PreviewMouseUp += Progress_PreviewMouseRightButtonUp;
            _progressTimer = new Timer(1000);
            _progressTimer.Elapsed += _progressTimer_Elapsed;
            _progressTimer.AutoReset = true;
            _progressTimer.Start();

            Volume.PreviewMouseUp += Volume_PreviewMouseUp;
        }

        private void Volume_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Player.Volume = Volume.Value;
        }

        private void Progress_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            int pos = Convert.ToInt32(Progress.Value);
            Player.Position = new TimeSpan(0, 0, 0, pos, 0);
        }

        private void _progressTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() => Progress.Value = (int)Player.Position.TotalSeconds);
        }

        private void Player_MediaOpened(object sender, RoutedEventArgs e)
        {
            Progress.Value = 0;
            Progress.Maximum = (int)Player.NaturalDuration.TimeSpan.TotalSeconds;
            Player.Volume = Volume.Value;
        }

        private void Player_MediaEnded(object sender, RoutedEventArgs e)
        {
            if(_vm != null)
            {
                if (_vm.NextCommand.CanExecute(null))
                {
                    _vm.NextCommand.Execute(null);
                }
            }
        }

        private void YoutubeLibView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _vm = e.NewValue as IVideoInfoViewModel;
            if(_vm != null)
            {
                _vm.PropertyChanged += VmPropertyChanged;
            }
        }

        private void VmPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //Make sure to run on ui thread
            Dispatcher.Invoke(async () =>
            {
                if (e.PropertyName == nameof(IVideoInfoViewModel.Video))
                {
                    await SetSong(_vm.Video.VideoId);
                }
                else if(e.PropertyName == nameof(IVideoInfoViewModel.Playing))
                {
                    if (_vm.Playing)
                        Player.Play();
                    else
                        Player.Pause();
                }
            });
        }

        private async Task SetSong(string videoId)
        {
            try
            {
                Player.Visibility = Visibility.Hidden;
                BusyIndicator.Visibility = Visibility.Visible;
                using (var service = Client.For(YouTube.Default))
                {
                    _logger.Log($"{Tag} Downloading song {videoId}", Category.Info, Priority.Low);
                    var video = await service.GetVideoAsync("https://youtube.com/watch?v=" + videoId);


                    var tempFolder = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "youtubecache");
                    Directory.CreateDirectory(tempFolder);
                    var path = System.IO.Path.Combine(tempFolder, videoId + video.FileExtension);

                    if (!File.Exists(path))
                    {
                        _logger.Log($"{Tag} saving song to {path}", Category.Info, Priority.Low);
                        File.WriteAllBytes(path, await video.GetBytesAsync());
                    }
                    
                    _logger.Log($"{Tag} setting uro for player", Category.Info, Priority.Low);
                    Dispatcher.Invoke(() => { Player.Source = new Uri(path, UriKind.Absolute); });
                }
            }
            catch (Exception ex)
            {
                _logger.Log($"{Tag} Youtube video fetch error {ex}", Category.Exception, Priority.High);
                _dialogService.ShowError($"Youtube player error",ex.ToString());
            }
            finally
            {
                Player.Visibility = Visibility.Visible;
                BusyIndicator.Visibility = Visibility.Hidden;
            }
        }
    }
}
