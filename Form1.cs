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
using Music_Player_WFApp.Net.Properties;
using System.Runtime.Remoting.Channels;

namespace Music_Player_WFApp.Net
{
    public partial class Form1 : Form
    {
        List<string> filesPath = new List<string>();
        bool loop = false;
        bool shuffle = false;
        Bitmap basicSongFoto;
        //WindowsMediaPlayer wmp;
        public Form1()
        {
            InitializeComponent();
            axWindowsMediaPlayer.settings.volume = 50;
            axWindowsMediaPlayer.uiMode = "none";
            lblVolumeMax.Text = "50 %";
            basicSongFoto = Properties.Resources.pngwing_com;
            //wmp = (WindowsMediaPlayer)axWindowsMediaPlayer.GetOcx();

        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Multiselect = true;

                var result = ofd.ShowDialog();

                if (result == DialogResult.Cancel || result == DialogResult.None) return;

                if (result == DialogResult.OK)
                {
                    try
                    {
                        //files = ofd.FileNames;
                        listBoxTrackList.Items.Clear();
                        filesPath.AddRange(ofd.FileNames);
                        for (int i = 0; i < filesPath.Count; i++)
                        {
                            listBoxTrackList.Items.Add(filesPath[i]);
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
            axWindowsMediaPlayer.URL = filesPath[listBoxTrackList.SelectedIndex];
            axWindowsMediaPlayer.Ctlcontrols.play();

            //wmp.settings.setMode("Audio", true);
            //wmp.currentPlaylist.appendItem(wmp.mediaCollection.getByName("Bars"));

            //Set picture Album from selected song ... Work only if contains this image
            try
            {
                var file = TagLib.File.Create(filesPath[listBoxTrackList.SelectedIndex]);
                var bin = (byte[])(file.Tag.Pictures[0].Data.Data);
                pictureBoxArt.Image = Image.FromStream(new MemoryStream(bin));
            }
            catch
            {
                pictureBoxArt.Image = basicSongFoto;
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            listBoxTrackList.Items.Clear();
            filesPath.Clear();
        }

        private void btnFulScreen_Click(object sender, EventArgs e)
        {
            if (filesPath.Count > 0) axWindowsMediaPlayer.fullScreen = true;
        }

        private void axWindowsMediaPlayer_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (axWindowsMediaPlayer.playState == WMPLib.WMPPlayState.wmppsStopped)
            {
                if (loop) axWindowsMediaPlayer.Ctlcontrols.play();
                else if (listBoxTrackList.SelectedIndex < listBoxTrackList.Items.Count - 1)
                {
                    listBoxTrackList.SelectedIndex = listBoxTrackList.SelectedIndex + 1;
                }

                //axWindowsMediaPlayer.Ctlcontrols.next();
            }
        }

        private void btnLoop_Click(object sender, EventArgs e)
        {
            if (loop)
            {
                loop = false;
                btnLoop.BackColor = Color.Black;
            }
            else
            {
                loop = true;
                btnLoop.BackColor = Color.Gray;
            }
        }

        private void btnShuffle_Click(object sender, EventArgs e)
        {
            if (shuffle)
            {
                shuffle = false;
                btnShuffle.BackColor = Color.Black;
            }
            else
            {
                shuffle = true;
                btnShuffle.BackColor = Color.Gray;
            }
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            if (panelFavorit.Visible == true)
            {
                btnList.BackColor = Color.Black;
                panelFavorit.Visible = false;
            }
            else
            {
                btnList.BackColor = Color.Gray;
                panelFavorit.Visible = true;
            }
        }

        private void btnAddFavorite_Click(object sender, EventArgs e)
        {
            //if (listBoxFavoriteList.)
                if (filesPath.Count > 0 && !string.IsNullOrEmpty(listBoxTrackList.Text)) listBoxFavoriteList.Items.Add(listBoxTrackList.Text);
        }

        private void listBoxFavoriteList_SelectedIndexChanged(object sender, EventArgs e)
        {
            axWindowsMediaPlayer.URL = listBoxFavoriteList.Text;
            axWindowsMediaPlayer.Ctlcontrols.play();

            //Set picture Album from selected song ... Work only if contains this image
            //try
            //{
            //    var file = TagLib.File.Create(filesPath[listBoxTrackList.SelectedIndex]);
            //    var bin = (byte[])(file.Tag.Pictures[0].Data.Data);
            //    pictureBoxArt.Image = Image.FromStream(new MemoryStream(bin));
            //}
            //catch
            //{
            //    pictureBoxArt.Image = basicSongFoto;
            //}
        }
    }
}
