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
        public void UpdateStatus()
        {
            EntityEnum.Status SavedStatus = EntityEnum.Status.Idle;

            while (Active && !Globals.Exiting)
            {


                if (Settings.UseClientBlock)
                {
                    Update();

                    if ((this.PlayerEntity.Status & EntityEnum.Status.Debug) == 0)
                    {
                        SavedStatus = PlayerEntity.Status;
                        PlayerEntity.Status = EntityEnum.Status.Debug;
                    }
                }
                else if(Settings.UseEngagedMode)
                {
                    Update();

                    if ((this.PlayerEntity.Status & EntityEnum.Status.Engaged) == 0)
                    {
                        PlayerEntity.Status = EntityEnum.Status.Engaged;
                    }
                }
                else
                {
                    Update();
                    PlayerEntity.Status = SavedStatus;
                }

                Thread.Sleep(100);

            }

        }



        public Thread StatusThread { get; set; }
    }
}
