using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.InteropServices;
using Pyxie.Memory;

namespace Pyxie.FFXIStructures
{
    public abstract class FFXIObject<T>
    {
        internal FFXIObject(MemHandler memHandler, IntPtr baseAddress)
        {
            MemoryHandler = memHandler;
            UpdateWithAddress(baseAddress);
        }

        internal virtual void UpdateWithAddress(IntPtr baseAddress)
        {
            BaseAddress = baseAddress;

            T objStruct = StructFromPtr<T>(baseAddress);

            foreach (var field in typeof(T).GetFields(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                PropertyInfo prop = GetType().GetProperty(field.Name);

                if (prop != null)
                {
                    Object val = field.GetValue(objStruct);
                    prop.SetValue(this, val);
                }

            }
        }

        private TU StructFromPtr<TU>(IntPtr address)
        {
            TU structure = default(TU);

            if(address != IntPtr.Zero)
            {
                int bytesRead;
                byte[] _c = MemoryHandler.ReadAdress(address, (uint)Marshal.SizeOf(typeof(T)), out bytesRead);
                GCHandle _h = GCHandle.Alloc(_c, GCHandleType.Pinned);
                structure = (TU)Marshal.PtrToStructure(_h.AddrOfPinnedObject(), typeof(T));
                _h.Free();
            }

            return structure;
        }

        internal virtual void Modify<TX>(string field, TX val)
        {
            IntPtr target = IntPtr.Add(BaseAddress, (int) Marshal.OffsetOf(typeof(T), field));

            try
            {
                object byteVal = typeof(BitConverter).GetMethod("GetBytes", new[] { val.GetType() }).
                    Invoke(null, new object[] { val });

                MemoryHandler.WriteAddress(target, byteVal as byte[]);
            }
            catch(AmbiguousMatchException)
            {

            }
        }

        public IntPtr BaseAddress { get; set; }

        public T Struct { get; set; }

        public MemHandler MemoryHandler { get; set; }
    }
}
