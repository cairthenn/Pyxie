using Pyxie.FFXIStructures;
using Pyxie.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Xml.Serialization;

namespace Pyxie
{
    public class Buffs : FFXIObject<BuffStruct>
    {
        public Buffs(MemHandler memHandler, IntPtr baseAddress)
            : base(memHandler, baseAddress)
        {
            LoadBuffs();
        }

        public void LoadBuffs()
        {
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\status.xml"))
            {
                MessageBox.Show("Pyxie was unable to load your status resources file. To use status detection,\r\nadd status.xml from your resources folder or obtain the latest download.");
                Lookup = Enumerable.Empty<String>().ToLookup(x => default(Int16));
                return;
            }

            try
            {
                using (StreamReader streamReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\status.xml"))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(b[]), new XmlRootAttribute() { ElementName = "status" });
                    Lookup = ((b[])serializer.Deserialize(streamReader)).ToLookup(buff => buff.id, buff => buff.Name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Pyxie was unable to load your status resources file:\r\n\r\n" + ex.ToString());
                return;
            }
        }

        public class b
        {
            [XmlAttribute]
            public Int16 id;
            [XmlText]
            public String Name;
        }

        public IEnumerable<Int16> BuffList
        {
            get
            {
                var temp = Read<BuffStruct>("BuffList");
                return temp.BuffList.AsEnumerable();
            }
        }
        

        public static ILookup<Int16, String> Lookup;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct BuffStruct
    {
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)] [FieldOffset(0)] public Int16[] BuffList;
    }
}
