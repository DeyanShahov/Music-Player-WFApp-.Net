using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using WMPLib;

namespace Music_Player_WFApp.Net
{
    public partial class Form1 : Form
    {
        List<string> files = new List<string>();
        //WindowsMediaPlayer wmp;
        public Form1()
        {
            InitializeComponent();
            axWindowsMediaPlayer.settings.volume = 50;
            lblVolumeMax.Text = "50 %";
            //wmp = (WindowsMediaPlayer)axWindowsMediaPlayer.GetOcx();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Multiselect = true;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        //files = ofd.FileNames;
                        listBoxTrackList.Items.Clear();
                        files.AddRange(ofd.FileNames);
                        for (int i = 0; i < files.Count; i++)
                        {
                           listBoxTrackList.Items.Add(files[i]);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Unable to load file!\n\n" + ex.Message); 
                    }
                }
            }
        }



        private void listBoxTrackList_SelectedIndexChanged(object sender, EventArgs e)
        {
            axWindowsMediaPlayer.URL = files[listBoxTrackList.SelectedIndex];
            axWindowsMediaPlayer.Ctlcontrols.play();

            //wmp.settings.setMode("Audio", true);
            //wmp.currentPlaylist.appendItem(wmp.mediaCollection.getByName("Bars"));

            //Set picture Album from selected song ... Work only if contains this image
            try
            {
                var file = TagLib.File.Create(files[listBoxTrackList.SelectedIndex]);
                var bin = (byte[])(file.Tag.Pictures[0].Data.Data);
                pictureBoxArt.Image = Image.FromStream(new MemoryStream(bin));
            }
            catch
            {
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer.Ctlcontrols.stop();
            progressBar.Value = 0;
        }

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
            if (axWindowsMediaPlayer.playState == WMPPlayState.wmppsPlaying)
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

        private void btnClear_Click(object sender, EventArgs e) => listBoxTrackList.Items.Clear();
 
    }
}
