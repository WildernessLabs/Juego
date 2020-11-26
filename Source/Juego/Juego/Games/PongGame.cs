using System;

namespace Juego.Games
{
    public partial class PongGame : IGame
    {
        int ballX;
        int ballY;
        int ballXSpeed;
        int ballYSpeed;

        static int GameWidth = 128;
        static int GameHeight = 64;

        static int ballRadius = 3;
        static int paddleWidth = 3;
        static int paddleHeight = 14;

        static int playerX = 4;
        static int cpuX = GameWidth - paddleWidth - 4;

        int playerY;
        int cpuY;

        int playerScore;
        int cpuScore;

        Random random;

        public PongGame()
        {
            random = new Random();
        }

        public void Reset()
        {
            playerScore = 0;
            cpuScore = 0;

            cpuY = GameHeight / 2 - paddleHeight / 2;
            playerY = GameHeight / 2 - paddleHeight / 2;

            ResetBall();
        }

        void ResetBall()
        {
            ballX = GameWidth / 2;
            ballY = GameHeight / 2;

            ballXSpeed = 2 + random.Next() % 5;
            ballYSpeed = 1 + random.Next() % 3;
        }

        void UpdateCpuPlayer()
        {
            //very simple logic for now
            if (cpuY < ballY) { cpuY++; }
            else if (cpuY > ballY) { cpuY--; }
        }

        void Update()
        {
            UpdateCpuPlayer();

            ballX += ballXSpeed;
            if (ballY >= playerY &&
                ballY <= playerY + paddleHeight &&
                ballX - ballRadius <= playerX)
            {
                ballXSpeed *= -1;
                ballX += ballXSpeed;
            }
            else if (ballX - ballRadius < 0)
            {
                cpuScore++;
                ballXSpeed *= -1;

                ResetBall();
            }
            else if (ballY >= cpuY &&
                ballY <= cpuY + paddleHeight &&
                ballX + ballRadius > cpuX)
            {
                ballXSpeed *= -1;
                ballX += ballXSpeed;
            }
            else if (ballX + ballRadius > 128)
            {
                playerScore++;
                ballXSpeed *= -1;

                ResetBall();
            }

            ballY += ballYSpeed;
            if (ballY - ballRadius < 0)
            {
                ballYSpeed *= -1;
                ballY += ballYSpeed;
            }
            else if (ballY + ballRadius > 64)
            {
                ballYSpeed *= -1;
                ballY += ballYSpeed;
            }
        }

        public void Down()
        {
            if(playerY < 64 - paddleHeight)
            {
                playerY += 4;
            }
        }

        public void Left()
        {
            
        }

        public void Right()
        {
            
        }

        public void Up()
        {
            if(playerY > 4)
            {
                playerY -= 4;
            }
        }
    }
}
