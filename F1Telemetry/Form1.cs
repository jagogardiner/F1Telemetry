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
            GetPacket(Client, RemoteIP);
        }

        // unsafe generic method because we are confident that the data sent matches our struct
        // https://stackoverflow.com/questions/2871/reading-a-c-c-data-structure-in-c-sharp-from-a-byte-array
        unsafe T ByteArrayToStructure<T>(byte[] bytes) where T : struct
        {
            fixed (byte* ptr = &bytes[0])
            {
                return (T)Marshal.PtrToStructure((IntPtr)ptr, typeof(T));
            }
        }

        public unsafe void GetPacket(UdpClient Client, IPEndPoint RemoteIP)
        {
            while (true)
            {
                try
                {
                    byte[] rec = Client.Receive(ref RemoteIP);
                    ReadOnlySpan<byte> Bytes = rec;
                    var header = MemoryMarshal.Cast<byte, TelemetryStructures.PacketHeader>(Bytes)[0];
                    Bytes = Bytes.Slice(Unsafe.SizeOf<TelemetryStructures.PacketHeader>());
                    switch (header.m_packetId)
                    {
                        case TelemetryStructures.PacketType.CarTelemetry:
                            int carIndex = 0;
                            foreach (var telemetry in MemoryMarshal.Cast<byte, TelemetryStructures.CarTelemetryData>(Bytes))
                            {
                                Console.WriteLine($"car: {carIndex}, gear: {telemetry.m_gear}, speed: {telemetry.m_speed}");
                                carIndex++;
                            }
                            Console.WriteLine();
                            System.Threading.Thread.Sleep(500);
                            Console.Clear();
                            break;
                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }
    }
}
