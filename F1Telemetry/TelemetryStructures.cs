using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace F1Telemetry
{
    public class TelemetryStructures
    {
        // Documented by Codemasters, adapted for C# usage
        // Credits on how to unpack these structures (thanks guys):
        // https://forums.codemasters.com/topic/50942-f1-2020-udp-specification/
        // https://stackoverflow.com/questions/60352529/byte-array-to-struct-udp-packet

        [StructLayout(LayoutKind.Explicit, Pack = 0, Size = 24)]
        public struct PacketHeader
        {
            [FieldOffset(0)]
            public ushort m_packetFormat;         // 2020
            [FieldOffset(2)]
            public byte m_gameMajorVersion;     // Game major version - "X.00"
            [FieldOffset(3)]
            public byte m_gameMinorVersion;     // Game minor version - "1.XX"
            [FieldOffset(4)]
            public byte m_packetVersion;        // Version of this packet type, all start from 1
            [FieldOffset(5)]
            public PacketType m_packetId;        // Identifier for the packet type, see below
            [FieldOffset(6)]
            public ulong m_sessionUID;           // Unique identifier for the session
            [FieldOffset(14)]
            public float m_sessionTime;          // Session timestamp
            [FieldOffset(18)]
            public uint m_frameIdentifier;      // Identifier for the frame the data was retrieved on
            [FieldOffset(22)]
            public byte m_playerCarIndex;       // Index of player's car in the array

            [FieldOffset(23)]
            // ADDED IN BETA 2: 
            public byte m_secondaryPlayerCarIndex;  // Index of secondary player's car in the array (splitscreen)
                                                    // 255 if no second player
        }

        public enum PacketType : byte
        {
            Motion = 0, // Contains all motion data for player’s car – only sent while player is in control

            Session = 1,// Data about the session – track, time left

            LapData = 2,//  Data about all the lap times of cars in the session

            Event = 3, // Various notable events that happen during a session

            Participants = 4, // List of participants in the session, mostly relevant for multiplayer

            CarSetups = 5, // Packet detailing car setups for cars in the race

            CarTelemetry = 6,  // Telemetry data for all cars

            CarStatus = 7 //  Status data for all cars such as damage
        }

        [StructLayout(LayoutKind.Explicit, Pack = 0, Size = 63)]
        public unsafe struct CarTelemetryData
        {
            [FieldOffset(0)]
            public ushort m_speed;                         // Speed of car in kilometres per hour

            [FieldOffset(2)]
            public float m_throttle;                      // Amount of throttle applied (0.0 to 1.0)

            [FieldOffset(6)]
            public float m_steer;                         // Steering (-1.0 (full lock left) to 1.0 (full lock right))

            [FieldOffset(10)]
            public float m_brake;                         // Amount of brake applied (0.0 to 1.0)

            [FieldOffset(14)]
            public byte m_clutch;                        // Amount of clutch applied (0 to 100)

            [FieldOffset(15)]
            public sbyte m_gear;                          // Gear selected (1-8, N=0, R=-1)

            [FieldOffset(16)]
            public ushort m_engineRPM;                     // Engine RPM

            [FieldOffset(18)]
            public byte m_drs;                           // 0 = off, 1 = on

            [FieldOffset(19)]
            public byte m_revLightsPercent;              // Rev lights indicator (percentage)

            [FieldOffset(20)]
            public fixed ushort m_brakesTemperature[4];          // Brakes temperature (celsius)

            [FieldOffset(28)]
            public fixed byte m_tyresSurfaceTemperature[4];    // Tyres surface temperature (celsius)

            [FieldOffset(36)]
            public fixed byte m_tyresInnerTemperature[4];      // Tyres inner temperature (celsius)

            [FieldOffset(44)]
            public byte m_engineTemperature;             // Engine temperature (celsius)

            [FieldOffset(46)]
            public fixed float m_tyresPressure[4];              // Tyres pressure (PSI)

            [FieldOffset(62)]
            public fixed byte m_surfaceType[4];                // Driving surface, see appendices
        };

        [StructLayout(LayoutKind.Explicit, Pack = 0, Size = 53)]
        public unsafe struct PacketCarTelemetryData
        {
            [FieldOffset(0)]
            public PacketHeader m_header;         // Header

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
            [FieldOffset(24)]
            public CarTelemetryData[] m_carTelemetryData;

            [FieldOffset(46)]
            public UInt32 m_buttonStatus;        // Bit flags specifying which buttons are being pressed
                                          // currently - see appendices
            [FieldOffset(50)]
            // Added in Beta 3:
            public byte m_mfdPanelIndex;       // Index of MFD panel open - 255 = MFD closed
                                        // Single player, race – 0 = Car setup, 1 = Pits
                                        // 2 = Damage, 3 =  Engine, 4 = Temperatures
                                        // May vary depending on game mode
            [FieldOffset(51)]
            public byte m_mfdPanelIndexSecondaryPlayer;   // See above
            [FieldOffset(52)]
            public sbyte m_suggestedGear;       // Suggested gear for the player (1-8)
                                        // 0 if no gear suggested
        };
    }
}
