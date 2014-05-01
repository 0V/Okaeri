using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using Microsoft.Win32;

namespace Okaeri
{
    public partial class OkaeriForm : Form
    {
        private string userfileName = "";


        public OkaeriForm()
        {
            InitializeComponent();
            SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);
        }

        private void OkaeriForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            notifyIconOkaeri.Visible = false;
            SystemEvents.PowerModeChanged -= new PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);
        }

        private void menuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            this.Activate();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIconOkaeri.Visible = false;
            try
            {
                Application.Exit();
            }
            catch
            {
                System.Environment.Exit(0);
            }
        }

        private void notifyIconOkaeri_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            this.Activate();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            if((string)comboVoice.SelectedValue == "userfiles"){
                if (!(System.IO.File.Exists(userfileName))){
                    MessageBox.Show("The voice file was not found.\n Please check file exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
                }
            }
            
            this.Visible = false;
            
        }

        private void OkaeriForm_Load(object sender, EventArgs e)
        {

            var voiceTable = new DataTable();
            voiceTable.Columns.Add("ID", typeof(string));
            voiceTable.Columns.Add("NAME", typeof(string));

            var dict = new Dictionary<string, string>(){
                {"okaeri_03.wav","おかえり! [Okaeri!]"},
                {"okaerinasai_02.wav","おかえりなさい [Okaerinasai]"},
                {"ohayo_01.wav","おはよっ [Ohayo]"},
                {"ohayougozaimasu_02.wav","おはようございます [Ohayou gozaimasu]"},
                {"ohayougozaimasu_03.wav","おはようございまーす [Ohayou gozaimaaasu]"},
                {"ohhaa_01.wav","おっはー! [Ohhaa!]"},
                {"ohhaa_02.wav","おっ...はあぁぁ... [O...haaa...]"},
                {"ohhayoo_01.wav","おっはよー! [Ohhayoo!]"},
                {"ohhayoo_02.wav","お...はよぉぉ... [O...hayoo...]"},
                {"ohisashiburi_03.wav","おひさしぶり [Ohisashiburi]"},
                {"ohisashiburidesu_03.wav","おひさしぶりですっ [Ohisashiburidesu]"},
                {"userfiles","選択したファイル [User Select]"}
            };

            foreach (var data in dict)
            {
                var row = voiceTable.NewRow();
                row["ID"] = data.Key;
                row["NAME"] = data.Value;
                voiceTable.Rows.Add(row);
            }
            voiceTable.AcceptChanges();
            comboVoice.DataSource = voiceTable;
            comboVoice.DisplayMember = "NAME";
            comboVoice.ValueMember = "ID";
        }

        private void fileOpenButton_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.Filter = "wavファイル|*.wav";

            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                userfileName = this.openFileDialog1.FileName;
                comboVoice.SelectedValue = "userfiles";
            }
        }

        private void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            if (e.Mode == PowerModes.Resume)
            {
                string directryName = @"voices/";
                try
                {
                    if ((string)comboVoice.SelectedValue == "userfiles")
                        PlayMusic(userfileName);
                    else
                        PlayMusic(directryName + (string)comboVoice.SelectedValue);
                }
                catch { }
            }
        }


        private void PlayMusic(string fileName)
        {
            if (System.IO.File.Exists(fileName))
            {
                new SoundPlayer(fileName).Play();
            }
        }
    }
}