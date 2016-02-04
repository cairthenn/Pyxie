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
            bool ActiveFlags = false;

            while (Active && !Globals.Exiting)
            {

                if (((Settings.UseMaintenance && Settings.DangerMode) || Settings.UseGM))
                {
                    ActiveFlags = true;
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
                else if(ActiveFlags)
                {
                    ActiveFlags = false;
                    int FlagReset = (int)PlayerEntity.Flags3;

                    if ((PlayerEntity.Flags3 & EntityEnum.Flags3.Maintenance) != 0)
                        FlagReset -= (int)EntityEnum.Flags3.Maintenance;

                    if ((PlayerEntity.Flags3 & EntityEnum.Flags3.GM) != 0)
                        FlagReset -= (int)EntityEnum.Flags3.GM;

                    PlayerEntity.Flags3 = (EntityEnum.Flags3)FlagReset;
                }

                Thread.Sleep(100);
            }
        }

        #region =="Properties"

        public Thread FlagThread { get; set; }

        #endregion
    }
}
