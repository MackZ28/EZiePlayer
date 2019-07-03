using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;
using Un4seen.Bass;

namespace EZiePlayer 
{
    
    public partial class MainWindow 
    {
        public double volume;
        int posOfSelector = 0;
        int numOfTrack = 1;

        Playerlogic player = new Playerlogic();
        DispatcherTimer timer = new DispatcherTimer();

        

        public MainWindow()
        {
            InitializeComponent();
            player.InitBass(Playerlogic.HZ);
            timer.Tick += timer_Tick;
            
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
                    TagModel tm = new TagModel(fileName);
                    playlist.Items.Add(numOfTrack + ". "+ tm.Artist +" - " + tm.Title + " " + "\n   " + tm.BitRate + " kbps " + " - " + tm.Album);
                    numOfTrack++;
                }  
            } 

            if (playlist.Items.Count > 0 && Playerlogic.Stream == 0)
            {
                playlist.SelectedIndex = 0;
                Play();
               
            }

        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if ((playlist.Items.Count != 0) && (playlist.SelectedIndex != -1))
            {
                Play();
            }
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            player.Pause();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            player.Stop();
            sliderPos.Value = 0;

        }
        
        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            posOfSelector--;
            if (playlist.Items.Count != 0 && playlist.Items.Count > posOfSelector && posOfSelector > -1 )
            {
                playlist.SelectedIndex = posOfSelector;
                Play();
            } else posOfSelector = playlist.Items.Count;
        }
        
        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            posOfSelector++;
            if (playlist.Items.Count != 0 && playlist.Items.Count > posOfSelector)
            {
                playlist.SelectedIndex = posOfSelector;
                Play();
            } else posOfSelector = -1;
            
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
            Artist.Text = tm.Artist;
            Track.Text = tm.Title;
        }

        private void SliderPos_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Playerlogic.GetPosOfScroll(Playerlogic.Stream, sliderPos.Value);
            Play();
        }
    }
}
