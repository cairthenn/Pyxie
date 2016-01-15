using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using MahApps.Metro;
using System.IO;
using System.Xml.Serialization;
using Pyxie.FFXIStructures;

namespace Pyxie
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public ObservablePlayerCollection<Player> ActiveProcessList;
        private Thread ProcessThread;

        public MainWindow()
        {
            InitializeComponent();

        }


        private void PyxieWindow_Loaded(object sender, RoutedEventArgs e)
        {            
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Pyxie.xml"))
            {
                try
                {
                    using (StreamReader streamReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\Pyxie.xml"))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
                        Globals.Instance.Pyxie = (Configuration)serializer.Deserialize(streamReader);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Pyxie was unable to load your global configuration file:\r\n\r\n" + ex.ToString());
                    return;
                }
            }

            Version Current = typeof(MainWindow).Assembly.GetName().Version;
            
            ActiveProcessList = new ObservablePlayerCollection<Player>();

            ProcessThread = new Thread(() => UpdateProcessSelections(ActiveProcessList));
            ProcessThread.IsBackground = true;
            ProcessThread.Start();

            this.PeopleList.DataContext = ActiveProcessList;
            this.GlobalSettings.DataContext = Globals.Instance.Pyxie;
            this.ExclusionsSettings.DataContext = Globals.Instance.Pyxie;
        }

        private void PyxieWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Globals.Exiting = true;

            try
            {
                using (StreamWriter streamWriter = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\Pyxie.xml"))
                {
                    var serializer = new XmlSerializer(typeof(Configuration));
                    serializer.Serialize(streamWriter, Globals.Instance.Pyxie);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Pyxie was unable to save your global configuration file:\r\n\r\n" + ex.ToString());
            }

            foreach (Player FFXIChar in ActiveProcessList)
            {
                FFXIChar.SaveSettings();
            }
            Thread.Sleep(150);
        }


        #region "Positional Adjustment Buttons"

        private void nwButton_Click(object sender, RoutedEventArgs e)
        {
            ActiveProcessList[this.PeopleList.SelectedIndex].MoveNorthWest();
        }

        private void nButton_Click(object sender, RoutedEventArgs e)
        {
            ActiveProcessList[this.PeopleList.SelectedIndex].MoveNorth();
        }

        private void neButton_Click(object sender, RoutedEventArgs e)
        {
            ActiveProcessList[this.PeopleList.SelectedIndex].MoveNorthEast();
        }

        private void upButton_Click(object sender, RoutedEventArgs e)
        {
            ActiveProcessList[this.PeopleList.SelectedIndex].MoveUp();
        }

        private void wButton_Click(object sender, RoutedEventArgs e)
        {
            ActiveProcessList[this.PeopleList.SelectedIndex].MoveWest();
        }

        private void eButton_Click(object sender, RoutedEventArgs e)
        {
            ActiveProcessList[this.PeopleList.SelectedIndex].MoveEast();
        }

        private void dnButton_Click(object sender, RoutedEventArgs e)
        {
            ActiveProcessList[this.PeopleList.SelectedIndex].MoveDown();
        }

        private void swButton_Click(object sender, RoutedEventArgs e)
        {
            ActiveProcessList[this.PeopleList.SelectedIndex].MoveSouthWest();
        }

        private void sButton_Click(object sender, RoutedEventArgs e)
        {
            ActiveProcessList[this.PeopleList.SelectedIndex].MoveSouth();
        }

        private void seButton_Click(object sender, RoutedEventArgs e)
        {
            ActiveProcessList[this.PeopleList.SelectedIndex].MoveSouthEast();
        }

        #endregion

        private void PeopleList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ActiveProcessList.Count > this.PeopleList.SelectedIndex)
            {
                if (PeopleList.SelectedIndex >= 0)
                {
                    this.playerSettings.DataContext = ActiveProcessList[this.PeopleList.SelectedIndex];
                    this.playerSettings.IsEnabled = true;
                }
                else
                {
                    this.playerSettings.DataContext = new Player();
                    this.playerSettings.IsEnabled = false;
                }
            }
        }

        public void UpdateProcessSelections(ObservablePlayerCollection<Player> ActiveProcessList)
        {



            while (!Globals.Exiting)
            {
                // Get current running processes named pol:

                var CurrentProcessList = Process.GetProcessesByName("pol").ToList();


                if (CurrentProcessList.Count == 0 && ActiveProcessList.Count > 0)
                {
                    //There are no POL Processes we need to handle
                    //Clear them, Players will clean themselves up

                    ActiveProcessList.Clear();
                }
                else if (CurrentProcessList.Count > 0)
                {
                    foreach (Process FFXI in CurrentProcessList)
                    {
                        // The process isn't responding, ignore it
                        if (!FFXI.Responding)
                            continue;

                        if (!ActiveProcessList.Any(P => P.Id == FFXI.Id))
                        {
                            if (FFXI.MainWindowTitle.Length == 0 || FFXI.MainWindowTitle.Contains(' '))
                                continue;

                            // The character hasn't been added to the list yet
                            ActiveProcessList.Add(new Player(FFXI));
                            HandleNewCharacter();
                            continue;
                        }

                    }

                    foreach (Player FFXIChar in ActiveProcessList.ToList())
                    {
                        if (!CurrentProcessList.Any(P => P.MainWindowTitle == FFXIChar.Name))
                        {
                            FFXIChar.SaveSettings();
                            FFXIChar.Destroy();
                            ActiveProcessList.Remove(FFXIChar);
                            
                        }

                    }
                }

                Thread.Sleep(100);
            }
        }

        public void HandleNewCharacter()
        {
            Dispatcher.Invoke(new Action(() =>
                {
                    int SavedIndex = PeopleList.SelectedIndex;

                    // Silly work around: Change the data context to a new player to properly set up threads for a newly added player,
                    // switch to them, and then switch back if a previous player existed.

                    playerSettings.DataContext = null;

                    PeopleList.SelectedIndex = ActiveProcessList.Count - 1;
                    
                    if(ActiveProcessList.Count > 1)
                        PeopleList.SelectedIndex = SavedIndex;
                }));
        }

        #region "Exclusion Helpers"

        private void ExcludedPlayersButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ExcludedPlayersTextbox.Text.Length > 0)
            {
                Globals.Instance.Pyxie.ExcludedPlayers.Add(this.ExcludedPlayersTextbox.Text);
                this.ExcludedPlayersTextbox.Text = "";
            }
        }

        private void ExcludedZonesButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ExcludedZonesTextbox.Text.Length > 0)
            {
                Globals.Instance.Pyxie.ExcludedZones.Add(this.ExcludedZonesTextbox.Text);
                this.ExcludedZonesTextbox.Text = "";
            }
            else
            {
                Globals.Instance.Pyxie.ExcludedZones.Add(Zones.Instance.ZoneMap.FirstOrDefault(key => key.Any(ienum => ienum ==
                    ActiveProcessList[PeopleList.SelectedIndex].Zone)).Key);
            }
        }

        private void IncludedZonesButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.IncludedZonesTextbox.Text.Length > 0)
            {
                Globals.Instance.Pyxie.IncludedZones.Add(this.IncludedZonesTextbox.Text);
                this.IncludedZonesTextbox.Text = "";
            }
            else
            {
                Globals.Instance.Pyxie.IncludedZones.Add(Zones.Instance.ZoneMap.FirstOrDefault(key => key.Any(ienum => ienum ==
                    ActiveProcessList[PeopleList.SelectedIndex].Zone)).Key);
            }
        }

        private void ExcludedPlayers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((uint)this.ExcludedPlayers.SelectedIndex < Globals.Instance.Pyxie.ExcludedPlayers.Count)
                Globals.Instance.Pyxie.ExcludedPlayers.RemoveAt(this.ExcludedPlayers.SelectedIndex);
        }

        private void ExcludedZones_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((uint)this.ExcludedZones.SelectedIndex < Globals.Instance.Pyxie.ExcludedZones.Count)
                Globals.Instance.Pyxie.ExcludedZones.RemoveAt(this.ExcludedZones.SelectedIndex);
        }

        private void IncludedZones_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((uint)this.IncludedZones.SelectedIndex < Globals.Instance.Pyxie.IncludedZones.Count)
                Globals.Instance.Pyxie.IncludedZones.RemoveAt(this.IncludedZones.SelectedIndex);
        }

        private void ExcludedPlayersTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ExcludedPlayersButton_Click(sender, e);
        }

        private void ExcludedZonesTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ExcludedZonesButton_Click(sender, e);
        }

        private void IncludedZonesTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ExcludedZonesButton_Click(sender, e);
        }

        #endregion


        #region "Theme/Accent Handling"

        private void themeRedClick(object sender, RoutedEventArgs e)
        {
            Globals.Instance.Pyxie.Accent = "Red";
            UpdateTheme();
        }

        private void themeGreenClick(object sender, RoutedEventArgs e)
        {
            Globals.Instance.Pyxie.Accent = "Green";
            UpdateTheme();
        }

        private void themeBlueClick(object sender, RoutedEventArgs e)
        {
            Globals.Instance.Pyxie.Accent = "Blue";
            UpdateTheme();
        }

        private void themePurpleClick(object sender, RoutedEventArgs e)
        {
            Globals.Instance.Pyxie.Accent = "Purple";
            UpdateTheme();
        }

        private void themeOrangeClick(object sender, RoutedEventArgs e)
        {
            Globals.Instance.Pyxie.Accent = "Orange";
            UpdateTheme();
        }

        private void themeLimeClick(object sender, RoutedEventArgs e)
        {
            Globals.Instance.Pyxie.Accent = "Lime";
            UpdateTheme();
        }

        private void themeEmeraldClick(object sender, RoutedEventArgs e)
        {
            Globals.Instance.Pyxie.Accent = "Emerald";
            UpdateTheme();
        }

        private void themeTealClick(object sender, RoutedEventArgs e)
        {
            Globals.Instance.Pyxie.Accent = "Teal";
            UpdateTheme();
        }

        private void themeCyanClick(object sender, RoutedEventArgs e)
        {
            Globals.Instance.Pyxie.Accent = "Cyan";
            UpdateTheme();
        }

        private void themeCobaltClick(object sender, RoutedEventArgs e)
        {
            Globals.Instance.Pyxie.Accent = "Cobalt";
            UpdateTheme();
        }

        private void themeIndigoClick(object sender, RoutedEventArgs e)
        {
            Globals.Instance.Pyxie.Accent = "Indigo";
            UpdateTheme();
        }

        private void themeVioletClick(object sender, RoutedEventArgs e)
        {
            Globals.Instance.Pyxie.Accent = "Violet";
            UpdateTheme();
        }

        private void themePinkClick(object sender, RoutedEventArgs e)
        {
            Globals.Instance.Pyxie.Accent = "Pink";
            UpdateTheme();
        }

        private void themeMagentaClick(object sender, RoutedEventArgs e)
        {
            Globals.Instance.Pyxie.Accent = "Magenta";
            UpdateTheme();
        }

        private void themeCrimsonClick(object sender, RoutedEventArgs e)
        {
            Globals.Instance.Pyxie.Accent = "Crimson";
            UpdateTheme();
        }

        private void themeAmberClick(object sender, RoutedEventArgs e)
        {
            Globals.Instance.Pyxie.Accent = "Amber";
            UpdateTheme();
        }

        private void themeYellowClick(object sender, RoutedEventArgs e)
        {
            Globals.Instance.Pyxie.Accent = "Yellow";
            UpdateTheme();
        }

        private void themeBrownClick(object sender, RoutedEventArgs e)
        {
            Globals.Instance.Pyxie.Accent = "Brown";
            UpdateTheme();
        }

        private void themeOliveClick(object sender, RoutedEventArgs e)
        {
            Globals.Instance.Pyxie.Accent = "Olive";
            UpdateTheme();
        }

        private void themeSteelClick(object sender, RoutedEventArgs e)
        {
            Globals.Instance.Pyxie.Accent = "Steel";
            UpdateTheme();
        }

        private void themeMauveClick(object sender, RoutedEventArgs e)
        {
            Globals.Instance.Pyxie.Accent = "Mauve";
            UpdateTheme();
        }

        private void themeSiennaClick(object sender, RoutedEventArgs e)
        {
            Globals.Instance.Pyxie.Accent = "Sienna";
            UpdateTheme();
        }

        public void UpdateTheme()
        {
            if (ThemeManager.Accents.ToLookup(a => a.ToString())[Globals.Instance.Pyxie.Accent] != null
                                                                      && Globals.Instance.Pyxie.Accent != null)
            {
                ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent(Globals.Instance.Pyxie.Accent), ThemeManager.GetAppTheme("BaseDark"));
            }
        }

        #endregion



    }


}
