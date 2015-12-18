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

        public float X { get; set; }
        public float Z { get; set; }
        public float Y { get; set; }
        public float X2 { get; set; }
        public float Z2 { get; set; }
        public float Y2 { get; set; }
        public float X3 { get; set; }
        public float Z3 { get; set; }
        public float Y3 { get; set; }
        public bool Freeze { get; set; }
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
