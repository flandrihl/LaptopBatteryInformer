using System.Runtime.InteropServices;

namespace LaptopBatteryInformer
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SYSTEM_POWER_STATUS
    {
        public ACLineStatus ACLineStatus;
        public BatteryStatus BatteryFlag;
        public byte BatteryLifePercent;
        public byte SystemStatusFlag;
        public int BatteryLifeTime;
        public int BatteryFullLifeTime;
    }
}