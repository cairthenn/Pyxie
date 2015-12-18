using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Pyxie.Memory
{
    public class MemHandler
    {
        #region "Process Access Flags"

        private enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VMOperation = 0x00000008,
            VMRead = 0x00000010,
            VMWrite = 0x00000020,
            DupHandle = 0x00000040,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            Synchronize = 0x00100000
        }

        #endregion

        
        #region "Constuctor & Destructor"

        public MemHandler(Process ffxi)
        {
            ffxi_ = ffxi;
            OpenProcess((uint) ffxi.Id);
        }

        ~MemHandler()
        {
            CloseHandle(handle_);
        }

        #endregion

        #region "DllImports"

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess,
                                                 [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, UInt32 dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern Int32 CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll")]
        private static extern Int32 ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer,
                                                      UInt32 size, out IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize,
                                                      out int lpNumberOfBytesWritten);

        #endregion


        private void OpenProcess(uint pID)
        {
            try
            {
                handle_ = OpenProcess(ProcessAccessFlags.All, false, pID);
            }
            catch
            {
 
            }
        }

        public IntPtr ResolvePointer(IntPtr pointer)
        {
            int outres;
            byte[] structure = ReadAdress(pointer, 4, out outres);
            var target = (IntPtr)BitConverter.ToInt32(structure, 0);
            return target;
        }

        public byte[] ReadAdress(IntPtr memoryAddress, uint bytesToRead, out int bytesRead)
        {
            try
            {
                if (bytesToRead > 0)
                {
                    var buffer = new byte[bytesToRead];
                    IntPtr ptrBytesRead;
                    ReadProcessMemory(handle_, memoryAddress, buffer, bytesToRead, out ptrBytesRead);
                    bytesRead = ptrBytesRead.ToInt32();
                    return buffer;
                }

                bytesRead = 0;
                return new byte[] { 0, 0, 0, 0 };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to read process memory: " + ex.Message);
                bytesRead = 0;
                return new byte[] { 0, 0, 0, 0 };
            }
        }

        public int WriteAddress(IntPtr memoryAddress, byte[] value)
        {
            int bytesWritten;

            try
            {
                WriteProcessMemory(handle_, memoryAddress, value, (UInt32)value.Length, out bytesWritten);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to write process memory: " + ex.Message);
                return 0;
            }

            return bytesWritten;
        }


        #region "Properties"

        private readonly Process ffxi_;
        private IntPtr handle_;

        #endregion

    }

    
}
