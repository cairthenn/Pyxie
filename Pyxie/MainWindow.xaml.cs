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
            
            ActiveProcessList = new ObservablePlayerCollection<Player>();

            ProcessThread = new Thread(() => UpdateProcessSelections(ActiveProcessList));
            ProcessThread.IsBackground = true;
            ProcessThread.Start();

            this.PeopleList.DataContext = ActiveProcessList;
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
                            ActiveProcessList.Remove(FFXIChar);
                        }

                    }
                }

            }
        }

        public void HandleNewCharacter()
        {
            Dispatcher.Invoke(new Action(() =>
                {
                    int SavedIndex = PeopleList.SelectedIndex;

                    // Silly work around: Change the data context to a new player to properly set up threads for a newly added player,
                    // switch to them, and then switch back if a previous player existed.

                    playerSettings.DataContext = new Player();

                    PeopleList.SelectedIndex = ActiveProcessList.Count - 1;
                    
                    if(ActiveProcessList.Count > 1)
                        PeopleList.SelectedIndex = SavedIndex;
                }));
        }



    }


}
