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
            while(UseSpeed && Active && !Globals.Exiting)
            {
                this.Update();

                if(Globals.Instance.Pyxie.UseChocoboSpeed && PlayerBuffs.BuffList.Any(b => Buffs.Lookup[b].Contains("Chocobo")))
                {
                    Speed = SPEED_CHOCOBO;
                }
                else if(UseDetection && Detected)
                {
                    if(!Globals.Instance.Pyxie.UseBoundDetectedMovement &&
                        PlayerBuffs.BuffList.Any(b => Buffs.Lookup[b].Contains("bind")))
                    {
                        Speed = 0;
                    }
                    else if(Settings.DetectedSpeed != Speed)
                    {
                        if(PlayerBuffs.BuffList.Any(b => Buffs.Lookup[b].Contains("Flee")))
                        {
                            Speed = SPEED_FLEE > Settings.DetectedSpeed ? SPEED_FLEE : Settings.DetectedSpeed;
                        }
                        else
                        {
                            Speed = Settings.DetectedSpeed;
                        }
                    }
                }
                else
                {
                    if(!Globals.Instance.Pyxie.UseBoundMovement && 
                        PlayerBuffs.BuffList.Any(b => Buffs.Lookup[b].Contains("bind")))
                    {
                        Speed = 0;
                    }
                    else
                    {
                        Speed = Settings.Speed;
                    }
                }

                Thread.Sleep(100);
            }

            Speed = SPEED_BASE;
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
        /// Gets or sets whether to use movement speed. Handles thread creation.
        /// </summary>
        public bool UseSpeed
        {
            get
            {
                if (Settings.UseSpeed && SpeedThread == null)
                {
                    SpeedThread = new Thread(new ThreadStart(UpdateSpeed));
                    SpeedThread.IsBackground = true;
                    SpeedThread.Start();
                }
                return Settings.UseSpeed;
            }
            set
            {
                if (value)
                {
                    SpeedThread = new Thread(new ThreadStart(UpdateSpeed));
                    SpeedThread.IsBackground = true;
                    SpeedThread.Start();
                }
                Settings.UseSpeed = value;
            }
        }

        /// <summary>
        /// A thread to update a player's movement.
        /// </summary>
        public Thread SpeedThread { get; set; }

        #endregion
    }
}
