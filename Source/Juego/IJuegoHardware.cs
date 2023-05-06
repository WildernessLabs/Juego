using Meadow.Foundation.Audio;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;

namespace WildernessLabs.Hardware.Juego
{
    public interface IJuegoHardware
    {
        public IGraphicsDisplay? Display { get; }

        public PushButton? Right_UpButton { get; }
        public PushButton? Right_DownButton { get; }
        public PushButton? Right_LeftButton { get; }
        public PushButton? Right_RightButton { get; }

        public PushButton? Left_UpButton { get; }
        public PushButton? Left_DownButton { get; }
        public PushButton? Left_LeftButton { get; }
        public PushButton? Left_RightButton { get; }

        public PushButton? StartButton { get; }
        public PushButton? SelectButton { get; }

        public PiezoSpeaker? LeftSpeaker { get; }
        public PiezoSpeaker? RightSpeaker { get; }

        public PwmLed? BlinkyLed { get; }
    }
}