using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Pyxie
{
    public partial class Player
    {
        public void UpdateJaZero()
        {
            while (UseJaZero && Active && !Globals.Exiting)
            {
                
                Update();

                if (!(Globals.Instance.Pyxie.UseJaZeroDetection && Detected))
                {
                    if (UseJaZero)
                    {
                        this.PlayerDisplay.Modify<bool>("Freeze", false);
                        this.PlayerEntity.Modify<bool>("Freeze", false);
                    }


                }

                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// Gets or sets whether to use JA Zero. Handles thread creation.
        /// </summary>
        public bool UseJaZero
        {
            get
            {
                if (Settings.UseJaZero && JaZeroThread == null)
                {
                    JaZeroThread = new Thread(new ThreadStart(UpdateJaZero));
                    JaZeroThread.IsBackground = true;
                    JaZeroThread.Start();
                }
                return Settings.UseJaZero;
            }
            set
            {
                if (value && (JaZeroThread == null || !JaZeroThread.IsAlive))
                {
                    JaZeroThread = new Thread(new ThreadStart(UpdateJaZero));
                    JaZeroThread.IsBackground = true;
                    JaZeroThread.Start();
                }

                Settings.UseJaZero = value;
            }
        }

        /// <summary>
        /// Thread for handling JAZero
        /// </summary>
        public Thread JaZeroThread { get; set; }

    }
}
