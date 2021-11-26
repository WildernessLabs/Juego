using System;

namespace Juego.Games
{
    public partial class LanderGame : IGame
    {
        enum LanderState
        {
            Forward,
            Left,
            Right,
            Dead
        }

        LanderState landerState;

        public double GameTime { get; private set; }
        public double TimeDelta => GameTime - lastTime;

        public double LanderX { get; set; }
        public double LanderY { get; private set; }

        public int Lives { get; private set; }

        public int CellSize { get; private set; }



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

        public LanderGame(int cellSize = 8, int width = 128)
        {

            Reset();
        }

        public void Reset()
        {
            gameStart = DateTime.Now;

            Lives = 3;

            lastInput = UserInput.None;
        }

        void ResetLander()
        {

        }

        void GenerateGame()
        {




        }

        double lastTime;
        public void Update()
        {
            lastTime = GameTime;
            GameTime = (DateTime.Now - gameStart).TotalSeconds;

            switch(lastInput)
            {
                case UserInput.Up:

                    break;
                case UserInput.Down:

                    break;
                case UserInput.Left:

                    break;
                case UserInput.Right:

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

        void KillLander()
        {
            landerState = LanderState.Dead;
            ResetLander();
        }
    }
}