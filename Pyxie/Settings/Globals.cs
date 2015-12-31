using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Pyxie
{
    public class Globals
    {
        private Globals()
        {
            Pyxie = new Configuration();
        }

        public static Boolean Exiting { get; set; }

        /// <summary>
        /// Global configuration settings, serialiazable to XML.
        /// </summary>
        public Configuration Pyxie { get; set; }

        /// <summary>
        /// Internal singleton instance of this class.
        /// </summary>
        private static Globals mInstance;

        /// <summary>
        /// Gets the instance of this class.
        /// </summary>
        public static Globals Instance
        {
            get { return mInstance ?? (mInstance = new Globals()); }
        }
    }

    public class Configuration
    {
        public Configuration()
        {
          ExcludedPlayers = new ObservableCollection<string>();
          ExcludedZones = new ObservableCollection<string>();
          IncludedZones = new ObservableCollection<string>();
          Transparency = 100.0f;
        }
    
        /// <summary>
        /// Setting that determines movement capability while bound.
        /// </summary>
        [XmlElement]
        public bool UseBoundMovement { get; set; }
        
        /// <summary>
        /// Setting that determines movement capability while bound and detected.
        /// </summary>
        [XmlElement]
        public bool UseBoundDetectedMovement { get; set; }
        
        /// <summary>
        /// Setting that adds an observable flag to the invisible ones.
        /// </summary>
        [XmlElement]
        public bool UseVisibleFlags { get; set; }
        
        /// <summary>
        /// Setting to use the default speed whilst on a chocobo.
        /// </summary>
        [XmlElement]
        public bool UseChocoboSpeed { get; set; }
        
        /// <summary>
        /// Setting to be automatically detected without sneak on.
        /// </summary>
        [XmlElement]
        public bool DetectionWithoutSneak { get; set; }
        
        /// <summary>
        /// Setting to be automatically detected without invisible on.
        /// </summary>
        [XmlElement]
        public bool DetectionWithoutInvisible { get; set; }

        /// <summary>
        /// Setting to use detection for JA Zero Wait
        /// </summary>
        [XmlElement]
        public bool UseJaZeroDetection { get; set; }

        /// <summary>
        /// Setting to use detection for positioning
        /// </summary>
        [XmlElement]
        public bool UsePositioningDetection { get; set; }

        /// <summary>
        /// Setting to use zone delay.
        /// </summary>
        [XmlElement]
        public bool UseZoneDelay { get; set; }

        /// <summary>
        /// Custom positioning distance
        /// </summary>
        [XmlElement]
        public float CustomDistance { get; set; }
        
        /// <summary>
        /// Setting to use zone delay.
        /// </summary>
        [XmlElement]
        public bool UseCustomDistance { get; set; }
         
        /// <summary>
        /// Always on top setting.
        /// </summary>
        [XmlElement]
        public bool AlwaysOnTop { get; set; }

        [XmlElement]
        public string Accent { get; set; }

        private float transparancy;

        /// <summary>
        /// Window transparency setting.
        /// </summary>
        [XmlElement]
        public float Transparency { 
          get
          {
            return transparancy;
          }
          set
          {
            transparancy = value < 20.0f ? 20.0f : value;
          }
        }

        /// <summary>
        /// Minimize to tray setting.
        /// </summary>
        [XmlElement]
        public bool MinimizeToTray { get; set; }

        /// <summary>
        /// Gets or sets the delay after zoning before reactivating settings.
        /// </summary>
        [XmlElement]
        public int ZoneDelay { get; set; }

        /// <summary>
        /// List of excluded zones.
        /// </summary>
        [XmlArray]
        [XmlArrayItem(ElementName = "Include")]
        public ObservableCollection<String> IncludedZones { get; set; }

        /// <summary>
        /// List of excluded zones.
        /// </summary>
        [XmlArray]
        [XmlArrayItem(ElementName = "Exclude")]
        public ObservableCollection<String> ExcludedZones { get; set; }
        
        /// <summary>
        /// List of excluded players when checking for detected players.
        /// </summary>
        [XmlArray]
        [XmlArrayItem(ElementName = "Exclude")]
        public ObservableCollection<String> ExcludedPlayers { get; set; }

    }
}
