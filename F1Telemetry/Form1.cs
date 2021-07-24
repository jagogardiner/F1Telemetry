using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using EnvDTE;
using EnvDTE80;

namespace F1Telemetry
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var Client = new UdpClient(20777);
            var RemoteIP = new IPEndPoint(IPAddress.Any, 60420);

            F1Telemetry f1 = new F1Telemetry(20777);
        }
    }
}
