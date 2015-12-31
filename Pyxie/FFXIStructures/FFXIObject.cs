using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.InteropServices;
using Pyxie.Memory;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

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
        }


        /// <summary>
        /// Reads a value of specified field from memory. Does NOT support type TX with unspecified size
        /// </summary>
        /// <typeparam name="TX"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        internal virtual TX Read<TX>(string field)
        {
            IntPtr target = IntPtr.Add(BaseAddress, (int)Marshal.OffsetOf(typeof(T), field));

            int bytesRead;
            byte[] _c = MemoryHandler.ReadAdress(target, (uint)Marshal.SizeOf(typeof(TX)), out bytesRead);


            GCHandle handle = GCHandle.Alloc(_c, GCHandleType.Pinned);
            TX structure = (TX)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(TX));
            handle.Free();

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
                //Single byte

                var byteArray = new byte[1];
                byteArray[0] = Convert.ToByte(val);
                MemoryHandler.WriteAddress(target, byteArray);
            }
        }

        public IntPtr BaseAddress { get; set; }

        public T Struct { get; set; }

        public MemHandler MemoryHandler { get; set; }
    }
}
