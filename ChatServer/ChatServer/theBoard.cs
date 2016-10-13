using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Net.NetworkInformation;
using ChessSrv;

namespace ChatServer
{
    public partial class theBoard : Form
    {
        private delegate void UpdateStatusCallback(string strMessage);
        public string srvName = "White";
        public string cliName = "Black";


        public theBoard()
        {
            InitializeComponent();
            txtLog.AppendText("server ip: " + localIPAddress()+"\r\n");
            label1.Text = "srvSide: " + srvName;          
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            // Parse the server's IP address out of the TextBox
            IPAddress ipAddr = IPAddress.Parse(localIPAddress());
            // Create a new instance of the ChatServer object
            ChessServer mainServer = new ChessServer(ipAddr);
            // Hook the StatusChanged event handler to mainServer_StatusChanged
            ChessServer.StatusChanged += new StatusChangedEventHandler(mainServer_StatusChanged);
            // Start listening for connections
            mainServer.StartListening();
            // Show that we started to listen for connections
            txtLog.AppendText("monitoring for connections...\r\n");
        }

        public void mainServer_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            // Call the method that updates the form
            this.Invoke(new UpdateStatusCallback(this.UpdateStatus), new object[] { e.EventMessage });
        }

        private void UpdateStatus(string strMessage)
        {
            // Updates the log with the message
            txtLog.AppendText(strMessage + "\r\n");
        }

        public static string localIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
     
            foreach (IPAddress ip in host.AddressList)
            {
                localIP = ip.ToString();
                string[] temp = localIP.Split('.');

                if (ip.AddressFamily == AddressFamily.InterNetwork && temp[0] == "192")
                {
                    break;
                }
                else
                {
                    localIP = null;
                }
            }
            return localIP;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (srvName == "White")
            {
                srvName = "Black";
                cliName = "White";
            }
            else
            {
                srvName = "White";
                cliName = "Black";
            }

            label1.Text = "srvSide: " + srvName;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void theBoard_Load(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolTip boardTips = new System.Windows.Forms.ToolTip();
            boardTips.SetToolTip(btnSwap, "Choos your side!");
        }
    }
}