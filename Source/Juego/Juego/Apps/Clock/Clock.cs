using Juego.Games;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Juego.Apps
{
    public partial class Clock : IGame
    {
        public enum ClockState
        {
            Clock,
            StopWatch,
            IntervalTimer,
        }

        enum StopWatchState
        {
            Stop,
            Start, 
            Pause
        }

        ClockState state = ClockState.Clock;
        StopWatchState swState = StopWatchState.Stop;

        DateTime stopWatchStart = DateTime.MinValue;

        List<long> splits = new List<long>();
        long stopwatchOffset;
        long lastSplit;

        int interval1 = 180;
        int interval2 = 60;
        int intervalCount = 1;

        public void Up()
        {
            if (swState == StopWatchState.Stop)
            {  
            }
            else if (swState == StopWatchState.Start)
            {   //split
                stopwatchOffset += DateTime.Now.Ticks - stopWatchStart.Ticks;
                stopWatchStart = DateTime.Now;
                splits.Add(stopwatchOffset - lastSplit);
                lastSplit = stopwatchOffset;
            }
            else //we're paused so stop
            {
                //reset 
                splits = new List<long>();
                stopwatchOffset = 0;
                lastSplit = 0;
                swState = StopWatchState.Stop;
            }
        }

        public void Down()
        {
            if(state == ClockState.StopWatch)
            {
                if(swState == StopWatchState.Stop)
                {
                    stopwatchOffset = 0;
                    lastSplit = 0;
                    stopWatchStart = DateTime.Now;
                    swState = StopWatchState.Start;
                }
                else if(swState == StopWatchState.Start)
                {   //pause
                    swState = StopWatchState.Pause;
                    stopwatchOffset += DateTime.Now.Ticks - stopWatchStart.Ticks;
                }
                else //we're paused so resume
                {
                    stopWatchStart = DateTime.Now;
                    swState = StopWatchState.Start;
                }
            }
        }

        public void Left()
        {

        }

        public void Reset()
        {
         
        }

        public void Right()
        {
            switch(state)
            {
                case ClockState.Clock:
                    state = ClockState.StopWatch;
                    break;
                case ClockState.StopWatch:
                    state = ClockState.IntervalTimer;
                    break;
                default:
                    state = ClockState.Clock;
                    break;
            }
        }
    }
}