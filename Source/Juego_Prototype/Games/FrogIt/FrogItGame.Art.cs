﻿using Meadow.Foundation.Graphics.Buffers;
using Meadow.Peripherals.Displays;

namespace Juego.Games
{
    public partial class FrogItGame
    {
        Buffer1bpp frogUp, frogRight, frogLeft;
        Buffer1bpp logDarkLeft, logDarkRight, logDarkCenter;
        Buffer1bpp crocLeft, crocCenter, crocRight;
        Buffer1bpp truckLeft, truckCenter, truckRight;
        Buffer1bpp carLeft, carRight;

        void InitBuffers()
        {
            //could really reduce some buffer churn here .....

            byte[] frogU = { 0x99, 0xbd, 0x5a, 0x7e, 0x7e, 0x3c, 0x66, 0xc3 };
            byte[] frogL = { 0xe3, 0x3a, 0x5e, 0xfc, 0xfc, 0x5e, 0x3a, 0xe3 };
            byte[] frogR = { 0xc7, 0x5c, 0x7a, 0x3f, 0x3f, 0x7a, 0x5c, 0xc7 };

            frogUp = (new Buffer1bpp(8, 8, frogU)).RotateAndConvert<Buffer1bpp>(RotationType._90Degrees);
            frogLeft = (new Buffer1bpp(8, 8, frogL)).RotateAndConvert<Buffer1bpp>(RotationType._90Degrees);
            frogRight = (new Buffer1bpp(8, 8, frogR)).RotateAndConvert<Buffer1bpp>(RotationType._90Degrees);

            // byte[] logL = { 0xc0, 0x99, 0x00, 0x30, 0x09, 0x20, 0x9c, 0xc0 }; //log left
            // byte[] logC = { 0x00, 0xc9, 0x00, 0x1a, 0xc1, 0x00, 0xd7, 0x00 };
            //  byte[] logR = { 0x03, 0xdd, 0x22, 0x2a, 0xaa, 0x22, 0x5d, 0x03 };

            byte[] logDarkL = { 0x3f, 0x66, 0xff, 0xcf, 0xf6, 0xdf, 0x63, 0x3f }; //log left
            byte[] logDarkC = { 0xff, 0x36, 0xff, 0xe5, 0x3e, 0xff, 0x28, 0xff };
            byte[] logDarkR = { 0xfc, 0x22, 0xdd, 0xd5, 0x55, 0xdd, 0xa2, 0xfc };

            logDarkLeft = (new Buffer1bpp(8, 8, logDarkL)).RotateAndConvert<Buffer1bpp>(RotationType._90Degrees);
            logDarkCenter = (new Buffer1bpp(8, 8, logDarkC)).RotateAndConvert<Buffer1bpp>(RotationType._90Degrees);
            logDarkRight = (new Buffer1bpp(8, 8, logDarkR)).RotateAndConvert<Buffer1bpp>(RotationType._90Degrees);

            byte[] crocL = { 0x3f, 0x88, 0xc2, 0xe0, 0xf0, 0xa0, 0x01, 0xff }; //log left
            byte[] crocC = { 0xff, 0x83, 0x00, 0x00, 0x00, 0x00, 0x9c, 0x31 };
            byte[] crocR = { 0xff, 0xff, 0xff, 0x0f, 0x07, 0x01, 0x70, 0xff };

            crocLeft = (new Buffer1bpp(8, 8, crocL)).RotateAndConvert<Buffer1bpp>(RotationType._90Degrees);
            crocCenter = (new Buffer1bpp(8, 8, crocC)).RotateAndConvert<Buffer1bpp>(RotationType._90Degrees);
            crocRight = (new Buffer1bpp(8, 8, crocR)).RotateAndConvert<Buffer1bpp>(RotationType._90Degrees);

            byte[] truckL = { 0x7f, 0x40, 0x5f, 0x40, 0x5f, 0x40, 0x7f, 0x00 }; //log left
            byte[] truckC = { 0xff, 0x00, 0xff, 0x00, 0xff, 0x00, 0xff, 0x00 };
            byte[] truckR = { 0xdc, 0x7e, 0x59, 0x59, 0x59, 0x7e, 0xdc, 0x00 };

            truckLeft = (new Buffer1bpp(8, 8, truckL)).RotateAndConvert<Buffer1bpp>(RotationType._90Degrees);
            truckCenter = (new Buffer1bpp(8, 8, truckC)).RotateAndConvert<Buffer1bpp>(RotationType._90Degrees);
            truckRight = (new Buffer1bpp(8, 8, truckR)).RotateAndConvert<Buffer1bpp>(RotationType._90Degrees);

            byte[] carL = { 0x1c, 0x3f, 0x4c, 0x4c, 0x4c, 0x3f, 0x1c, 0x00 };
            byte[] carR = { 0x1c, 0xfe, 0x71, 0x71, 0x71, 0xfe, 0x1c, 0x00 };

            carLeft = (new Buffer1bpp(8, 8, carL)).RotateAndConvert<Buffer1bpp>(RotationType._90Degrees);
            carRight = (new Buffer1bpp(8, 8, carR)).RotateAndConvert<Buffer1bpp>(RotationType._90Degrees);
        }

