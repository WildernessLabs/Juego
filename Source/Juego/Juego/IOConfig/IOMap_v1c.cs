using Meadow;
using Meadow.Hardware;

namespace Juego
{
    public static class IODeviceMap
    {
        // Display
        public static (IMeadowDevice IODevice,
                       IPin ClockPin,
                       IPin CopiPin,
                       IPin CipoPin,
                       IPin DCPin,
                       IPin ResetPin,
                       IPin CSPin) Display = (
            MeadowApp.Device,
            MeadowApp.Device.Pins.SCK,
            MeadowApp.Device.Pins.MOSI,
            MeadowApp.Device.Pins.MISO,
            MeadowApp.Device.Pins.D03,
            MeadowApp.Device.Pins.D04,
            MeadowApp.Device.Pins.D14);

        // Buttons
        public static (IMeadowDevice Device,
                        IPin UpPin,
                        IPin DownPin,
                        IPin LeftPin,
                        IPin RightPin,
                        IPin SelectPin,
                        IPin StartPin) Buttons = (
            MeadowApp.Device,
            MeadowApp.Device.Pins.D06,
            MeadowApp.Device.Pins.D05,
            MeadowApp.Device.Pins.D12,
            MeadowApp.Device.Pins.D11,
            MeadowApp.Device.Pins.D15,
            MeadowApp.Device.Pins.D13);

        static IODeviceMap()
        {
        }
    }
}
