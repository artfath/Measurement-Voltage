using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace Measurement_Voltage_MA_V3._1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string value;
        string port;
        Int32 baud;
        static SerialPort serialPort1 = new SerialPort();
        //System.Windows.Controls.Label label1 = new Label();
        Paragraph pgraph = new Paragraph();
        FlowDocument flowDoc = new FlowDocument();
        public MainWindow()
        {
            InitializeComponent();

        }
        

        

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (combo_box_port.SelectedItem != null)
            {
                ComboBoxItem cbp = (ComboBoxItem) combo_box_port.SelectedItem;
                port = cbp.Content.ToString();
            }
        }

        private void ComboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (combo_box_baudrate.SelectedItem != null)
            {
                ComboBoxItem cbb = (ComboBoxItem)combo_box_baudrate.SelectedItem;
                string baud_str = cbb.Content.ToString();

                string[] baud_str_arr = baud_str.Split(' ');

                baud = Convert.ToInt32(baud_str_arr[0]);
            }
        }

        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {                
               
                serialPort1.PortName =  port;
                serialPort1.BaudRate = baud;
                serialPort1.Parity = Parity.None;
                serialPort1.DataBits = 8;
                serialPort1.StopBits = StopBits.One;
                serialPort1.Handshake = Handshake.None;
                serialPort1.ReadTimeout = 200;
                serialPort1.WriteTimeout = 50;
                serialPort1.DataReceived += new SerialDataReceivedEventHandler(Receive_Data);
                if (!(serialPort1.IsOpen))
                {         
                
                
                serialPort1.Open();
                btn_connect.IsEnabled = false;
                btn_disconnect.IsEnabled = true;
                lbl2.Content = "Connect";
                lbl2.Foreground = Brushes.Blue;

                }

                // Read_Data();    


            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Wrong Port Number: "+ port +"\nBaud Rate: "+ baud.ToString());
                //lbl2.Content = r.ToString();
            }
            

        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((serialPort1.IsOpen))
                {
                    serialPort1.Close();
                    btn_connect.IsEnabled = true;
                    btn_disconnect.IsEnabled = false;
                    lbl2.Content = "Disconnect";
                    lbl2.Foreground = Brushes.Red;
                }
            }
            catch
            {

            }
        }

        private delegate void UpdateUiTextDelegate(string text);
        private void Receive_Data(object sender, SerialDataReceivedEventArgs e)
        {
            value = serialPort1.ReadExisting();
            Dispatcher.Invoke(DispatcherPriority.Send,
                new UpdateUiTextDelegate(WriteData), value);
            //value = serialPort1.ReadLine();
            //Console.WriteLine("received: " + value);
            //
        }

        private void WriteData(string text)
        {
            // Assign the value of the plot to the RichTextBox.
            pgraph.Inlines.Add(text);
            flowDoc.Blocks.Add(pgraph);
            rbox.Document = flowDoc;
            v1.Text = text;
        }

        private void rbox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        

        private void Button_Click_Clear(object sender, RoutedEventArgs e)
        {
            rbox.Document.Blocks.Clear();
        }
    }
}
