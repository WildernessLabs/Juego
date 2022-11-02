using System;
using System.Threading;
using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Juego.Core;
using Meadow.Units;

namespace MeadowApp
{
	// Change F7FeatherV2 to F7FeatherV1 for V1.x boards
	public class MeadowApp : App<F7CoreComputeV2>
	{
        JuegoHardware_v2 Hardware { get; set; }

        public override Task Initialize()
        {
            Console.WriteLine("App.Initialize()");

            Hardware = new JuegoHardware_v2(Device);

            return base.Initialize();
        }

        public async override Task Run()
        {
            Console.WriteLine("Run.");

            for (int i = 0; i < 5; i++) {
                await Hardware.LeftSpeaker.PlayTone(new Frequency(440), TimeSpan.FromMilliseconds(500));
            }

            return;
        }
    }
}
