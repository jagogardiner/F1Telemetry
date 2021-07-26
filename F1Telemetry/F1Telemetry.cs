using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        // public packet data objects
        // for debugging until I figure these structures out completely
        public PacketHeader packetHeader;
        public PacketMotionData packetMotionData;
        public PacketSessionData packetSessionData;
        public PacketLapData packetLapData;
        public PacketEventData packetEventData;
        public PacketParticipantsData packetParticipantsData;
        public PacketCarSetupData packetCarSetupData;
        public PacketCarTelemetryData packetCarTelemetryData;
        public PacketCarStatusData packetCarStatusData;
        public PacketFinalClassificationData packetFinalClassificationData;
        public PacketLobbyInfoData packetLobbyInfoData;

        public CarTelemetryData playerCarTelemetryData = new CarTelemetryData();
        public CarStatusData playerCarStatusData = new CarStatusData();
        public CarSetupData playerCarSetupData = new CarSetupData();
        public LapData playerLapData = new LapData();
        public CarMotionData playerCarMotionData = new CarMotionData();

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
                packetHeader = (PacketHeader)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(PacketHeader));
                switch (packetHeader.m_packetId)
                {
                    case PacketType.CarSetups:
                        packetCarSetupData = (PacketCarSetupData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(PacketCarSetupData));
                        return;
                    case PacketType.CarStatus:
                        packetCarStatusData = (PacketCarStatusData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(PacketCarStatusData));
                        return;
                    case PacketType.CarTelemetry:
                        packetCarTelemetryData = (PacketCarTelemetryData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(PacketCarTelemetryData));
                        return;
                    case PacketType.Event:
                        packetEventData = (PacketEventData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(PacketEventData));
                        return;
                    case PacketType.LapData:
                        packetLapData = (PacketLapData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(PacketLapData));
                        return;
                    case PacketType.Motion:
                        packetMotionData = (PacketMotionData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(PacketMotionData));
                        return;
                    case PacketType.Participants:
                        packetParticipantsData = (PacketParticipantsData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(PacketParticipantsData));
                        return;
                    case PacketType.Session:
                        packetSessionData = (PacketSessionData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(PacketSessionData));
                        return;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to recieve packet");
            }
            finally
            {
                handle.Free();
            }
        }

        // Right now just getting player Telemetry
        public void UpdatePlayerTelemetry()
        {
            // Get player index
            int playerIndex = packetHeader.m_playerCarIndex;

            // Update telemetry object depending on new packet ID
            switch (packetHeader.m_packetId)
            {
                case PacketType.CarTelemetry:
                    playerCarTelemetryData = packetCarTelemetryData.m_carTelemetryData[playerIndex];
                    break;
                case PacketType.CarStatus:
                    playerCarStatusData = packetCarStatusData.m_carStatusData[playerIndex];
                    break;
                case PacketType.CarSetups:
                    playerCarSetupData = packetCarSetupData.m_carSetups[playerIndex];
                    break;
                case PacketType.LapData:
                    playerLapData = packetLapData.m_lapData[playerIndex];
                    break;
                case PacketType.Motion:
                    playerCarMotionData = packetMotionData.m_carMotionData[playerIndex];
                    break;
            }

            //if(packet.m_header.m_packetId == PacketType.CarTelemetry)
            //{
            //    int playerIndex = packet.m_header.m_playerCarIndex;
            //    CarTelemetryData playerData = packet.m_carTelemetryData[playerIndex];

            //    string gear = "";

            //    if(playerData.m_gear > packet.m_suggestedGear && packet.m_suggestedGear != 0)
            //    {
            //        gear = playerData.m_gear + " v " +"("+packet.m_suggestedGear+")";
            //    } else if(playerData.m_gear < packet.m_suggestedGear && packet.m_suggestedGear != 0)
            //    {
            //        gear = playerData.m_gear + " ^ " + "(" + packet.m_suggestedGear + ")";
            //    }
            //    else
            //    {
            //        gear = playerData.m_gear.ToString();
            //    }

            //    Console.SetCursorPosition(0, 0);
            //    Console.WriteLine($"Throttle (linear 0 - 1): {playerData.m_throttle}".PadRight(Console.WindowWidth, ' '));
            //    Console.WriteLine($"Brake (linear 0 - 1): {playerData.m_brake}".PadRight(Console.WindowWidth, ' '));
            //    Console.WriteLine($"Steering (-1 - 1): {playerData.m_steer}".PadRight(Console.WindowWidth, ' '));
            //    Console.WriteLine($"Speed (km/h): {playerData.m_speed}".PadRight(Console.WindowWidth, ' '));
            //    Console.WriteLine($"RPM: {playerData.m_engineRPM}".PadRight(Console.WindowWidth, ' '));
            //    Console.WriteLine($"Gear (-1 - 8): {gear}".PadRight(Console.WindowWidth, ' '));
            //    Console.WriteLine($"DRS: {playerData.m_drs}".PadRight(Console.WindowWidth, ' '));
            //    Console.WriteLine($"Engine temperature (deg C): {playerData.m_engineTemperature}".PadRight(Console.WindowWidth, ' '));
            //    Console.WriteLine($"Session time: {packet.m_header.m_sessionTime}".PadRight(Console.WindowWidth, ' '));
            //    Console.WriteLine($"Suggested Gear: {packet.m_suggestedGear}".PadRight(Console.WindowWidth, ' '));
            //}
        }
    }
}
