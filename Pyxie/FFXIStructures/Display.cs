using Pyxie.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pyxie.FFXIStructures
{
    public class Display : FFXIObject<DisplayStruct>
    {
        public Display(MemHandler memHandler, IntPtr baseAddress)
            : base(memHandler, baseAddress)
        {

        }

        public float X 
        {
            get
            {
                return Read<float>("X");
            }
            set
            {
                Modify<float>("X", value);
            }
        }
        public float Z
        {
            get
            {
                return Read<float>("Z");
            }
            set
            {
                Modify<float>("Z", value);
            }
        }
        public float Y
        {
            get
            {
                return Read<float>("Y");
            }
            set
            {
                Modify<float>("Y", value);
            }
        }
        public float X2
        {
            get
            {
                return Read<float>("X2");
            }
            set
            {
                Modify<float>("X2", value);
            }
        }
        public float Z2
        {
            get
            {
                return Read<float>("Z2");
            }
            set
            {
                Modify<float>("Z2", value);
            }
        }
        public float Y2
        {
            get
            {
                return Read<float>("Y2");
            }
            set
            {
                Modify<float>("Y2", value);
            }
        }
        public float X3
        {
            get
            {
                return Read<float>("X3");
            }
            set
            {
                Modify<float>("X3", value);
            }
        }
        public float Z3
        {
            get
            {
                return Read<float>("Z3");
            }
            set
            {
                Modify<float>("Z3", value);
            }
        }
        public float Y3
        {
            get
            {
                return Read<float>("Y3");
            }
            set
            {
                Modify<float>("Y3", value);
            }
        }
        public bool Freeze
        {
            get
            {
                return Read<bool>("Freeze");
            }
            set
            {
                Modify<bool>("Freeze", value);
            }
        }
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct DisplayStruct
    {
      [MarshalAs(UnmanagedType.R4)] [FieldOffset(0x34)] float X;
      [MarshalAs(UnmanagedType.R4)] [FieldOffset(0x38)] float Z;
      [MarshalAs(UnmanagedType.R4)] [FieldOffset(0x3C)] float Y;
      [MarshalAs(UnmanagedType.R4)] [FieldOffset(0xC4)] float X2;
      [MarshalAs(UnmanagedType.R4)] [FieldOffset(0xC8)] float Z2;
      [MarshalAs(UnmanagedType.R4)] [FieldOffset(0xCC)] float Y2;
      [MarshalAs(UnmanagedType.R4)] [FieldOffset(0xD4)] float X3;
      [MarshalAs(UnmanagedType.R4)] [FieldOffset(0xD8)] float Z3;
      [MarshalAs(UnmanagedType.R4)] [FieldOffset(0xDC)] float Y3;
      [MarshalAs(UnmanagedType.I1)] [FieldOffset(0xFC)] public bool Freeze;


    }


}
