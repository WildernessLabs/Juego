using System;
using Meadow.Foundation;
using Meadow.Foundation.Audio;
using Meadow.Foundation.Graphics;

namespace Juego.Games
{
    public partial class FrogItGame
    {
        readonly byte cellSize = 8;

        DrawPixelDel DrawPixel;

        public void Init(GraphicsLibrary gl)
        {
            gl.Clear();
            gl.DrawText(0, 0, "Meadow FrogIt");
            gl.DrawText(0, 16, "v0.2.3");
            gl.Show();

            //hacky scale for now
         //   if(gl.Width >= 160 && gl.Height >= 128)
        //    {
            //    DrawPixel = DrawPixel2x;
        //    }
          //  else
            {
                DrawPixel = DrawPixel1x;
            }
        }

        public void Update(IIOConfig ioConfig)
        {
            var gl = ioConfig.Graphics;

            Update();

            gl.Clear();
            DrawBackground(gl);
            DrawLanesAndCheckCollisions(gl);
            DrawFrog(gl, frogState);
            // DrawLives();
            gl.Show();
        }

        void DrawBackground(GraphicsLibrary graphics)
        {
            //draw docks
            for (int i = 0; i < 5; i++)
            {
              //  graphics.DrawRectangle(10 + 24 * i, 0, 12, 8, true, false);

                if (i < FrogsHome)
                {
                    DrawFrog(12 + 24 * i, 0, FrogState.Forward, graphics);
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
                DrawFrog(cellSize * (Columns - i), cellSize * (Rows - 1), FrogState.Forward, graphics);
            }
        }

        void DrawFrog(GraphicsLibrary graphics, FrogState state = FrogState.Forward)
        {
            DrawFrog((int)FrogX, (int)FrogY, state, graphics);
        }

        void DrawFrog(int x, int y, FrogState state, GraphicsLibrary graphics)
        {
            if (state == FrogState.Left)
            {
                DrawBitmap(x, y, 1, 8, frogLeft, graphics, Color.LawnGreen);
            }
            else if (state == FrogState.Forward)
            {
                DrawBitmap(x, y, 1, 8, frogUp, graphics, Color.LawnGreen);
            }
            else if(state == FrogState.Right)
            {
                DrawBitmap(x, y, 1, 8, frogRight, graphics, Color.LawnGreen);
            }
            else
            {
                graphics.DrawText(x, y, "X");
            }
        }

        void DrawTruck(int x, int y, int index, GraphicsLibrary graphics)
        {
            if (index == 1) DrawBitmap(x, y, 1, 8, truckLeft, graphics, Color.LightGray);
            else if (index == 2) DrawBitmap(x, y, 1, 8, truckCenter, graphics, Color.LightGray);
            else if (index == 3) DrawBitmap(x, y, 1, 8, truckRight, graphics, Color.LightGray);
        }

        void DrawLog(int x, int y, int index, GraphicsLibrary graphics)
        {
            if (index == 1) DrawBitmap(x, y, 1, 8, logDarkLeft, graphics, Color.Beige);
            else if (index == 2) DrawBitmap(x, y, 1, 8, logDarkCenter, graphics, Color.Beige);
            else if (index == 3) DrawBitmap(x, y, 1, 8, logDarkRight, graphics, Color.Beige);
        }

        void DrawCar(int x, int y, int index, GraphicsLibrary graphics)
        {
            if (index == 1) DrawBitmap(x, y, 1, 8, carLeft, graphics, Color.Red);
            else if (index == 2) DrawBitmap(x, y, 1, 8, carRight, graphics, Color.Red);
        }

        delegate void DrawPixelDel(int x, int y, bool colored, GraphicsLibrary graphics, Color color);

        void DrawPixel1x(int x, int y, bool colored, GraphicsLibrary graphics, Color color)
        {
            graphics.DrawPixel(x, y, colored?color:Color.Black);
        }

        void DrawPixel2x(int x, int y, bool colored, GraphicsLibrary graphics)
        {
            x *= 2;
            y *= 2;

            graphics.DrawPixel(x, y, colored);
            graphics.DrawPixel(x + 1, y, colored);
            graphics.DrawPixel(x, y + 1, colored);
            graphics.DrawPixel(x + 1, y + 1, colored);
        }

        void DrawBitmap(int x, int y, int width, int height, byte[] bitmap, GraphicsLibrary graphics, Color color)
        {
            for (var ordinate = 0; ordinate < height; ordinate++) //y
            {
                for (var abscissa = 0; abscissa < width; abscissa++) //x
                {
                    var b = bitmap[(ordinate * width) + abscissa];
                    byte mask = 0x01;

                    for (var pixel = 0; pixel < 8; pixel++)
                    {
                        DrawPixel(x + (8 * abscissa) + 7 - pixel, y + ordinate, (b & mask) > 0, graphics, color);

                        mask <<= 1;
                    }
                }
            }
        }
    }
}