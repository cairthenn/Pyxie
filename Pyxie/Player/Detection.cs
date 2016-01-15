using Pyxie.FFXIStructures;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pyxie
{
    public partial class Player : INotifyPropertyChanged
    {
        public void AutoDetection()
        {

            while(Active && !Globals.Exiting)
            {
                if (Settings.UseDetection)
                {
                    this.Update();

                    if (Globals.Instance.Pyxie.ExcludedZones.Any(z => Zones.Instance.ZoneMap[z].Contains(Zone)))
                    {
                        // Excluded Zone: No Detection
                        this.Detected = false;
                        this.DetectedText = "Zone Exclusion";
                    }
                    else if (Globals.Instance.Pyxie.IncludedZones.Any(z => Zones.Instance.ZoneMap[z].Contains(Zone)))
                    {
                        // Included Zone: Always Detect
                        this.Detected = true;
                        this.DetectedText = "Zone Inclusion";
                    }
                    else if (PreviousZone != Zone)
                    {
                        if (Globals.Instance.Pyxie.UseZoneDelay)
                        {
                            this.Detected = true;

                            if (this.PreviousZone != -1)
                            {
                                for (int i = Globals.Instance.Pyxie.ZoneDelay; i > 0; i--)
                                {
                                    this.DetectedText = String.Format("Zone Delay: {0}", i);
                                    Thread.Sleep(1000);
                                }
                            }
                        }

                        this.PreviousZone = this.Zone;
                    }
                    else if (Globals.Instance.Pyxie.DetectionWithoutInvisible &&
                        !PlayerBuffs.BuffList.Any(b => Buffs.Lookup[b].Contains("Invisible")))
                    {
                        this.Detected = true;
                        this.DetectedText = "Invisible Not Active";
                    }
                    else if (Globals.Instance.Pyxie.DetectionWithoutSneak &&
                        !PlayerBuffs.BuffList.Any(b => Buffs.Lookup[b].Contains("Sneak")))
                    {
                        this.Detected = true;
                        this.DetectedText = "Sneak Not Active";
                    }
                    else if (CheckForPlayers())
                    {
                        this.Detected = true;
                    }
                    else
                    {
                        this.Detected = false;
                    }
                }

                Thread.Sleep(100);
            }        
        }

            /// <summary>
        /// Loops through mob array to check for nearby players
        /// </summary>
        /// <returns>True if a player is found.</returns>
        public bool CheckForPlayers()
        {
            Exclusion = false;

            for(int index = 0; index <= NPC_MAP_SIZE; index++)
            {
                Entity Check = GetEntityByIndex(index);

                if(Check != null)
                {                    
                    if(!Check.IsNPC && Check.Name.Length > 0 && Check.ID != this.PlayerEntity.ID && Check.Distance < 2500 && ((Check.Flags1 & EntityEnum.Flags1.Hidden) == 0))
                    {
                        if(Settings.UseExclusions && Globals.Instance.Pyxie.ExcludedPlayers.Any(n => Check.Name.ToLower().Equals(n.ToLower())))
                        {
                            Exclusion = true;
                            continue;
                        }
                        else                        
                        {
                            this.DetectedText = String.Format("Detected: {0}", Check.Name);
                            return true;
                        }
                    }
                }
            }

           this.DetectedText = "Not Detected";

            return false;
        }

        public Entity GetEntityByIndex(int index)
        {
            IntPtr address = MemoryHandler.ResolvePointer(MobArray + 4 * index);

            if (address == IntPtr.Zero) return null;
            else return new Entity(MemoryHandler, address);
        }

        /// <summary>
        /// Constant referring to the total possible entries in the MobArray.
        /// </summary>
        public const int NPC_MAP_SIZE = 2048;

        #region "Properties"

        private String detectedText;

        public String DetectedText { get { return detectedText; } set { detectedText = value; RaisePropertyChanged(); } }

        /// <summary>
        /// Thread for detection.
        /// </summary>
        public Thread DetectionThread { get; set; }


        /// <summary>
        /// Gets or sets if the auto-detection was triggered.
        /// </summary>
        public Boolean Detected { get; set; }

        /// <summary>
        /// Gets or sets if an exclusion was found.
        /// </summary>
        public Boolean Exclusion { get; set; }

        #endregion

        #region "Implement INotifyPropertyChanged"
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }
        #endregion
    }


}
