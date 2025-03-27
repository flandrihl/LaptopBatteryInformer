using System;

namespace LaptopBatteryInformer
{
    [Flags]
    public enum BatteryStatus : byte
    {
        /// <summary>
        /// The high (>66%)
        /// </summary>
        High = 1 << 0,      //Высокий (>66%)
        /// <summary>
        /// The short (<33%)
        /// </summary>
        Short = 1 << 1,     //Низкий (<33%)
        /// <summary>
        /// The critical (<5%)
        /// </summary>
        Critical = 1 << 2,  //Критический
        /// <summary>
        /// The charging
        /// </summary>
        Charging = 1 << 3,  //Заряжается
        /// <summary>
        /// The not present
        /// </summary>
        NotPresent = 128    //Батарея отсутствует
    }
}