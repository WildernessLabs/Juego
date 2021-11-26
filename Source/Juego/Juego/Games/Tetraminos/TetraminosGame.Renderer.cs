using System;
using System.Threading;
using Meadow.Foundation.Graphics;

namespace Juego.Games
{
    public partial class TetraminosGame
    {
        int blockSize = 1;
        int topIndent;
        int leftIndent;

        public void Init(MicroGraphics graphics)
        {
            graphics.Clear();
            graphics.DrawText(0, 0, "Meadow Tetraminoes");
            graphics.DrawText(0, 10, "v0.2.0");
            graphics.Show();

            leftIndent = 3;

            if (graphics.Height > 128)
            {
                graphics.CurrentFont = new Font8x12();
            }
            else
            {
                graphics.CurrentFont = new Font4x8();
            }

            topIndent = graphics.CurrentFont.Height + 2;

            //little hacky but works out nicely for the low res displays
            blockSize = (graphics.Height - topIndent - 4) / 19;

            Thread.Sleep(1000);
        }

        public void Update(IIOConfig ioConfig)
        {
            var graphics = ioConfig.Graphics;

            Update();
            graphics.Clear();
            DrawGameField(graphics);
            DrawPreview(graphics);
            graphics.Show();

            Thread.Sleep(Math.Max(50 - Level, 0));
        }

        //ToDo - scale
        void DrawPreview(MicroGraphics graphics)
        {
            //draw next piece
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (IsPieceLocationSet(i, j, NextPiece))
                    {
                        graphics.DrawRectangle(i*2 + 54, j*2, 2, 2);
                    }
                }
            }
        }

        void DrawGameField(MicroGraphics graphics)
        {
            int xIndent = leftIndent + 2;
            int yIndent = topIndent + 2;

            graphics.DrawText(xIndent, 0, $"Lines: {LinesCleared}");

            graphics.DrawRectangle(leftIndent,
                topIndent,
                4 + blockSize * 9,
                4 + 19 * blockSize);

            //draw current piece
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (IsPieceLocationSet(i, j, CurrentPiece))
                    {
                        //  graphics.DrawPixel(i, j);
                        graphics.DrawRectangle((CurrentPiece.X + i) * blockSize + xIndent,
                            (CurrentPiece.Y + j) * blockSize + yIndent,
                            blockSize, blockSize, true, true);
                    }
                }
            }

            //draw gamefield
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    if (IsGameFieldSet(i, j))
                    {
                        graphics.DrawRectangle((i) * blockSize + xIndent,
                            (j) * blockSize + yIndent,
                            blockSize, blockSize, true, true);
                    }
                }
            }
        }
    }
}