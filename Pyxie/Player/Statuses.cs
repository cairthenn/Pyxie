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
            while ((UseEngagedMode || UseClientBlock) && Active && !Globals.Exiting)
            {
                this.Update();

                if (UseClientBlock)
                {
                    if ((this.PlayerEntity.Status & EntityEnum.Status.Debug) == 0)
                        this.PlayerEntity.Modify<byte>("Status", (byte)EntityEnum.Status.Debug);
                }
                else if(UseEngagedMode)
                {
                    if ((this.PlayerEntity.Status & EntityEnum.Status.Engaged) == 0)
                        this.PlayerEntity.Modify<byte>("Status", (byte)EntityEnum.Status.Engaged);
                }

                Thread.Sleep(100);

            }


            //Returns you to dead status if you're dead, otherwise idle.
            if ((this.PlayerEntity.Flags1 & EntityEnum.Flags1.Dead) != 0)
                this.PlayerEntity.Modify<byte>("Status", (byte)EntityEnum.Status.Dead);
            else
                this.PlayerEntity.Modify<byte>("Status", (byte)EntityEnum.Status.Idle);
        }



        public bool UseEngagedMode {
            get
            {
                if (Settings.UseEngagedMode && StatusThread == null)
                {
                    StatusThread = new Thread(new ThreadStart(UpdateStatus));
                    StatusThread.IsBackground = true;
                    StatusThread.Start();
                }
                return Settings.UseEngagedMode;
            }
            set
            {
                if (value && (StatusThread == null || !StatusThread.IsAlive))
                {
                    StatusThread = new Thread(new ThreadStart(UpdateStatus));
                    StatusThread.IsBackground = true;
                    StatusThread.Start();
                }
                Settings.UseEngagedMode = value;
            }
        }

        public bool UseClientBlock {
            get
            {
                if (Settings.UseClientBlock && StatusThread == null)
                {
                    StatusThread = new Thread(new ThreadStart(UpdateStatus));
                    StatusThread.IsBackground = true;
                    StatusThread.Start();
                }
                return Settings.UseClientBlock;
            }
            set
            {
                if (value && (StatusThread == null || !StatusThread.IsAlive))
                {
                    StatusThread = new Thread(new ThreadStart(UpdateStatus));
                    StatusThread.IsBackground = true;
                    StatusThread.Start();
                }
                Settings.UseClientBlock = value;
            }
        }

        public Thread StatusThread { get; set; }
    }
}
