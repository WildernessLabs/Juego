using System;

namespace Juego.Games
{
    public partial class FroggerGame : IGame
    {
        //each lane has a velocity (float)
        public float[] LaneSpeeds { get; private set; } = new float[6] { 1.0f, -2.0f, 1.5f, -1.0f, 1.5f, -2.0f };
        public byte[,] LaneData { get; private set; } = new byte[6, 32]
        {
            //no data for docks
            {1,2,3,0,1,2,3,0,0,0,1,2,3,0,1,3,0,0,0,0,1,2,3,0,0,0,0,1,2,3,0,0 },//logs
            {0,0,1,3,0,0,0,1,3,0,0,0,1,3,0,0,1,2,3,0,0,0,0,0,1,3,0,0,1,3,0,0 },//logs
            {1,2,3,0,1,2,3,0,0,0,1,2,3,0,1,2,3,0,0,0,1,2,2,3,0,0,0,1,2,3,0,0 },//logs
            {0,0,1,3,0,1,3,0,0,0,0,0,0,0,0,0,1,3,0,0,0,0,0,0,1,3,0,0,1,3,0,0 },//trucks
            {0,0,1,2,0,0,0,0,0,0,0,1,2,0,0,0,1,2,0,0,0,1,2,0,1,2,0,0,0,0,0,0 },//cars
            {1,2,3,0,0,0,0,0,0,0,0,1,2,3,0,0,0,0,0,1,2,3,0,0,0,0,0,1,2,3,0,0 },//trucks
            //no data for start lane
        };

        public double GameTime { get; private set; }
        public double TimeDelta => GameTime - lastTime;

        public byte LaneLength => 32;
        public byte Columns => 16;
        public byte Rows => 8;

        public double FrogX { get; set; }
        public double FrogY { get; private set; }

        public byte Lives { get; private set; }
        public byte FrogsHome { get; private set; }

        public byte CellSize { get; private set; }

        DateTime gameStart;
        UserInput lastInput;

        enum UserInput
        {
            None,
            Up,
            Down,
            Left,
            Right,
        }

        public FroggerGame(byte cellsize = 8)
        {
            CellSize = cellsize;
            Reset();
        }

        public void Reset()
        {
            gameStart = DateTime.Now;
            ResetFrog();
            Lives = 3;

            FrogsHome = 0;

            lastInput = UserInput.None;
        }

        void ResetFrog()
        {
            FrogX = (byte)(Columns * CellSize / 2);
            FrogY = (byte)((Rows - 1) * CellSize);
        }

        double lastTime;
        public void Update()
        {
            lastTime = GameTime;
            GameTime = (DateTime.Now - gameStart).TotalSeconds;

            switch(lastInput)
            {
                case UserInput.Up:
                    MoveFrogUp();
                    break;
                case UserInput.Down:
                    MoveFrogDown();
                    break;
                case UserInput.Left:
                    MoveFrogLeft();
                    break;
                case UserInput.Right:
                    MoveFrogRight();
                    break;
            }
            //clear for next frame
            lastInput = UserInput.None;
        }

        public void Up()
        {
            lastInput = UserInput.Up;
        }

        public void Down()
        {
            lastInput = UserInput.Down;
        }

        public void Left()
        {
            lastInput = UserInput.Left;
        }

        public void Right()
        {
            lastInput = UserInput.Right;
        }



        void MoveFrogUp()
        {
            if (FrogY >= CellSize) { FrogY -= CellSize; }

            if (FrogY == 0)
            {
                FrogsHome++;
                if (FrogsHome >= 5) { Reset(); }
                else { ResetFrog(); }
            }
        }

        void MoveFrogDown()
        {
            if (FrogY < Rows * CellSize - CellSize) { FrogY += CellSize; }
        }

        void MoveFrogLeft()
        {
            if (FrogX > CellSize) { FrogX -= CellSize; }
        }

        void MoveFrogRight()
        {
            if (FrogX <= Columns * CellSize - CellSize) { FrogX += CellSize; }
        }

        void KillFrog()
        {
            ResetFrog();
        }
    }
}