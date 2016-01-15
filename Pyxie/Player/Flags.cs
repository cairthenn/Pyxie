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
            while(Active && Settings.DangerMode && !Globals.Exiting)
            {
                if (Settings.UseGM || Settings.UseMaintenance)
                {
                    this.Update();

                    int ResultFlag = (int)PlayerEntity.Flags3;

                    if ((PlayerEntity.Flags3 & EntityEnum.Flags3.Maintenance) == 0 && Settings.UseMaintenance)
                        ResultFlag |= (int)EntityEnum.Flags3.Maintenance;
                    else if ((PlayerEntity.Flags3 & EntityEnum.Flags3.Maintenance) != 0 && !Settings.UseMaintenance)
                        ResultFlag -= (int)EntityEnum.Flags3.Maintenance;

                    if ((PlayerEntity.Flags3 & EntityEnum.Flags3.GM) == 0 && Settings.UseGM)
                        ResultFlag |= (int)EntityEnum.Flags3.GM;
                    else if ((PlayerEntity.Flags3 & EntityEnum.Flags3.GM) != 0 && !Settings.UseGM)
                        ResultFlag -= (int)EntityEnum.Flags3.GM;

                    PlayerEntity.Flags3 = (EntityEnum.Flags3)ResultFlag;
                }

                Thread.Sleep(100);
            }

            int FlagReset = (int)PlayerEntity.Flags3;

            if((PlayerEntity.Flags3 & EntityEnum.Flags3.Maintenance) != 0)
                FlagReset -= (int)EntityEnum.Flags3.Maintenance;

            if ((PlayerEntity.Flags3 & EntityEnum.Flags3.GM) != 0)
                FlagReset -= (int)EntityEnum.Flags3.GM;

            PlayerEntity.Flags3 = (EntityEnum.Flags3)FlagReset;
        }

        #region =="Properties"

        public Thread FlagThread { get; set; }

        #endregion
    }
}
