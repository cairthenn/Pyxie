using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pyxie
{
    public partial class Player
    {
        internal const float SPEED_MAX = 32.0f;
        internal const float SPEED_BASE = 5.0f;
        internal const float SPEED_FLEE = 6.0f;
        internal const float SPEED_CHOCOBO = 8.0f;
        
        // Used for speed writer NOP
        internal static byte[] DefaultSpeedBytes = new byte[6];

        public void UpdateSpeed()
        {
            bool ChangedSpeed = false;

            while(Active && !Globals.Exiting)
            {
                if (Settings.UseSpeed)
                {
                    this.Update();

                    if (Globals.Instance.Pyxie.UseChocoboSpeed && PlayerBuffs.BuffList.Any(b => Buffs.Lookup[b].Contains("Chocobo")))
                    {
                        Speed = SPEED_CHOCOBO;
                        ChangedSpeed = true;
                    }
                    else if (Settings.UseDetection && Detected)
                    {
                        if (!Globals.Instance.Pyxie.UseBoundDetectedMovement &&
                            PlayerBuffs.BuffList.Any(b => Buffs.Lookup[b].Contains("bind")))
                        {
                            Speed = 0;
                            ChangedSpeed = true;
                        }
                        else if (Settings.DetectedSpeed != Speed)
                        {
                            if (PlayerBuffs.BuffList.Any(b => Buffs.Lookup[b].Contains("Flee")))
                            {
                                Speed = SPEED_FLEE > Settings.DetectedSpeed ? SPEED_FLEE : Settings.DetectedSpeed;
                                ChangedSpeed = true;
                            }
                            else
                            {
                                Speed = Settings.DetectedSpeed;
                                ChangedSpeed = true;
                            }
                        }
                    }
                    else
                    {
                        if (!Globals.Instance.Pyxie.UseBoundMovement &&
                            PlayerBuffs.BuffList.Any(b => Buffs.Lookup[b].Contains("bind")))
                        {
                            Speed = 0;
                            ChangedSpeed = true;
                        }
                        else
                        {
                            Speed = Settings.Speed;
                            ChangedSpeed = true;
                        }
                    }
                }
                else if(ChangedSpeed)
                {
                    Speed = SPEED_BASE;
                    ChangedSpeed = false;
                }

                Thread.Sleep(100);
            }

            
        }


        #region =="Properties"

        /// <summary>
        /// Wrapper to write proper values to memory
        /// </summary>
        /// <param name="speed"></param>
        public float Speed
        {
            get
            {
                return PlayerEntity.Speed;
            }
            set
            {
                if (value < SPEED_BASE && value != 0)
                    this.PlayerEntity.Speed = SPEED_BASE;
                else if (value > SPEED_MAX)
                    this.PlayerEntity.Speed = SPEED_MAX;
                else
                    this.PlayerEntity.Speed = value;

            }
        }

        /// <summary>
        /// A thread to update a player's movement.
        /// </summary>
        public Thread SpeedThread { get; set; }

        #endregion
    }
}
