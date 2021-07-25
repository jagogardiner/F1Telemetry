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
        // public packet data objects
        // for debugging until I figure these structures out completely
        PacketHeader packetHeader;
        CarMotionData carMotionData;
        PacketMotionData packetMotionData;
        MarshalZone marshalZone;
        WeatherForecastSample weatherForecastSample;
        PacketSessionData packetSessionData;
        LapData lapData;
        PacketLapData packetLapData;
        FastestLap fastestLap;
        Retirement retirement;
        TeamMateInPits teamMateInPits;
        RaceWinner raceWinner;
        Penalty penalty;
        SpeedTrap speedTrap;
        EventDataDetails eventDataDetails;
        PacketEventData packetEventData;
        ParticipantData participantData;
        PacketParticipantsData packetParticipantsData;
        CarSetupData carSetupData;
        PacketCarSetupData packetCarSetupData;
        CarTelemetryData carTelemetryData;
        public PacketCarTelemetryData packetCarTelemetryData;
        CarStatusData carStatusData;
        PacketCarStatusData packetCarStatusData;
        FinalClassificationData finalClassificationData;
        PacketFinalClassificationData packetFinalClassificationData;
        LobbyInfoData lobbyInfoData;
        PacketLobbyInfoData packetLobbyInfoData;

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
                PacketHeader packetHeader = (PacketHeader)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(PacketHeader));
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
            }
            finally
            {
                handle.Free();
            }
        }

        public static void UpdateTelemetry(PacketCarTelemetryData packet)
        {
            int playerIndex = packet.m_header.m_playerCarIndex;
            try
            {
                CarTelemetryData playerData = packet.m_carTelemetryData[playerIndex];

                Console.SetCursorPosition(0, 0);
                Console.WriteLine($"Throttle (linear 0 - 1): {playerData.m_throttle}".PadRight(Console.WindowWidth, ' '));
                Console.WriteLine($"Brake (linear 0 - 1): {playerData.m_brake}".PadRight(Console.WindowWidth, ' '));
                Console.WriteLine($"Steering (-1 - 1): {playerData.m_steer}".PadRight(Console.WindowWidth, ' '));
                Console.WriteLine($"Speed (km/h): {playerData.m_speed}".PadRight(Console.WindowWidth, ' '));
                Console.WriteLine($"RPM: {playerData.m_engineRPM}".PadRight(Console.WindowWidth, ' '));
                Console.WriteLine($"Gear (0 - 7): {playerData.m_gear}".PadRight(Console.WindowWidth, ' '));
                Console.WriteLine($"DRS: {playerData.m_drs}".PadRight(Console.WindowWidth, ' '));
                Console.WriteLine($"Engine temperature (deg C): {playerData.m_engineTemperature}".PadRight(Console.WindowWidth, ' '));
                Console.WriteLine($"Session time: {packet.m_header.m_sessionTime}".PadRight(Console.WindowWidth, ' '));
            } catch(Exception e)
            {

            }
        }
    }
}
