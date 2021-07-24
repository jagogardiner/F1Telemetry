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
        // Read generic marshaling
        public static T Read<T>(ReadOnlySpan<byte> bytes) where T : struct
        => MemoryMarshal.Cast<byte, T>(bytes)[0];

        [StructLayout(LayoutKind.Explicit, Pack = 0, Size = 8)]
        public struct UInt16Quad
        {
            [FieldOffset(0)]
            public ushort A;
            [FieldOffset(2)]
            public ushort B;
            [FieldOffset(6)]
            public ushort C;
            [FieldOffset(6)]
            public ushort D;
        }
        [StructLayout(LayoutKind.Explicit, Pack = 0, Size = 16)]
        public struct SingleQuad
        {
            [FieldOffset(0)]
            public float A;
            [FieldOffset(4)]
            public float B;
            [FieldOffset(8)]
            public float C;
            [FieldOffset(12)]
            public float D;
        }
        [StructLayout(LayoutKind.Explicit, Pack = 0, Size = 4)]
        public struct ByteQuad
        {
            [FieldOffset(0)]
            public byte A;
            [FieldOffset(1)]
            public byte B;
            [FieldOffset(2)]
            public byte C;
            [FieldOffset(3)]
            public byte D;
        }

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

        public struct CarMotionData
        {
            public float m_worldPositionX;           // World space X position
            public float m_worldPositionY;           // World space Y position
            public float m_worldPositionZ;           // World space Z position
            public float m_worldVelocityX;           // Velocity in world space X
            public float m_worldVelocityY;           // Velocity in world space Y
            public float m_worldVelocityZ;           // Velocity in world space Z
            public short m_worldForwardDirX;         // World space forward X direction (normalised)
            public short m_worldForwardDirY;         // World space forward Y direction (normalised)
            public short m_worldForwardDirZ;         // World space forward Z direction (normalised)
            public short m_worldRightDirX;           // World space right X direction (normalised)
            public short m_worldRightDirY;           // World space right Y direction (normalised)
            public short m_worldRightDirZ;           // World space right Z direction (normalised)
            public float m_gForceLateral;            // Lateral G-Force component
            public float m_gForceLongitudinal;       // Longitudinal G-Force component
            public float m_gForceVertical;           // Vertical G-Force component
            public float m_yaw;                      // Yaw angle in radians
            public float m_pitch;                    // Pitch angle in radians
            public float m_roll;                     // Roll angle in radians
        };

        unsafe public struct PacketMotionData
        {
            PacketHeader m_header;                  // Header

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
            CarMotionData[] m_carMotionData;      // Data for all cars on track

            // Extra player car ONLY data
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] m_suspensionPosition;      // Note: All wheel arrays have the following order:
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] m_suspensionVelocity;      // RL, RR, FL, FR
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] m_suspensionAcceleration;  // RL, RR, FL, FR
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] m_wheelSpeed;              // Speed of each wheel
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] m_wheelSlip;               // Slip ratio for each wheel
            public float m_localVelocityX;             // Velocity in local space
            public float m_localVelocityY;             // Velocity in local space
            public float m_localVelocityZ;             // Velocity in local space
            public float m_angularVelocityX;           // Angular velocity x-component
            public float m_angularVelocityY;           // Angular velocity y-component
            public float m_angularVelocityZ;           // Angular velocity z-component
            public float m_angularAccelerationX;       // Angular velocity x-component
            public float m_angularAccelerationY;       // Angular velocity y-component
            public float m_angularAccelerationZ;       // Angular velocity z-component
            public float m_frontWheelsAngle;           // Current front wheels angle in radians
        };

        [StructLayout(LayoutKind.Explicit, Pack = 0, Size = 63)]
        public struct CarTelemetryData
        {
            [FieldOffset(0)]
            public ushort m_speed;                    // Speed of car in kilometres per hour
            [FieldOffset(2)]
            public float m_throttle;                 // Amount of throttle applied (0.0 to 1.0)
            [FieldOffset(6)]
            public float m_steer;                    // Steering (-1.0 (full lock left) to 1.0 (full lock right))
            [FieldOffset(10)]
            public float m_brake;                    // Amount of brake applied (0.0 to 1.0)
            [FieldOffset(14)]
            public byte m_clutch;                   // Amount of clutch applied (0 to 100)
            [FieldOffset(15)]
            public sbyte m_gear;                     // Gear selected (1-8, N=0, R=-1)
            [FieldOffset(16)]
            public ushort m_engineRPM;                // Engine RPM
            [FieldOffset(18)]
            public byte m_drs;                      // 0 = off, 1 = on
            [FieldOffset(19)]
            public byte m_revLightsPercent;         // Rev lights indicator (percentage)
            [FieldOffset(20)]
            public UInt16Quad m_brakesTemperature;     // Brakes temperature (celsius)
            [FieldOffset(28)]
            public UInt16Quad m_tyresSurfaceTemperature; // Tyres surface temperature (celsius)
            [FieldOffset(36)]
            public UInt16Quad m_tyresInnerTemperature; // Tyres inner temperature (celsius)
            [FieldOffset(44)]
            public ushort m_engineTemperature;        // Engine temperature (celsius)
            [FieldOffset(46)]
            public SingleQuad tyresPressure;         // Tyres pressure (PSI)
            [FieldOffset(62)]
            public ByteQuad m_surfaceType;           // Driving surface, see appendice
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
