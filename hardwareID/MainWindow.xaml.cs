using System;
using System.Data;
using System.IO;
using System.Collections.Generic;
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
using MahApps.Metro.Controls;
using System.Management;
using System.Threading;
using System.Windows.Threading;

namespace hardwareID
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            gaga.Visibility = Visibility.Visible;
            //MessageBox.Show("ContentRendered");
        }

        private void DeschideGithub(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/filipac/csharp-cod-unic-calculator");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Thread oThread = new Thread(this.test);
            tab1.IsEnabled = false;
            oThread.IsBackground = true;
            oThread.Start();
            tab_control.SelectedItem = gaga;
            //oThread.Join();
        }

        private void test()
        {
            //gaga.Visibility = Visibility.Visible;
            this.LabelWrite(this.getUniqueID(string.Empty));
            if (preText1.Dispatcher.CheckAccess())
            {
                preText1.Visibility = Visibility.Visible;
            }
            else
            {
                preText1.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() => preText1.Visibility = Visibility.Visible));
            }
        }
        public void LabelWrite(string value)
        {
            if (undeVineCodul.Dispatcher.CheckAccess())
                undeVineCodul.Text = value;
            else
            {
                undeVineCodul.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() => undeVineCodul.Text = value));
            }
            progressRingID.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() => progressRingID.Visibility = Visibility.Hidden));
        }
        delegate void LabelWriteDelegate(string value);

        private string getUniqueID(string drive)
        {
            if (drive == string.Empty)
            {
                //Find first drive
                foreach (DriveInfo compDrive in DriveInfo.GetDrives())
                {
                    if (compDrive.IsReady)
                    {
                        drive = compDrive.RootDirectory.ToString();
                        break;
                    }
                }
            }

            if (drive.EndsWith(":\\"))
            {
                //C:\ -> C
                drive = drive.Substring(0, drive.Length - 2);
            }

            string volumeSerial = getVolumeSerial(drive);
            string cpuID = getCPUID();

            //Mix them up and remove some useless 0's
            return cpuID.Substring(13) + cpuID.Substring(1, 4) + volumeSerial + cpuID.Substring(4, 4);
        }

        private string getVolumeSerial(string drive)
        {
            ManagementObject disk = new ManagementObject(@"win32_logicaldisk.deviceid=""" + drive + @":""");
            disk.Get();

            string volumeSerial = disk["VolumeSerialNumber"].ToString();
            disk.Dispose();

            return volumeSerial;
        }

        private string getCPUID()
        {
            string cpuInfo = "";
            ManagementClass managClass = new ManagementClass("win32_processor");
            ManagementObjectCollection managCollec = managClass.GetInstances();

            foreach (ManagementObject managObj in managCollec)
            {
                if (cpuInfo == "")
                {
                    //Get only the first CPU's ID
                    cpuInfo = managObj.Properties["processorID"].Value.ToString();
                    break;
                }
            }

            return cpuInfo;
        }
    }
}
