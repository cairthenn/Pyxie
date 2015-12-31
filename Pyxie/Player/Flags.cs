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
        public void UpdateFlags()
        {
            while((UseMaintenance || UseGM) && Active && Settings.DangerMode && !Globals.Exiting)
            {
                this.Update();

                int ResultFlag = (int)PlayerEntity.Flags3;

                if ((PlayerEntity.Flags3 & EntityEnum.Flags3.Maintenance) == 0 && UseMaintenance)
                    ResultFlag |= (int)EntityEnum.Flags3.Maintenance;
                else if ((PlayerEntity.Flags3 & EntityEnum.Flags3.Maintenance) != 0 && !UseMaintenance)
                    ResultFlag -= (int)EntityEnum.Flags3.Maintenance;

                if ((PlayerEntity.Flags3 & EntityEnum.Flags3.GM) == 0 && UseGM)
                    ResultFlag |= (int)EntityEnum.Flags3.GM;
                else if ((PlayerEntity.Flags3 & EntityEnum.Flags3.GM) != 0 && !UseGM)
                    ResultFlag -= (int)EntityEnum.Flags3.GM;

                PlayerEntity.Flags3 = (EntityEnum.Flags3) ResultFlag;
            }

            int FlagReset = (int)PlayerEntity.Flags3;

            if((PlayerEntity.Flags3 & EntityEnum.Flags3.Maintenance) != 0)
                FlagReset -= (int)EntityEnum.Flags3.Maintenance;

            if ((PlayerEntity.Flags3 & EntityEnum.Flags3.GM) != 0)
                FlagReset -= (int)EntityEnum.Flags3.GM;

            PlayerEntity.Flags3 = (EntityEnum.Flags3)FlagReset;
        }

        #region =="Properties"

        /// <summary>
        /// Gets or sets whether to use GM Flag. Handles thread creation.
        /// </summary>
        public bool UseGM
        {
            get
            {
                if (Settings.UseGM && FlagThread == null)
                {
                    FlagThread = new Thread(new ThreadStart(UpdateFlags));
                    FlagThread.IsBackground = true;
                    FlagThread.Start();
                }
                return Settings.UseGM;
            }
            set
            {
                if (value && (FlagThread == null || !FlagThread.IsAlive))
                {
                    FlagThread = new Thread(new ThreadStart(UpdateFlags));
                    FlagThread.IsBackground = true;
                    FlagThread.Start();
                }
                Settings.UseGM = value;
            }
        }

        /// <summary>
        /// Gets or sets whether to use Maintenance Flag. Handles thread creation.
        /// </summary>
        public bool UseMaintenance
        {
            get
            {
                if (Settings.UseMaintenance && FlagThread == null)
                {
                    FlagThread = new Thread(new ThreadStart(UpdateFlags));
                    FlagThread.IsBackground = true;
                    FlagThread.Start();
                }
                return Settings.UseMaintenance;
            }
            set
            {
                if (value && (FlagThread == null || !FlagThread.IsAlive))
                {
                    FlagThread = new Thread(new ThreadStart(UpdateFlags));
                    FlagThread.IsBackground = true;
                    FlagThread.Start();
                }
                Settings.UseMaintenance = value;
            }
        }

        public Thread FlagThread { get; set; }

        #endregion
    }
}