        // Bitmaps created by @senkunmusahi using https://www.riyas.org/2013/12/online-led-matrix-font-generator-with.html
        byte[,] bitmaps = {
           // Frogs [done]
           { 0x99, 0xbd, 0x5a, 0x7e, 0x7e, 0x3c, 0x66, 0xc3 },// {0x83, 0xDC, 0x7A, 0x3F, 0x3F, 0x7A, 0xDC, 0x83},
           { 0xe3, 0x3a, 0x5e, 0xfc, 0xfc, 0x5e, 0x3a, 0xe3 }, // {0x99, 0xBD, 0xDB, 0x7E, 0x7E, 0x3C, 0xE7, 0x81},
           { 0xe3, 0x3a, 0x5e, 0xfc, 0xfc, 0x5e, 0x3a, 0xe3 }, // {0x81, 0xE7, 0x3C, 0x7E, 0x7E, 0xDB, 0xBD, 0x99},

        // Bigger logs
            {0x3C, 0x7E, 0xD7, 0xB5, 0xAD, 0xBF, 0xFF, 0xED},
            {0xAD, 0xAD, 0xFF, 0xB7, 0xF5, 0xBF, 0xB7, 0xAD},
            {0xED, 0xBD, 0xC3, 0xBD, 0xA5, 0xBD, 0x42, 0x3C},  

        // Trucks
            {0x00, 0x7F, 0x41, 0x55, 0x55, 0x55, 0x55, 0x55},
            {0x55, 0x55, 0x55, 0x55, 0x55, 0x55, 0x55, 0x55},
            {0x41, 0x7F, 0x22, 0x7F, 0x7F, 0x63, 0x22, 0x1C},

        // Crocs
            {0x41, 0x63, 0x46, 0x6E, 0x7C, 0x7E, 0x7A, 0x3E},
            {0xBC, 0xFE, 0x7E, 0x3E, 0xBE, 0xBE, 0xFC, 0x7C},
            {0x78, 0x38, 0x38, 0x38, 0x70, 0x60, 0x60, 0x40},

        // Cars
            {0x00, 0x1C, 0x22, 0x63, 0x7F, 0x7F, 0x22, 0x22},
            {0x22, 0x3E, 0x3E, 0x7F, 0x63, 0x63, 0x22, 0x1C},
            {0x22, 0x3E, 0x3E, 0x7F, 0x63, 0x63, 0x22, 0x1C}
        };

        // Opening artwork created by @senkunmusahi using https://www.riyas.org/2013/12/online-led-matrix-font-generator-with.html
        readonly byte[] titleBmp = {
            0x00, 0x07, 0x00, 0xe0, 0x00, 0x00, 0x0d, 0x83, 0xb0, 0x00, 0x00, 0x18, 0xc3, 0x18, 0x00, 0x00,
            0x12, 0x66, 0xc8, 0x00, 0x00, 0x36, 0x7e, 0xcc, 0x00, 0x00, 0x34, 0x7f, 0x8c, 0x00, 0x00, 0x77,
            0x00, 0xee, 0x00, 0x00, 0xe0, 0x00, 0x07, 0x00, 0x01, 0xc0, 0x00, 0x03, 0x00, 0x01, 0x80, 0x00,
            0x01, 0x80, 0x01, 0x80, 0x00, 0x01, 0x80, 0x03, 0x80, 0x00, 0x01, 0xc0, 0x03, 0x08, 0x00, 0x20,
            0xc0, 0x03, 0x0e, 0x00, 0x60, 0xc0, 0x03, 0x87, 0x01, 0xc1, 0xc0, 0x01, 0x83, 0xff, 0x81, 0x80,
            0x01, 0x80, 0xfe, 0x01, 0x80, 0x78, 0xc0, 0x00, 0x03, 0x1e, 0xfc, 0xf0, 0x00, 0x0f, 0x3f, 0xde,
            0xff, 0xff, 0xff, 0xfb, 0xc7, 0xff, 0xff, 0xff, 0xe3, 0xc1, 0xe7, 0xff, 0xe7, 0x83, 0xc0, 0xe0,
            0x00, 0x07, 0x06, 0x60, 0x60, 0x00, 0x06, 0x06, 0x60, 0x30, 0x00, 0x0c, 0x06, 0x30, 0x30, 0x00,
            0x0c, 0x0c, 0x38, 0x18, 0x00, 0x18, 0x18, 0x1c, 0x0c, 0x00, 0x38, 0x38, 0x0e, 0x0c, 0x81, 0x30,
            0x70, 0x06, 0x0c, 0x83, 0x30, 0xe0, 0x03, 0x8c, 0xff, 0x31, 0xc0, 0x01, 0xcc, 0xff, 0x33, 0x80,
            0x33, 0xff, 0x81, 0xff, 0xcc, 0x7f, 0xff, 0x81, 0xff, 0xfe, 0xfc, 0x07, 0x81, 0xe0, 0x3e, 0x30,
            0x0f, 0xc3, 0xf0, 0x0c, 0x00, 0x3f, 0xe7, 0xf8, 0x00, 0x00, 0x7f, 0xe7, 0xfe, 0x00, 0x00, 0x67,
            0x36, 0xe6, 0x00, 0x00, 0x06, 0x36, 0x60, 0x00
        };
    }
}