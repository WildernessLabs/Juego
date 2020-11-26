using System;
using Meadow.Foundation.Graphics;

namespace Juego.Games
{
    public partial class FroggerGame
    {
        readonly byte cellSize = 8;

        public void Init(GraphicsLibrary gl)
        {

        }

        public void Update(GraphicsLibrary gl)
        {
            Update();

            gl.Clear();
            DrawBackground(gl);
            DrawLanesAndCheckCollisions(gl);
            DrawFrog(gl);
            // DrawLives();
            gl.Show();
        }

        void DrawBackground(GraphicsLibrary graphics)
        {
            //draw docks
            for (int i = 0; i < 5; i++)
            {
                graphics.DrawRectangle(10 + 24 * i, 0, 12, 8, true, false);

                if (i < FrogsHome)
                {
                    DrawFrog(12 + 24 * i, 0, 1, graphics);
                }
            }

            //draw water
            //graphics.DrawRectangle(0, cellSize, 128, cellSize * 3, true, true);
        }

        void DrawLanesAndCheckCollisions(GraphicsLibrary graphics)
        {
            int startPos, index, x, y;
            int cellOffset;

            for (byte row = 0; row < 6; row++)
            {
                startPos = (int)(GameTime * LaneSpeeds[row]) % LaneLength;
                cellOffset = (int)(8.0f * GameTime * LaneSpeeds[row]) % cellSize;

                if (startPos < 0)
                {
                    startPos = LaneLength - (Math.Abs(startPos) % 32);
                }

                y = cellSize * (row + 1);

                if (row < 3 && y == FrogY)
                {
                    FrogX -= (TimeDelta * LaneSpeeds[row] * 8f);
                }

                for (byte i = 0; i < Columns + 2; i++)
                {
                    index = LaneData[row, (startPos + i) % LaneLength];

                    x = (i - 1) * cellSize - cellOffset;

                    if (index == 0)
                    {
                        if (row < 3)
                        {
                            if (IsFrogCollision(x, y) == true)
                            {
                                KillFrog();
                            }
                        }
                        continue;
                    }

                    switch (row)
                    {
                        case 0:
                        case 1:
                        case 2:
                            DrawLog(x, y, index, graphics);
                            break;
                        case 3:
                        case 5:
                            DrawTruck(x, y, index, graphics);
                            if (IsFrogCollision(x, y)) { KillFrog(); }
                            break;
                        case 4:
                            DrawCar(x, y, index, graphics);
                            if (IsFrogCollision(x, y)) { KillFrog(); }
                            break;
                    }
                }
            }
        }

        bool IsFrogCollision(int x, int y)
        {
            if (y == FrogY &&
                x > FrogX &&
                x < FrogX + cellSize)
            {
                return true;
            }
            return false;
        }

        void DrawLives(GraphicsLibrary graphics)
        {
            for (int i = 1; i < Lives; i++)
            {
                DrawFrog(cellSize * (Columns - i), cellSize * (Rows - 1), 1, graphics);
            }
        }

        void DrawFrog(GraphicsLibrary graphics)
        {
            DrawFrog((int)FrogX, (int)FrogY, 1, graphics);
        }

        void DrawFrog(int x, int y, int frame, GraphicsLibrary graphics)
        {
            if (frame == 0)
            {
                DrawBitmap(x, y, 1, 8, frogLeft, graphics);
            }
            else if (frame == 1)
            {
                DrawBitmap(x, y, 1, 8, frogUp, graphics);
            }
            else
            {
                DrawBitmap(x, y, 1, 8, frogRight, graphics);
            }
        }

        void DrawTruck(int x, int y, int index, GraphicsLibrary graphics)
        {
            if (index == 1) DrawBitmap(x, y, 1, 8, truckLeft, graphics);
            else if (index == 2) DrawBitmap(x, y, 1, 8, truckCenter, graphics);
            else if (index == 3) DrawBitmap(x, y, 1, 8, truckRight, graphics);
        }

        void DrawLog(int x, int y, int index, GraphicsLibrary graphics)
        {
            if (index == 1) DrawBitmap(x, y, 1, 8, logDarkLeft, graphics);
            else if (index == 2) DrawBitmap(x, y, 1, 8, logDarkCenter, graphics);
            else if (index == 3) DrawBitmap(x, y, 1, 8, logDarkRight, graphics);
        }

        void DrawCar(int x, int y, int index, GraphicsLibrary graphics)
        {
            if (index == 1) DrawBitmap(x, y, 1, 8, carLeft, graphics);
            else if (index == 2) DrawBitmap(x, y, 1, 8, carRight, graphics);
        }

        void DrawBitmap(int x, int y, int width, int height, byte[] bitmap, GraphicsLibrary graphics)
        {
            for (var ordinate = 0; ordinate < height; ordinate++) //y
            {
                for (var abscissa = 0; abscissa < width; abscissa++) //x
                {
                    var b = bitmap[(ordinate * width) + abscissa];
                    byte mask = 0x01;

                    for (var pixel = 0; pixel < 8; pixel++)
                    {
                        if ((b & mask) > 0)
                        {
                            graphics.DrawPixel(x + (8 * abscissa) + 7 - pixel, y + ordinate);
                        }
                        else
                        {
                            graphics.DrawPixel(x + (8 * abscissa) + 7 - pixel, y + ordinate, false);
                        }
                        mask <<= 1;
                    }
                }
            }
        }
    }
}