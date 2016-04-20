﻿using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using CacheViewer.Domain;
using CacheViewer.Domain.Archive;
using CacheViewer.Domain.Factories;
using CacheViewer.Domain.Models;
using NAudio.Wave;
using NLog;

namespace CacheViewer.Controls
{
    public partial class SoundControl : UserControl
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly SoundCache soundArchive;

        public SoundControl()
        {
            this.InitializeComponent();

            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                if (!Directory.Exists("./Sounds/"))
                {
                    Directory.CreateDirectory("./Sounds/");
                }

                this.soundArchive = (SoundCache)ArchiveFactory.Instance.Build(CacheFile.Sound, true);
                //this.LoadSoundArchive();
            }
        }

        private void SaveToWavButtonClick(object sender, EventArgs e)
        {
            try
            {

                var id = (int)this.SoundsDataGrid.SelectedRows[0].Cells[1].Value;
                var cacheIndex = this.soundArchive.CacheIndices.Find(x => x.Identity == id);
                byte[] data = this.soundArchive[cacheIndex.Identity].Item1.Array;
                Sound sound = new Sound(data);


                // for mono 16 bit audio captured at 16kHz
                WaveFormat format = new WaveFormat(sound.Bitrate, sound.Frequency, 1);
                string fileName = "./Sounds/" + cacheIndex.Identity + ".wav";

                while (File.Exists(fileName))
                {
                    fileName = fileName.Substring(fileName.Length - 4) + "1.wav";
                }

                using (WaveFileWriter writer = new WaveFileWriter(fileName, format))
                {
                    // todo
                    writer.Write(sound.Buffer, 0, sound.Buffer.Length);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }

        private void SaveAllSoundsButtonClick(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < this.soundArchive.CacheIndices.Count; i++)
                {
                    var cacheIndex = this.soundArchive.CacheIndices[i];
                    byte[] data = this.soundArchive[cacheIndex.Identity].Item1.Array;
                    Sound sound = new Sound(data);

                    // for mono 16 bit audio captured at 16kHz

                    //  Creates a new PCM format with the specified sample rate, bit depth and channels
                    // public WaveFormat(int bitrate, int frequency, int channels);

                    WaveFormat format = new WaveFormat(sound.Bitrate, sound.Frequency, sound.NumberOfChannels);

                    string fileName = "./Sounds/" + cacheIndex.Identity + ".wav";

                    if (File.Exists(fileName))
                    {
                        try
                        {
                            File.Delete(fileName);
                        }
                        catch(Exception es)
                        {
                            logger.Error(es, es.Message);
                            fileName = cacheIndex.Identity + DateTime.Now.Ticks + ".wav";
                        }
                    }

                    using (WaveFileWriter writer = new WaveFileWriter(fileName, format))
                    {
                        this.SavingSoundFileLabel.Text = DomainMessages.
                            SoundControl_SaveAllSoundsButtonClick_Saving_sound_file_ + fileName;

                        writer.Write(sound.Buffer, 0, sound.Buffer.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }

        private void PlaySoundFileButtonClick(object sender, EventArgs e)
        {
            var id = (int)this.SoundsDataGrid.SelectedRows[0].Cells[1].Value;
            var cacheIndex = this.soundArchive.CacheIndices.Find(x=>x.Identity == id);
            byte[] data = this.soundArchive[cacheIndex.Identity].Item1.Array;
            Sound sound = new Sound(data);

            IWaveProvider provider = new RawSourceWaveStream(
                new MemoryStream(sound.Buffer), 
                new WaveFormat(sound.Bitrate, sound.Frequency, sound.NumberOfChannels));
            WaveOut waveOut = new WaveOut();
            waveOut.Init(provider);
            waveOut.Play();
            // uint version = 0;
            // CREATESOUNDEXINFO createsoundexinfo = new CREATESOUNDEXINFO();
            // RESULT result = Factory.System_Create(ref this.fmodSystem);

            // Errcheck(result);
            // result = fmodSystem.getVersion(ref version);
            // Errcheck(result);

            // result = fmodSystem.init(2, INITFLAGS.NORMAL, (IntPtr)null);

            // Errcheck(result);

            // createsoundexinfo.cbsize = Marshal.SizeOf(createsoundexinfo);
            // createsoundexinfo.length = (uint)this.sound.Buffer.Length;
            // createsoundexinfo.fileoffset        = 0;
            // //createsoundexinfo.length            = frequency * channels * 2 * 2;
            // createsoundexinfo.numchannels       = this.sound.NumberOfChannels;
            // createsoundexinfo.defaultfrequency  = this.sound.Frequency;
            // createsoundexinfo.format            = SOUND_FORMAT.NONE;

            // result = fmodSystem.createSound(this.sound.SoundDataLength.ToString(CultureInfo.InvariantCulture), 
            //     (this.mode | MODE.CREATESTREAM), ref createsoundexinfo, ref fmodSound);

            //Errcheck(result);
            //fmodSystem.playSound(CHANNELINDEX.FREE, fmodSound, false, ref channel);

        }

        private void SoundsDataGridRowEnter(object sender, DataGridViewCellEventArgs e)
        {
            //try
            //{
            //    this.sound = (Sound) this.SoundsDataGrid.Rows[e.RowIndex].Cells["SongObject"].Value;
            //    this.cacheIndex = this.soundArchive.CacheIndices[(int) this.SoundsDataGrid.Rows[e.RowIndex].Cells["Identity"].Value];
            //    this.SelectedSoundLabel.Text = "Selected Sound File: " + this.cacheIndex.identity;
            //    this.FrequencyLabel.Text = "Frequency: " + this.sound.Frequency;
            //    this.BitrateLabel.Text = "Bitrate: " + this.sound.Bitrate;
            //    this.LengthLabel.Text = "Length ( in bytes ): " + this.sound.SoundDataLength;
            //}
            //catch (Exception ex)
            //{
            //    logger.ErrorException(ex.Message, ex);
            //}
        }

        private async void LoadCacheButton_Click(object sender, EventArgs e)
        {
            this.SoundsDataGrid.Rows.Clear();
            await this.LoadSoundArchive();
        }



        private async Task LoadSoundArchive()
        {
            await Task.Run(() =>
            {
                for (int i = 0; i < this.soundArchive.CacheIndices.Count; i++)
                {
                    var cacheIndex = this.soundArchive.CacheIndices[i];

                    var values = new object[8];
                    //Sound s = new Sound(this.soundArchive[cacheIndex.identity].Item1.Array);

                    //int n = this.SoundsDataGrid.Rows.Add();

                    values[0] = i;
                    values[1] = cacheIndex.Identity;
                    //values[2] = s.Bitrate;
                    //values[3] = s.Frequency;
                    //values[4] = s.NumberOfChannels;
                    //values[5] = s.SoundDataLength;
                    //values[6] = s;

                    SetRenderItem(this.SoundsDataGrid, values);
                }
            });
        }

        private void SetRenderItem(DataGridView soundsDataGrid, object[] values)
        {
            if (soundsDataGrid.InvokeRequired)
            {
                soundsDataGrid.BeginInvoke(new MethodInvoker(() => SetRenderItem(soundsDataGrid, values)));
            }
            else
            {

                int n = this.SoundsDataGrid.Rows.Add();
                this.SoundsDataGrid.Rows[n].Cells[0].Value = values[0];
                this.SoundsDataGrid.Rows[n].Cells[1].Value = values[1];
                this.SoundsDataGrid.Rows[n].Cells[2].Value = values[2];
                this.SoundsDataGrid.Rows[n].Cells[3].Value = values[3];
                this.SoundsDataGrid.Rows[n].Cells[4].Value = values[4];
                this.SoundsDataGrid.Rows[n].Cells[5].Value = values[5];
                //this.SoundsDataGrid.Rows[n].Cells[6].Value = values[6];
                soundsDataGrid.Refresh();
            }
        }


    }
}
