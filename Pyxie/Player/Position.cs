using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyxie
{
    public partial class Player
    {
        public void MoveNorth()
        {
            if (Globals.Instance.Pyxie.UsePositioningDetection && Detected)
                return;

            this.Update();

            float delta;

            if (Globals.Instance.Pyxie.UseCustomDistance)
                delta = Globals.Instance.Pyxie.CustomDistance;
            else
                delta = 3.0f;

            this.PlayerDisplay.Modify<float>("Y", PlayerDisplay.Y + delta);
            this.PlayerDisplay.Modify<float>("Y2", PlayerDisplay.Y2 + delta);
            this.PlayerDisplay.Modify<float>("Y3", PlayerDisplay.Y3 + delta);
        }

        public void MoveSouth()
        {
            if (Globals.Instance.Pyxie.UsePositioningDetection && Detected)
                return;

            this.Update();

            float delta;

            if (Globals.Instance.Pyxie.UseCustomDistance)
                delta = Globals.Instance.Pyxie.CustomDistance;
            else
                delta = 3.0f;

            this.PlayerDisplay.Modify<float>("Y", PlayerDisplay.Y - delta);
            this.PlayerDisplay.Modify<float>("Y2", PlayerDisplay.Y2 - delta);
            this.PlayerDisplay.Modify<float>("Y3", PlayerDisplay.Y3 - delta);
        }

        public void MoveEast()
        {
            if (Globals.Instance.Pyxie.UsePositioningDetection && Detected)
                return;

            this.Update();

            float delta;

            if (Globals.Instance.Pyxie.UseCustomDistance)
                delta = Globals.Instance.Pyxie.CustomDistance;
            else
                delta = 3.0f;

            this.PlayerDisplay.Modify<float>("X", PlayerDisplay.X + delta);
            this.PlayerDisplay.Modify<float>("X2", PlayerDisplay.X2 + delta);
            this.PlayerDisplay.Modify<float>("X3", PlayerDisplay.X3 + delta);
        }

        public void MoveWest()
        {
            if (Globals.Instance.Pyxie.UsePositioningDetection && Detected)
                return;

            this.Update();

            float delta;

            if (Globals.Instance.Pyxie.UseCustomDistance)
                delta = Globals.Instance.Pyxie.CustomDistance;
            else
                delta = 3.0f;

            this.PlayerDisplay.Modify<float>("X", PlayerDisplay.X - delta);
            this.PlayerDisplay.Modify<float>("X2", PlayerDisplay.X2 - delta);
            this.PlayerDisplay.Modify<float>("X3", PlayerDisplay.X3 - delta);
        }

        public void MoveDown()
        {
            if (Globals.Instance.Pyxie.UsePositioningDetection && Detected)
                return;

            this.Update();

            float delta;

            if (Globals.Instance.Pyxie.UseCustomDistance)
                delta = Globals.Instance.Pyxie.CustomDistance;
            else
                delta = 3.0f;

            this.PlayerDisplay.Modify<float>("Z", PlayerDisplay.Z + delta);
            this.PlayerDisplay.Modify<float>("Z2", PlayerDisplay.Z2 + delta);
            this.PlayerDisplay.Modify<float>("Z3", PlayerDisplay.Z3 + delta);
        }

        public void MoveUp()
        {
            if (Globals.Instance.Pyxie.UsePositioningDetection && Detected)
                return;

            this.Update();

            float delta;

            if (Globals.Instance.Pyxie.UseCustomDistance)
                delta = Globals.Instance.Pyxie.CustomDistance;
            else
                delta = 3.0f;

            this.PlayerDisplay.Modify<float>("Z", PlayerDisplay.Z - delta);
            this.PlayerDisplay.Modify<float>("Z2", PlayerDisplay.Z2 - delta);
            this.PlayerDisplay.Modify<float>("Z3", PlayerDisplay.Z3 - delta);
        }

        public void MoveNorthEast()
        {
            if (Globals.Instance.Pyxie.UsePositioningDetection && Detected)
                return;

            this.Update();

            float delta;

            if (Globals.Instance.Pyxie.UseCustomDistance)
                delta = Globals.Instance.Pyxie.CustomDistance;
            else
                delta = 3.0f;

            this.PlayerDisplay.Modify<float>("Y", PlayerDisplay.Y + (delta / 2));
            this.PlayerDisplay.Modify<float>("Y2", PlayerDisplay.Y2 + (delta / 2));
            this.PlayerDisplay.Modify<float>("Y3", PlayerDisplay.Y3 + (delta / 2));
            this.PlayerDisplay.Modify<float>("X", PlayerDisplay.X + (delta / 2));
            this.PlayerDisplay.Modify<float>("X2", PlayerDisplay.X2 + (delta / 2));
            this.PlayerDisplay.Modify<float>("X3", PlayerDisplay.X3 + (delta / 2));
        }

        public void MoveNorthWest()
        {
            if (Globals.Instance.Pyxie.UsePositioningDetection && Detected)
                return;

            this.Update();

            float delta;

            if (Globals.Instance.Pyxie.UseCustomDistance)
                delta = Globals.Instance.Pyxie.CustomDistance;
            else
                delta = 3.0f;

            this.PlayerDisplay.Modify<float>("Y", PlayerDisplay.Y + (delta / 2));
            this.PlayerDisplay.Modify<float>("Y2", PlayerDisplay.Y2 + (delta / 2));
            this.PlayerDisplay.Modify<float>("Y3", PlayerDisplay.Y3 + (delta / 2));
            this.PlayerDisplay.Modify<float>("X", PlayerDisplay.X - (delta / 2));
            this.PlayerDisplay.Modify<float>("X2", PlayerDisplay.X2 - (delta / 2));
            this.PlayerDisplay.Modify<float>("X3", PlayerDisplay.X3 - (delta / 2));
        }

        public void MoveSouthEast()
        {
            if (Globals.Instance.Pyxie.UsePositioningDetection && Detected)
                return;

            this.Update();

            float delta;

            if (Globals.Instance.Pyxie.UseCustomDistance)
                delta = Globals.Instance.Pyxie.CustomDistance;
            else
                delta = 3.0f;

            this.PlayerDisplay.Modify<float>("Y", PlayerDisplay.Y - (delta / 2));
            this.PlayerDisplay.Modify<float>("Y2", PlayerDisplay.Y2 - (delta / 2));
            this.PlayerDisplay.Modify<float>("Y3", PlayerDisplay.Y3 - (delta / 2));
            this.PlayerDisplay.Modify<float>("X", PlayerDisplay.X + (delta / 2));
            this.PlayerDisplay.Modify<float>("X2", PlayerDisplay.X + (delta / 2));
            this.PlayerDisplay.Modify<float>("X3", PlayerDisplay.X3 + (delta / 2));
        }

        public void MoveSouthWest()
        {
            if (Globals.Instance.Pyxie.UsePositioningDetection && Detected)
                return;

            this.Update();

            float delta;

            if (Globals.Instance.Pyxie.UseCustomDistance)
                delta = Globals.Instance.Pyxie.CustomDistance;
            else
                delta = 3.0f;

            this.PlayerDisplay.Modify<float>("Y", PlayerDisplay.Y - (delta / 2));
            this.PlayerDisplay.Modify<float>("Y2", PlayerDisplay.Y2 + (delta / 2));
            this.PlayerDisplay.Modify<float>("Y3", PlayerDisplay.Y3 - (delta / 2));
            this.PlayerDisplay.Modify<float>("X", PlayerDisplay.X - (delta / 2));
            this.PlayerDisplay.Modify<float>("X2", PlayerDisplay.X2 + (delta / 2));
            this.PlayerDisplay.Modify<float>("X3", PlayerDisplay.X3 - (delta / 2));
        }
    }
}
