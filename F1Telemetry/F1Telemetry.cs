using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static F1Telemetry.TelemetryStructures;

namespace F1Telemetry
{
    public class F1Telemetry
    {
        private UdpClient udpClient;
        private IPEndPoint ipEndPoint;

        public bool connected { get; set; }

        public F1Telemetry(int port)
        {
            udpClient = new UdpClient(port);
            ipEndPoint = new IPEndPoint(IPAddress.Any, port);

            udpClient.BeginReceive(new AsyncCallback(RecieveCallback), null);
        }

        private void RecieveCallback(IAsyncResult result)
        {
            byte[] rec = udpClient.EndReceive(result, ref ipEndPoint);
            udpClient.BeginReceive(new AsyncCallback(RecieveCallback), null);

            // Using a GCHandle and Marshaling so we don't have to deal with unsafe code.
            GCHandle handle = GCHandle.Alloc(rec, GCHandleType.Pinned);

            try
            {
                // Get packet header
            }
            catch (Exception ex)
            {
                PacketHeader packetHeader = (PacketHeader)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(PacketHeader));
                switch(packetHeader.m_packetId)
                {
                    case PacketType.CarSetups:
                        return;
                    case PacketType.CarStatus:
                        return;
                    case PacketType.CarTelemetry:
                        return;
                    case PacketType.Event:
                        return;
                    case PacketType.LapData:
                        return;
                    case PacketType.Motion:
                        return;
                    case PacketType.Participants:
                        return;
                    case PacketType.Session:
                        return;
                }
            }
        }
    }
}
