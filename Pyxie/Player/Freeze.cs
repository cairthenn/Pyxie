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
            while (Active && !Globals.Exiting)
            {
                if (Settings.UseJaZero)
                {
                    Update();

                    if (!(Globals.Instance.Pyxie.UseJaZeroDetection && Detected))
                    {

                            PlayerDisplay.Freeze = false;
                            PlayerEntity.Freeze = false;
                     }
                }

                Thread.Sleep(100);
            }

        }

        /// <summary>
        /// Thread for handling JAZero
        /// </summary>
        public Thread JaZeroThread { get; set; }

    }
}
