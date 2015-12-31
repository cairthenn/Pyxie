using Pyxie.FFXIStructures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyxie
{
    public partial class Player
    {
        public void FindSignatures()
        {

            Debug.Assert(MemoryHandler != null, "No memory handler was specified. Ensure MemoryHandler is not null before calling FindSignatures().");

            // MobArray is the address that holds a pointer to the mob array. MobArrayPosition is the address that holds the position.

            this.MobArray = MemoryHandler.ResolvePointer(SignatureScanner.FindPattern(new byte[] { 0x8B, 0x56, 0x0C, 0x8B, 0x04, 0x2A, 0x8B, 0x04, 0x85 }, "xxxxxxxxx", 9));
            this.MobArrayPosition = SignatureScanner.FindPattern(new byte[] { 0x66, 0xC7, 0x44, 0x24, 0x10, 0x79, 0x00, 0x50 }, "xxxxxxxx", 37);

            this.BuffPtr = MemoryHandler.ResolvePointer(SignatureScanner.FindPattern(new byte[] { 0xEB, 0x0E, 0x33, 0xDB, 0x8A, 0xF8, 0x8A, 0xD9, 0x66, 0x89, 0x1C, 0x55 }, "xxxxxxxxxxxx", 12));
            this.ZonePtr = SignatureScanner.FindPattern(new byte[] { 0x7C, 0xE1, 0x8B, 0x4E, 0x08, 0x8B, 0x15 }, "xxxxxxx", 7);

            var index = (short)MemoryHandler.ResolvePointer(MemoryHandler.ResolvePointer(this.MobArrayPosition) + 4);
            this.PlayerEntity = new Entity(MemoryHandler, MemoryHandler.ResolvePointer(MobArray + 4 * index));
            this.PlayerDisplay = new Display(MemoryHandler, PlayerEntity.Display);
            this.PlayerBuffs = new Buffs(MemoryHandler, this.BuffPtr);

        }

        public IntPtr MobArrayPosition { get; set; }

        public IntPtr MobArray { get; set; }

        public IntPtr BuffPtr { get; set; }

        public IntPtr ZonePtr { get; set; }
    }
}
