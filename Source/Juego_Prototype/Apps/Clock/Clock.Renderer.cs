using Meadow;
using Meadow.Foundation;
using Meadow.Foundation.Displays.TextDisplayMenu;
using Meadow.Foundation.Graphics;
using System;
using System.Linq;
using System.Threading;

namespace Juego.Apps
{
    public partial class Clock
    {
        IFont fontClock;
        IFont fontDate;
        IFont FontText;

        Menu menu;

        const string ActiveID = "active";
        const string RestId = "rest";
        const string SetTime = "setTime";
        const string SetDate = "setDate";

        public void Init(MicroGraphics gl)
        {
            gl.CurrentFont = fontClock = new Font12x16();
            fontDate = new Font8x12();
            FontText = new Font4x8();

            MeadowApp.Device.SetClock(new DateTime(2021, 3, 1));

            InitMenu(gl);

            return; 

            gl.Clear();
            gl.DrawText(0, 0, "Meadow Clock");
            gl.DrawText(0, 16, "v0.1.0");
            gl.Show();

            Thread.Sleep(500);
        }

        public void Update(IIOConfig ioConfig)
        {
            var gl = ioConfig.Graphics;

            if (menu != null && menu.IsEnabled)
            {
                return;
            }

            if(state == ClockState.Clock)
            {
                UpdateClock(ioConfig);
            }
            else if(state == ClockState.StopWatch)
            {
                UpdateStopWatch(ioConfig);
            }
            else
            {
                UpdateIntervalTimer(ioConfig);
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

        void UpdateIntervalTimer(IIOConfig ioConfig)
        {
            var gl = ioConfig.Graphics;

            gl.Clear();

            gl.CurrentFont = fontClock;

            long seconds = ((DateTime.Now - intervalStart).Ticks) / 10000000;
            if(itState == IntervalTimerState.Stop) { seconds = 0; }

            gl.DrawText(gl.Width - 2, 0, GetTotalTime(seconds), alignmentH: HorizontalAlignment.Right);
            gl.DrawText(gl.Width - 2, 24, GetActiveTime(seconds), alignmentH: HorizontalAlignment.Right);
            gl.DrawText(gl.Width - 2, 48, GetRestTime(seconds), alignmentH: HorizontalAlignment.Right);

            gl.CurrentFont = fontDate;

            gl.DrawText(4, 02, $"SET { seconds / (interval1 + interval2) + 1}");
            gl.DrawText(4, 26, "ACTIVE");
            gl.DrawText(4, 50, "REST");

            if(seconds % (interval1 + interval2) < interval1)
            {
                gl.DrawRectangle(0, 22, gl.Width, 18);
                ioConfig.rgbLed.SetColor(Color.Green);
            }
            else
            {
                gl.DrawRectangle(0, 46, gl.Width, 18);
                ioConfig.rgbLed.SetColor(Color.Red);
            }

            gl.Show();
        }

        string GetTotalTime(long seconds)
        {
            return $"{seconds / 60}:{seconds % 60:00}";
        }

        string GetActiveTime(long seconds)
        {
            int cycleTotal = interval1 + interval2;

            var cycleTime = seconds % cycleTotal;

            int activeTime = 0;

            if(cycleTime >=interval1)
            {
                activeTime = 0;
            }
            else
            {
                activeTime = interval1 - (int)cycleTime;
            }

            return $"{activeTime / 60}:{activeTime % 60:00}";
        }

        string GetRestTime(long seconds)
        {
            int cycleTotal = interval1 + interval2;

            var cycleTime = seconds % cycleTotal;

            int restTime = 0;

            if (cycleTime < interval1)
            {
                restTime = interval2;
            }
            else
            {
                restTime = interval2 - (int)cycleTime + interval1;
            }

            return $"{restTime / 60}:{restTime % 60:00}";
        }

        void UpdateStopWatch(IIOConfig ioConfig)
        {
            var gl = ioConfig.Graphics;

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

        void UpdateClock(IIOConfig ioConfig)
        {
            ioConfig.rgbLed.SetColor(Color.Black);
            var gl = ioConfig.Graphics;

            gl.Clear();

            gl.CurrentFont = fontClock;
            gl.DrawText(gl.Width / 2, 10, DateTime.Now.ToString("h:mm:sstt"), alignmentH: HorizontalAlignment.Center);

            gl.CurrentFont = fontDate;
            gl.DrawText(gl.Width / 2, 30, DateTime.Now.ToString("dddd"), alignmentH: HorizontalAlignment.Center);
            gl.DrawText(gl.Width / 2, 46, DateTime.Now.ToString("MMMM d, yyyy"), alignmentH: HorizontalAlignment.Center);

            gl.Show();

            if(state == ClockState.Clock)
            {
                Thread.Sleep(100);
            }
        }   

        void InitMenu(MicroGraphics gl)
        {
            Resolver.Log.Info("InitMenu");

            var menuItems = new MenuItem[]
            {
                new MenuItem("Clock",
                    subItems: new MenuItem[]{new MenuItem("Date", id: SetDate, type: "Date", value: new DateTime(2021, 2, 20)),
                                             new MenuItem("Time", id: SetTime, type: "Time", value: new TimeSpan(12, 45, 30))
                    }),
                new MenuItem("Intervals",
                    subItems: new MenuItem[]{new MenuItem("Active duration", id: ActiveID, type: "TimeShort", value: new TimeSpan(0, 0, interval1)),
                                             new MenuItem("Rest duration", id: RestId, type: "TimeShort", value: new TimeSpan(0, 0, interval2)) 
                    })
             };

            menu = new Menu(gl, menuItems);

            menu.ValueChanged += Menu_ValueChanged;
        }

        private void Menu_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            switch (e.ItemID)
            {
                case ActiveID:
                    interval1 = (int)((TimeSpan)e.Value).TotalSeconds;
                    break;
                case RestId:
                    interval2 = (int)((TimeSpan)e.Value).TotalSeconds;
                    break;
                case SetTime:
                    { 
                        var now = DateTime.Now;
                        var time = (TimeSpan)e.Value;
                        MeadowApp.Device.SetClock(new DateTime(now.Year, now.Month, now.Day, time.Hours, time.Minutes, time.Seconds));
                    }
                    break;
                case SetDate:
                    {
                        var now = DateTime.Now;
                        var date = (DateTime)e.Value;
                        MeadowApp.Device.SetClock(new DateTime(date.Year, date.Month, date.Day, now.Hour, now.Minute, now.Second));
                    }
                    break;
            }
        }
    }
}