using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using System.Runtime.InteropServices;

using Pyxie.Memory;
using Pyxie.FFXIStructures;
using System.Threading;


namespace Pyxie
{
    public partial class Player
    {
        public Player()
        {
            this.Settings = new PlayerSettings();
        }

        public Player(System.Diagnostics.Process FFXI)
        {
            this.Id = FFXI.Id;
            this.Name = FFXI.MainWindowTitle;

            foreach(ProcessModule Module in FFXI.Modules)
            {
                if(Module.ModuleName == "FFXiMain.dll")
                {
                    this.SignatureScanner = new SigScan(FFXI, Module.BaseAddress, Module.ModuleMemorySize);
                    break;
                }
            }

            this.Active = true;

            this.Settings = new PlayerSettings();

            this.MemoryHandler = new MemHandler(FFXI);

            this.FindSignatures();

            this.LoadSettings();

            //Create and initialize threads

            this.DetectionThread = new Thread(new ThreadStart(AutoDetection));
            this.DetectionThread.IsBackground = true;
            this.DetectionThread.Start();

            this.StatusThread = new Thread(new ThreadStart(UpdateStatus));
            this.StatusThread.IsBackground = true;
            this.StatusThread.Start();

            this.FlagThread = new Thread(new ThreadStart(UpdateFlags));
            this.FlagThread.IsBackground = true;
            this.FlagThread.Start();

            this.SpeedThread = new Thread(new ThreadStart(UpdateSpeed));
            this.SpeedThread.IsBackground = true;
            this.SpeedThread.Start();

            this.JaZeroThread = new Thread(new ThreadStart(UpdateJaZero));
            this.JaZeroThread.IsBackground = true;
            this.JaZeroThread.Start();



            this.Update();
        }

        public void Destroy()
        {
            this.Active = false;
        }

        public void Update()
        {
            var index = (short)MemoryHandler.ResolvePointer(MemoryHandler.ResolvePointer(this.MobArrayPosition) + 4);

            this.PlayerEntity.UpdateWithAddress(MemoryHandler.ResolvePointer(MobArray + 4 * index));
            this.PlayerDisplay.UpdateWithAddress(PlayerEntity.Display);
            this.PlayerBuffs.UpdateWithAddress(BuffPtr);

            int ZoneTemp = (int) MemoryHandler.ResolvePointer(MemoryHandler.ResolvePointer(ZonePtr));

            if (ZoneTemp > 0xFF)
                Zone = ZoneTemp - 0x1BC;
            else
                Zone = ZoneTemp;

        }

        public override String ToString()
        {
            return this.Name;
        }


        #region "Settings Handling"

        public void LoadSettings()
        {
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Players"))
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\Players");

            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Players\\" + Name + ".xml"))
                return;

            try
            {
                using (var reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\Players\\" + Name + ".xml"))
                {
                    var serializer = new XmlSerializer(typeof(PlayerSettings));
                    this.Settings = (PlayerSettings)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Pyxie was unable to load your player configuration file: \r\n\r\n" + ex.ToString());
                return;
            }
        }

        public void SaveSettings()
        {
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Players"))
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\Players");

            try
            {
                using (StreamWriter streamWriter = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\Players\\" + Name + ".xml"))
                {
                    var serializer = new XmlSerializer(typeof(PlayerSettings));
                    serializer.Serialize(streamWriter, this.Settings);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Pyxie was unable to save your player configuration file:\r\n\r\n" + ex.ToString());
            }
        }

        #endregion


        #region "Properties"

        public int Id { get; set; }
        public String Name { get; set; }

        public Boolean Active { get; set; }

        public Entity PlayerEntity { get; set; }

        public Display PlayerDisplay { get; set; }

        public Buffs PlayerBuffs { get; set; }

        public PlayerSettings Settings { get; set; }

        public SigScan SignatureScanner { get; set; }

        public MemHandler MemoryHandler { get; set; }

        public Entity PlayerStruct { get; set; }

        public int Zone { get; set; }

        public int PreviousZone { get; set; }

        #endregion


    }

}
