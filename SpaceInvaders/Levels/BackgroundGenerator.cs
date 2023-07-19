using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Levels
{
    public class BackgroundGenerator
    {
        public BackgroundGenerator() { 

        }
        public void GenBg(int levelNum, SKCanvas canvas, SKPoint start, SKPoint end, int[] randomNumbers)
        {
            Random random = new Random();
            if (levelNum % 2 == 0)
            {
                var colors = new SKColor[] {
                    new SKColor((byte)randomNumbers[0], (byte)randomNumbers[1], (byte)randomNumbers[2]),
                    new SKColor((byte)randomNumbers[3], (byte)randomNumbers[4], (byte)randomNumbers[5])
                    };

                var shader = SKShader.CreateTwoPointConicalGradient(
                    new SKPoint(start.X, start.Y),
                    128,
                    new SKPoint(end.X, end.Y),
                    16,
                    colors,
                    null,
                    SKShaderTileMode.Clamp);

                var paint = new SKPaint
                {
                    Shader = shader
                };

                canvas.DrawPaint(paint);
            } else
            {
                // create a gradient
                var colors = new SKColor[] {
                    new SKColor((byte)randomNumbers[0], (byte)randomNumbers[1], (byte)randomNumbers[2]),
                    new SKColor((byte)randomNumbers[3], (byte)randomNumbers[4], (byte)randomNumbers[5])
                };

                var shader = SKShader.CreateLinearGradient(
                    new SKPoint(start.X, start.Y),
                    new SKPoint(end.X, end.Y),
                    colors,
                    null,
                    SKShaderTileMode.Clamp);

                var paint = new SKPaint
                {
                    Shader = shader
                };

                canvas.DrawPaint(paint);
            }
        }
    }
}
