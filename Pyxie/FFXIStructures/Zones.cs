using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace Pyxie.FFXIStructures
{
    public class Zones
    {

        private Zones()
        {
            LoadZones();
        }

        public void LoadZones()
        {
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\areas.xml"))
            {
                MessageBox.Show("Pyxie was unable to load your zone resources file. To use area detection,\r\nadd areas.xml from your resources folder or obtain the latest download.");
                ZoneMap = Enumerable.Empty<Int32>().ToLookup(x => default(String));
                return;
            }

            try
            {
                using (StreamReader streamReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\areas.xml"))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(a[]), new XmlRootAttribute() { ElementName = "areas" });
                    ZoneMap = ((a[])serializer.Deserialize(streamReader)).ToLookup(zone => zone.Name, zone => zone.id);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Pyxie was unable to load your zone resources file:\r\n\r\n" + ex.ToString());
                return;
            }
        }

        public ILookup<String, Int32> ZoneMap { get; set; }

        public class a
        {
            [XmlAttribute]
            public Int32 id;
            [XmlText]
            public String Name;
        }

        private static Zones mInstance;

        /// <summary>
        /// Gets the instance of this class.
        /// </summary>
        public static Zones Instance
        {
            get { return mInstance ?? (mInstance = new Zones()); }
        }
    }
}
