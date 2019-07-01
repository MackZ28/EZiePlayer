using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using Un4seen.Bass;

namespace EZiePlayer 
{
    
    public partial class MainWindow 
    {
        public double volume;
        private bool dragStarted = false;

        Playerlogic player = new Playerlogic();
        DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            player.InitBass(Playerlogic.HZ);
            timer.Tick += timer_Tick;
            timer.Start();
        }


        private void timer_Tick(object sender, EventArgs e)
        {
            if(Playerlogic.Stream != 0) 
           {
                sliderPos.Minimum = 0;
                sliderPos.Maximum = Bass.BASS_ChannelGetLength(Playerlogic.Stream);
                sliderPos.Value = Bass.BASS_ChannelGetPosition(Playerlogic.Stream);
           }
        }

        private void Source_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog()
            {
                Filter = "MP3|*.mp3|Любой файл|*",
                Multiselect = true,
            };
            if (OFD.ShowDialog() == true)
            {
                foreach (String fileName in OFD.FileNames) {
                    TrackList.Files.Add(fileName);
                    playlist.Items.Add(TrackList.GetFileName(fileName));
                }  
            }
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if ((playlist.Items.Count != 0) && (playlist.SelectedIndex != -1))
            {
                string current = TrackList.Files[playlist.SelectedIndex];
                player.Play(current, player.Volume); 
            }
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            player.Pause();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            player.Stop();
        }
        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            if (playlist.Items.Count != 0)
            {
                string current = TrackList.Files[playlist.SelectedIndex];
                player.Play(current, player.Volume);
            }
        }

        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            if (playlist.Items.Count != 0)
            {
                string current = TrackList.Files[playlist.SelectedIndex + 1];
                player.Play(current, player.Volume);
            }
        }

        private void Mute_Click(object sender, RoutedEventArgs e)
        {
            if (player.Volume != 0)
            {
                player.SetVolumeToStream(Playerlogic.Stream, 0);
                volume = slVol.Value;
                slVol.Value = 0;
            }
            else
            {
                player.SetVolumeToStream(Playerlogic.Stream, (int)volume);
                slVol.Value = volume;
            }
        }

        private void SlVol_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            player.SetVolumeToStream(Playerlogic.Stream, (short)e.NewValue);
        }

        private void SliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
           //lblProgressStatus.Text = TimeSpan.FromMilliseconds(sliderPos.Value).ToString(@"hh\:mm\:ss");
            lblProgressStatus.Text =player.GetPosOfStream(Playerlogic.Stream).ToString("HH:mm:ss");
        }

        private void SliderPos_DragStarted(object sender, DragStartedEventArgs e)
        {
            timer.Stop();
            player.Pause();
        }

        private void SliderPos_DragCompleted(object sender, DragCompletedEventArgs e)
        {

            Bass.BASS_ChannelSetPosition(Playerlogic.Stream, sliderPos.Value);
            
        }
    }
}
