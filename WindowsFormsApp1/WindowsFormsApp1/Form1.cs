using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BassLike.Stop();
            timer1.Enabled = false;
            sltime.Value = 0;
            label1.Text = "00:00:00";
        }

        private void btnEj_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            Vars.Files.Add(openFileDialog1.FileName);
            playlist.Items.Add(Vars.GetFileName(openFileDialog1.FileName));
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if ((playlist.Items.Count != 0) && (playlist.SelectedIndex != -1))
            {
                string current = Vars.Files[playlist.SelectedIndex];
                BassLike.Play(current, BassLike.Volume);
                label1.Text = TimeSpan.FromSeconds(BassLike.GetPosOfStream(BassLike.Stream)).ToString();
                label2.Text = TimeSpan.FromSeconds(BassLike.GetTimeOfStream(BassLike.Stream)).ToString();
                sltime.Maximum = BassLike.GetTimeOfStream(BassLike.Stream);
                sltime.Value = BassLike.GetPosOfStream(BassLike.Stream);
                timer1.Enabled = true;

            }


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = TimeSpan.FromSeconds(BassLike.GetPosOfStream(BassLike.Stream)).ToString();
            sltime.Value = BassLike.GetPosOfStream(BassLike.Stream);
        }

        private void sltime_Scroll(object sender, ScrollEventArgs e)
        {
            BassLike.SetPosOfScroll(BassLike.Stream, sltime.Value);
        }

        private void slVol_Scroll(object sender, ScrollEventArgs e)
        {
            BassLike.SetVolumeToStream(BassLike.Stream, slVol.Value);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BassLike.Pause();
            
        }
    }
}
