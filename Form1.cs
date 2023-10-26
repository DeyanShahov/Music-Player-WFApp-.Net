using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Music_Player_WFApp.Net
{
    public partial class Form1 : Form
    {
        string[] path;
        string[] files;
        public Form1()
        {
            InitializeComponent();
            axWindowsMediaPlayer.settings.volume = 50;
            lblVolumeMax.Text = "50 %";
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                files = ofd.FileNames;
                path = ofd.FileNames;
                for (int i = 0; i < files.Length; i++)
                {
                    listBoxTrackList.Items.Add(files[i]);
                }
            }
        }

        private void listBoxTrackList_SelectedIndexChanged(object sender, EventArgs e)
        {
            axWindowsMediaPlayer.URL = path[listBoxTrackList.SelectedIndex];
            axWindowsMediaPlayer.Ctlcontrols.play();
        }

        private void btnStop_Click(object sender, EventArgs e) => axWindowsMediaPlayer.Ctlcontrols.stop();

        private void btnPause_Click(object sender, EventArgs e) => axWindowsMediaPlayer.Ctlcontrols.pause();

        private void btnPlay_Click(object sender, EventArgs e) => axWindowsMediaPlayer.Ctlcontrols.play();

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (listBoxTrackList.SelectedIndex < listBoxTrackList.Items.Count - 1)
            {
                listBoxTrackList.SelectedIndex = listBoxTrackList.SelectedIndex + 1;
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (listBoxTrackList.SelectedIndex > 0)
            {
                listBoxTrackList.SelectedIndex = listBoxTrackList.SelectedIndex - 1;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (axWindowsMediaPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                progressBar.Maximum = (int)axWindowsMediaPlayer.Ctlcontrols.currentItem.duration;
                progressBar.Value = (int)axWindowsMediaPlayer.Ctlcontrols.currentPosition;
            }

            try
            {
                lblTrackStart.Text = axWindowsMediaPlayer.Ctlcontrols.currentPositionString;
                lblTrackEnd.Text = axWindowsMediaPlayer.Ctlcontrols.currentItem?.durationString;
            }
            catch
            {
            }
        }

        private void trackBarVolume_Scroll(object sender, EventArgs e)
        {
            axWindowsMediaPlayer.settings.volume = trackBarVolume.Value;
            lblVolumeMax.Text = trackBarVolume.Value.ToString() + " %";
        }

        private void ProgressBarMouseDown(object sender, MouseEventArgs e)
        {
            axWindowsMediaPlayer.Ctlcontrols.currentPosition = axWindowsMediaPlayer.currentMedia.duration * e.X / progressBar.Width;
        }
    }
}
