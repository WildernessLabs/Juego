using Meadow.Foundation.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Juego.Apps
{
    public partial class Clock
    {
        FontBase fontClock;
        FontBase fontDate;
        FontBase FontText;

        public void Init(GraphicsLibrary gl)
        {
            gl.CurrentFont = fontClock = new Font12x16();
            fontDate = new Font8x12();
            FontText = new Font4x8();

            MeadowApp.Device.SetClock(new DateTime(2021, 3, 1));

            return; 

            gl.Clear();
            gl.DrawText(0, 0, "Meadow Clock");
            gl.DrawText(0, 16, "v0.0.1");
            gl.Show();

            Thread.Sleep(500);
        }

        public void Update(GraphicsLibrary gl)
        {
            if(state == ClockState.Clock)
            {
                UpdateClock(gl);
            }
            else if(state == ClockState.StopWatch)
            {
                UpdateStopWatch(gl);
            }
            else
            {
                UpdateIntervalTimer(gl);
            }
        }

        string GetStopwatchTime(long ticks)
        {
            ticks /= 1000000;

            long h = ticks / 36000;
            ticks %= 36000;

            long m = ticks / 600;
            ticks %= 600;

            long s = ticks / 10;
            long ms = ticks %= 10;

            return $"{h}:{m:00}:{s:00}.{ms:0}";
        }

        void UpdateIntervalTimer(GraphicsLibrary gl)
        {
            gl.Clear();

            gl.CurrentFont = fontClock;

            long seconds = ((DateTime.Now - stopWatchStart).Ticks + stopwatchOffset) / 10000000;

            gl.DrawText(0, 0, $"Round {intervalCount}", alignment: GraphicsLibrary.TextAlignment.Left);

            gl.DrawText(gl.Width, 20, "3:00", alignment: GraphicsLibrary.TextAlignment.Right);

            gl.DrawText(gl.Width, 40, "1:00", alignment: GraphicsLibrary.TextAlignment.Right);

            gl.Show();
        }

        void UpdateStopWatch(GraphicsLibrary gl)
        {
            gl.Clear();
            gl.CurrentFont = FontText;
            gl.DrawText(0, 0, "Total");

            gl.CurrentFont = fontDate;

            if(swState == StopWatchState.Start)
            {
                gl.DrawText(0, 8, GetStopwatchTime((DateTime.Now - stopWatchStart).Ticks + stopwatchOffset));
            }
            else
            {
                gl.DrawText(0, 8, GetStopwatchTime(stopwatchOffset));
            }
            
            if(splits.Count > 0)
            {
                //current lap
                if(swState == StopWatchState.Start)
                {
                    gl.DrawText(0, 30, GetStopwatchTime((DateTime.Now - stopWatchStart).Ticks));
                }
                else
                {
                    gl.DrawText(0, 30, GetStopwatchTime(stopwatchOffset - splits.LastOrDefault()));
                }
               
                //last split
                gl.DrawText(0, 52, GetStopwatchTime(splits.LastOrDefault()));

                gl.CurrentFont = FontText;
                gl.DrawText(0, 22, $"Lap {splits.Count + 1}");
                gl.DrawText(0, 44, $"Lap {splits.Count} split");
            }

            gl.Show();
        }

        void UpdateClock(GraphicsLibrary gl)
        { 
            gl.Clear();

            gl.CurrentFont = fontClock;
            gl.DrawText(gl.Width / 2, 10, DateTime.Now.ToString("h:mm:sstt"), alignment: GraphicsLibrary.TextAlignment.Center);

            gl.CurrentFont = fontDate;
            gl.DrawText(gl.Width / 2, 30, DateTime.Now.ToString("dddd"), alignment: GraphicsLibrary.TextAlignment.Center);
            gl.DrawText(gl.Width / 2, 46, DateTime.Now.ToString("MMMM d, yyyy"), alignment: GraphicsLibrary.TextAlignment.Center);

            gl.Show();

            if(state == ClockState.Clock)
            {
                Thread.Sleep(100);
            }
        }   
    }
}