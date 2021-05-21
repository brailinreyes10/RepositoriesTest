using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SendSMSProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void SendSMS()
        {
            try
            {
                SerialPort sp = new SerialPort(
                txtPost.Text,      // port name
                38400,       // baud rate
                Parity.None, // parity
                8,           // bits
                StopBits.One // stop bits
            );

                sp.Handshake = Handshake.None;
                sp.WriteTimeout = -1;

                sp.Open();
                sp.WriteLine("AT" + Environment.NewLine);
                Thread.Sleep(100);
                sp.WriteLine("AT+CMGF=1" + Environment.NewLine);
                Thread.Sleep(100);
                sp.WriteLine("AT+CMGS=\"" + txtPhoneNumber.Text + "\"" + Environment.NewLine);
                Thread.Sleep(100);
                sp.WriteLine(txtMessage.Text + Environment.NewLine);
                Thread.Sleep(100);
                sp.Write(new byte[] { 26 }, 0, 1);
                Thread.Sleep(100);

                var response = sp.ReadExisting();
                if (response.Contains("ERROR"))
                {
                    MessageBox.Show("Send Failed", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("SMS Send", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                sp.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSendSMS_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            SendSMS();

            Cursor = Cursors.Default;
        }
    }
}
