using System;
using System.Threading;
using Meadow.Foundation.Graphics;

namespace Juego.Games
{
    public partial class TetraminosGame
    {
        int BLOCK_SIZE = 6;

        public void Init(GraphicsLibrary graphics)
        {
            graphics.CurrentFont = new Font4x8();
            graphics.Rotation = GraphicsLibrary.RotationType._270Degrees;

            graphics.Clear();
            graphics.DrawText(0, 0, "Meadow Tetraminoes");
            graphics.DrawText(0, 10, "v0.1.0");
            graphics.Show();

            Thread.Sleep(1000);
        }
        
        public void Update(GraphicsLibrary graphics)
        {
            Update();
            graphics.Clear();
            DrawGameField(graphics);
            DrawPreview(graphics);
            graphics.Show();

            Thread.Sleep(Math.Max(50 - Level, 0));
        }

        void DrawPreview(GraphicsLibrary graphics)
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

        void DrawGameField(GraphicsLibrary graphics)
        {
            int xIndent = 5;
            int yIndent = 12;

            graphics.DrawText(xIndent, 0, $"Lines: {LinesCleared}");

            graphics.DrawRectangle(3, 10, 58, 118);

            //draw current piece
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (IsPieceLocationSet(i, j, CurrentPiece))
                    {
                        //  graphics.DrawPixel(i, j);
                        graphics.DrawRectangle((CurrentPiece.X + i) * BLOCK_SIZE + xIndent,
                            (CurrentPiece.Y + j) * BLOCK_SIZE + yIndent,
                            BLOCK_SIZE, BLOCK_SIZE, true, true);
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
                        graphics.DrawRectangle((i) * BLOCK_SIZE + xIndent,
                            (j) * BLOCK_SIZE + yIndent,
                            BLOCK_SIZE, BLOCK_SIZE, true, true);
                    }
                }
            }
        }
    }
}