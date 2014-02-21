using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using ZargateTicketPrint.Classes;
using ZargateTicketPrint.Database;
using ZargateTicketPrint.ZebraHelpers;

namespace ZargateTicketPrint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly DispatcherTimer _printTimer = new DispatcherTimer {Interval = new TimeSpan(3000)};
        private Thread _printThread;

        public MainWindow()
        {
            InitializeComponent();
            Logger.Add("Launched program");
            DataGridLog.DataContext = Logger.GetLog();
            Logger.GetLog().CollectionChanged += MainWindow_CollectionChanged;

            cmbBoxPrinterName1.DataContext = Printers.InstalledPrinters();
            cmbBoxPrinterName1.ItemsSource = Printers.InstalledPrinters();
            cmbBoxPrinterName1.SelectedIndex = 0;
            if (Printer.Default.PrinterName1 != "")
            {
                try
                {
                    cmbBoxPrinterName1.SelectedItem = Printer.Default.PrinterName1;
                }
                catch (Exception)
                {
                    cmbBoxPrinterName1.SelectedItem = Printers.DefaultZebraPrinter();
                }
            }
            Logger.Add("Selected \"" + cmbBoxPrinterName1.SelectedItem + "\" as ticket printer1.");

            cmbBoxPrinterName2.DataContext = Printers.InstalledPrinters();
            cmbBoxPrinterName2.ItemsSource = Printers.InstalledPrinters();
            cmbBoxPrinterName2.SelectedIndex = 0;
            if (Printer.Default.PrinterName2 != "")
            {
                try
                {
                    cmbBoxPrinterName2.SelectedItem = Printer.Default.PrinterName2;
                }
                catch (Exception)
                {
                    cmbBoxPrinterName2.SelectedItem = Printers.DefaultZebraPrinter();
                }
            }
            Logger.Add("Selected \"" + cmbBoxPrinterName2.SelectedItem + "\" as ticket printer2.");

            txtDatabaseServer.Text = Mysql.Default.server;
            txtDatabaseDatabase.Text = Mysql.Default.database;
            txtDatabaseUsername.Text = Mysql.Default.username;
            txtDatabasePassword.Password = Mysql.Default.password;

            if (txtDatabaseServer.Text == "" || txtDatabaseDatabase.Text == "" || txtDatabaseUsername.Text == "" ||
                txtDatabasePassword.Password == "")
            {
                btnAutoPrintStart.IsEnabled = false;
            }

            _printTimer.Tick += _printTimer_Tick;

            TrayIcon.Visibility = Visibility.Hidden;
            TrayIcon.TrayMouseDoubleClick += TrayIcon_TrayMouseDoubleClick;
        }

        void MainWindow_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            refreshLogView();
            if (_printTimer.IsEnabled && Logger.GetLog()[Logger.GetLog().Count-1].Severity == Logger.Severity.ERROR)
            {
                autoPrintStop();
            }
        }
        private void TrayIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            Show();
            WindowState = WindowState.Normal;
            TrayIcon.Visibility = Visibility.Hidden;
        }

        private void _printTimer_Tick(object sender, EventArgs e)
        {
            if (_printThread == null || !_printThread.IsAlive)
            {
                _printThread = new Thread(new Tickets().PrintTicketsFromDatabase);
                _printThread.Start();
            }
            refreshLogView();
        }

        private void refreshLogView()
        {
            if (DataGridLog.Items.Count > 0)
            {
                var border = VisualTreeHelper.GetChild(DataGridLog, 0) as Decorator;
                if (border != null)
                {
                    var scroll = border.Child as ScrollViewer;
                    if (scroll != null) scroll.ScrollToEnd();
                }
            }
        }

        private void btnSettingsSave_Click(object sender, RoutedEventArgs e)
        {
            Printer.Default.PrinterName1 = cmbBoxPrinterName1.SelectedItem.ToString();
            Printer.Default.PrinterName2 = cmbBoxPrinterName2.SelectedItem.ToString();
            Logger.Add("Selected \"" + Printer.Default.PrinterName1 + "\" as ticket printer1.");
            Logger.Add("Selected \"" + Printer.Default.PrinterName2 + "\" as ticket printer2.");
            Printer.Default.Save();


            Mysql.Default.server = txtDatabaseServer.Text;
            Mysql.Default.database = txtDatabaseDatabase.Text;
            Mysql.Default.username = txtDatabaseUsername.Text;
            Mysql.Default.password = txtDatabasePassword.Password;
            Mysql.Default.Save();

            if (txtDatabaseServer.Text == "" || txtDatabaseDatabase.Text == "" || txtDatabaseUsername.Text == "" ||
                txtDatabasePassword.Password == "")
            {
                btnAutoPrintStart.IsEnabled = false;
            }
            else
            {
                btnAutoPrintStart.IsEnabled = true;
            }
        }

        private void btnAutoPrintStart_Click(object sender, RoutedEventArgs e)
        {
            _printTimer.Start();
            btnAutoPrintStart.IsEnabled = false;
            btnAutoPrintStop.IsEnabled = true;
            lblStatus.Content = "Running";
            lblStatus.Foreground = Brushes.LimeGreen;
            Logger.Add("Started auto printing of tickets.", Logger.Severity.INFO);
        }

        private void btnAutoPrintStop_Click(object sender, RoutedEventArgs e)
        {
            autoPrintStop();
        }

        private void autoPrintStop()
        {
            _printTimer.Stop();
            btnAutoPrintStart.IsEnabled = true;
            btnAutoPrintStop.IsEnabled = false;
            lblStatus.Content = "Stopped";
            lblStatus.Foreground = Brushes.Red;
            Thread.Sleep(100);
            Logger.Add("Stopped auto printing of tickets.", Logger.Severity.WARNING);
        }

        private void Window_StateChanged_1(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();
                TrayIcon.Visibility = Visibility.Visible;
            }
        }
    }
}