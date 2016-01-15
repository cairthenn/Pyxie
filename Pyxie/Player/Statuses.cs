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
            while (Active && !Globals.Exiting)
            {

                if (Settings.UseClientBlock)
                {
                    Update();

                    if ((this.PlayerEntity.Status & EntityEnum.Status.Debug) == 0)
                        PlayerEntity.Status = EntityEnum.Status.Debug;
                }
                else if(Settings.UseEngagedMode)
                {
                    Update();

                    if ((this.PlayerEntity.Status & EntityEnum.Status.Engaged) == 0)
                        PlayerEntity.Status = EntityEnum.Status.Engaged;
                }

                Thread.Sleep(100);

            }


            //Returns you to dead status if you're dead, otherwise idle.
            if ((this.PlayerEntity.Flags1 & EntityEnum.Flags1.Dead) != 0)
                PlayerEntity.Status = EntityEnum.Status.Dead;
            else
                PlayerEntity.Status = EntityEnum.Status.Idle;
        }



        public Thread StatusThread { get; set; }
    }
}
