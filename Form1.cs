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
    }
}
