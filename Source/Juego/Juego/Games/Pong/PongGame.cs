using System;

namespace Juego.Games
{
    public partial class PongGame : IGame
    {
        int ballX;
        int ballY;
        int ballXSpeed;
        int ballYSpeed;

        int GameWidth;
        int GameHeight;

        int ballRadius;
        int paddleWidth;
        int paddleHeight;

        int playerX;
        int cpuX;

        int playerY;
        int cpuY;

        int playerScore;
        int cpuScore;

        int movementDistance;

        Random random;

        public PongGame()
        {
            random = new Random();
        }

        void PongSetup(int width, int height)
        {
            GameWidth = width;
            GameHeight = height;

            ballRadius = width / 64;
            paddleWidth = width / 64;
            paddleHeight = width / 8;

            playerX = width / 32;
            cpuX = GameWidth - paddleWidth - playerX;

            movementDistance = height / 8;
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

            ballXSpeed = 2 + random.Next() % 4;
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

            //player paddle collision
            ballX += ballXSpeed;
            if (ballY >= playerY &&
                ballY <= playerY + paddleHeight &&
                ballX - ballRadius <= playerX)
            {
                ballXSpeed *= -1;
                ballX += ballXSpeed;

                //dynamically adjust Y speed based on paddle position
                if(ballY <= playerY + 2)
                {
                    ballYSpeed--;
                }
                else if(ballY >= playerY + paddleHeight - 2)
                {
                    ballYSpeed++;
                }
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
            else if (ballX + ballRadius > GameWidth)
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
            else if (ballY + ballRadius > GameHeight)
            {
                ballYSpeed *= -1;
                ballY += ballYSpeed;
            }
        }

        public void Down()
        {
            if(playerY < GameHeight - paddleHeight)
            {
                playerY += movementDistance;
            }
        }

        public void Left()
        {
            Reset();
        }

        public void Right()
        {
            
        }

        public void Up()
        {
            if(playerY > movementDistance)
            {
                playerY -= movementDistance;
            }
        }
    }
}
