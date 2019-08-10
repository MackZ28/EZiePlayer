using System;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Un4seen.Bass;

namespace EZiePlayer 
{

    public partial class MainWindow
    {
        DispatcherTimer timer = new DispatcherTimer();
        Playerlogic player = new Playerlogic();
        public double volume;
        int posOfSelector = 0;
        int numOfTrack = 1;
        bool isPlaying;

        public MainWindow()
        {
            InitializeComponent();
            player.InitBass(Playerlogic.HZ);
            timer.Tick += timer_Tick;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (Playerlogic.Stream != 0)
            {
                sliderPos.Minimum = 0;
                sliderPos.Maximum = Bass.BASS_ChannelGetLength(Playerlogic.Stream);
                sliderPos.Value = Bass.BASS_ChannelGetPosition(Playerlogic.Stream);

                if (sliderPos.Value == sliderPos.Maximum)
                {
                    posOfSelector++;
                    if (TrackList.Files.Count <= posOfSelector) posOfSelector = 0;
                    playlist.SelectedIndex = posOfSelector;
                    Play();
                } 
            }
        }

        private void Source_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog OFD = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "MP3|*.mp3|Любой файл|*",
                Multiselect = true,
            };
            if (OFD.ShowDialog() == true)
            {
                foreach (String fileName in OFD.FileNames)
                {
                    TrackList.Files.Add(fileName);
                    TagModel tm = new TagModel(fileName);
                    playlist.Items.Add(numOfTrack + ". "  + tm.Artist + " - " + tm.Title + "\n    " + new StringBuilder().Append('\t', 12) + player.GetLengthOfFile(fileName) + "\n  " + " Bitrate: " + tm.BitRate + " kbps     " + " Album: " + tm.Album);
                    numOfTrack++;
                }
            }

            if (playlist.Items.Count > 0 && Playerlogic.Stream == 0)
            {
                playlist.SelectedIndex = 0;
                Play();
                ImagePlay.Source = new BitmapImage(new Uri("Resources/pause.png", UriKind.Relative));
                isPlaying = true;
            }
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (playlist.Items.Count != 0 && playlist.SelectedIndex != -1 && isPlaying == false)
            {
                Play();
                isPlaying = true;
                ImagePlay.Source = new BitmapImage(new Uri("Resources/pause.png", UriKind.Relative));
            }
            else
            {
                player.Pause();
                isPlaying = false;
                ImagePlay.Source = new BitmapImage(new Uri("Resources/play.png", UriKind.Relative));
            }
        }

        private void Playlist_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            posOfSelector = playlist.SelectedIndex;
            Play();
            isPlaying = true;
            ImagePlay.Source = new BitmapImage(new Uri("Resources/pause.png", UriKind.Relative));
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            player.Stop();
            sliderPos.Value = 0;
            timer.Stop();
            isPlaying = false;
            ImagePlay.Source = new BitmapImage(new Uri("Resources/play.png", UriKind.Relative));
            lblProgressStatus.Text = "00:00:00";
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            posOfSelector--;
            if (playlist.Items.Count != 0 && playlist.Items.Count > posOfSelector && posOfSelector > -1)
            {
                playlist.SelectedIndex = posOfSelector;
                Play();
            }
            else posOfSelector = playlist.Items.Count;
        }

        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            posOfSelector++;
            if (playlist.Items.Count != 0 && playlist.Items.Count > posOfSelector)
            {
                playlist.SelectedIndex = posOfSelector;
                Play();
            }
            else posOfSelector = -1;
        }

        private void Mute_Click(object sender, RoutedEventArgs e)
        {
            if (player.Volume != 0)
            {
                player.SetVolumeToStream(Playerlogic.Stream, 0);
                volume = slVol.Value;
                slVol.Value = 0; ;
                ImageMute.Source = new BitmapImage(new Uri("Resources/mute.png", UriKind.Relative));
            }
            else
            {
                player.SetVolumeToStream(Playerlogic.Stream, (int)volume);
                slVol.Value = volume;
                ImageMute.Source = new BitmapImage(new Uri("Resources/volume.png", UriKind.Relative));
            }
        }

        private void SlVol_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            player.SetVolumeToStream(Playerlogic.Stream, (short)e.NewValue);
        }

        private void SliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblProgressStatus.Text = TimeSpan.FromSeconds(player.GetPosOfStream(Playerlogic.Stream)).ToString();

        }

        private void SliderPos_DragStarted(object sender, DragStartedEventArgs e)
        {
            timer.Stop();
            player.Pause();
        }

        private void SliderPos_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            Playerlogic.GetPosOfScroll(Playerlogic.Stream, sliderPos.Value);
            Play();
        }

        public void Play()
        {
            string current = TrackList.Files[playlist.SelectedIndex];
            player.Play(current, player.Volume);
            lblProgressTime.Text = TimeSpan.FromSeconds(player.GetTimeOfStream(Playerlogic.Stream)).ToString();
            timer.Start();
            posOfSelector = playlist.SelectedIndex;
            TagModel tm = new TagModel(TrackList.Files[playlist.SelectedIndex]);
            Artist.Text = "Now playing:  " + tm.Artist + " - " + tm.Title;
        }

        private void MenuItem_Delete(object sender, RoutedEventArgs e)
        {
            playlist.Items.RemoveAt(posOfSelector);
            TrackList.Files.RemoveAt(posOfSelector);
            posOfSelector--;
        }

        private void MenuItem_DeleteAll(object sender, RoutedEventArgs e)
        {
            playlist.Items.Clear();
            TrackList.Files.Clear();
            posOfSelector = 0;
            numOfTrack = 1;
        }
       

        private void SliderPos_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //timer.Stop();
            //player.Pause();
            //Playerlogic.GetPosOfScroll(Playerlogic.Stream, sliderPos.Value);
            //string current = TrackList.Files[playlist.SelectedIndex];
            //player.Play(current, player.Volume);
            
        }
    }
}
