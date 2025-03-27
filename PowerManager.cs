using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;

namespace LaptopBatteryInformer
{
    public sealed class PowerManager
    {
        [DllImport("kernel32.dll")]
        private static extern bool GetSystemPowerStatus(out SYSTEM_POWER_STATUS lpSystemPowerStatus);

        private static readonly Lazy<PowerManager> CurrentPowerManager =
            new(() => new(), LazyThreadSafetyMode.PublicationOnly);

        private System.Timers.Timer _timer;

        /// <summary>
        /// Occurs when [ac line status changed].
        /// </summary>
        public event EventHandler<ACLineStatus?> ACLineStatusChanged;
        /// <summary>
        /// Occurs when [battery status changed].
        /// </summary>
        public event EventHandler<BatteryStatus?> BatteryStatusChanged;
        /// <summary>
        /// Occurs when [battery level changed].
        /// </summary>
        public event EventHandler<byte> BatteryLevelChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="PowerManager"/> class.
        /// </summary>
        private PowerManager() => CheckBatteryStatus();

        /// <summary>
        /// Finalizes an instance of the <see cref="PowerManager"/> class.
        /// </summary>
        ~PowerManager() => _timer.Dispose();

        /// <summary>
        /// Gets the current Instance.
        /// </summary>
        /// <value>
        /// The current Instance.
        /// </value>
        public PowerManager Current => CurrentPowerManager.Value;

        private ACLineStatus? _acLineStatus;
        /// <summary>
        /// Gets the ac line status.
        /// </summary>
        /// <value>
        /// The ac line status.
        /// </value>
        public ACLineStatus? ACLineStatus
        {
            get => _acLineStatus;
            private set
            {
                if (_acLineStatus != value)
                {
                    _acLineStatus = value;
                    ACLineStatusChanged?.Invoke(this, _acLineStatus);
                }
            }
        }

        private BatteryStatus? _batteryStatus;
        /// <summary>
        /// Gets the battery status.
        /// </summary>
        /// <value>
        /// The battery status.
        /// </value>
        public BatteryStatus? BatteryStatus
        {
            get => _batteryStatus;
            private set
            {
                if (_batteryStatus != value)
                {
                    _batteryStatus = value;
                    BatteryStatusChanged?.Invoke(this, _batteryStatus);
                }
            }
        }

        private byte _batteryLevel;
        /// <summary>
        /// Gets the battery level.
        /// </summary>
        /// <value>
        /// The battery level.
        /// </value>
        public byte BatteryLevel
        {
            get => _batteryLevel;
            private set
            {
                if (_batteryLevel != value)
                {
                    _batteryLevel = value;
                    BatteryLevelChanged?.Invoke(this, _batteryLevel);
                }
            }
        }

        /// <summary>
        /// Runs the specified request interval.
        /// </summary>
        /// <param name="requestInterval">The request interval.</param>
        /// <returns></returns>
        public bool Run(int requestInterval)
        {
            if (_timer != null) return false;

            _timer = new(requestInterval);
            _timer.Elapsed += Timer_Elapsed;
            _timer.Start();

            return true;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e) =>
            CheckBatteryStatus();

        private void CheckBatteryStatus()
        {
            ACLineStatus? acLine = null;
            BatteryStatus? battety = null;
            byte level = 0;

            if (GetSystemPowerStatus(out SYSTEM_POWER_STATUS status))
            {
                acLine = status.ACLineStatus;
                battety = status.BatteryFlag;
                level = status.BatteryLifePercent;
            }

            ACLineStatus = acLine;
            BatteryStatus = battety;
            BatteryLevel = level;
        }
    }
}