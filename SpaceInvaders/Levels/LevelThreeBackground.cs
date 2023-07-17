using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Levels
{
    public class LevelThreeBackground : ILevelBackground
    {
        public void Draw(SKCanvas canvas, SKPoint start, SKPoint end)
        {
            // create the shader
            var colors = new SKColor[] {
                new SKColor(0, 255, 255),
                new SKColor(255, 0, 255),
                new SKColor(255, 255, 0),
                new SKColor(0, 255, 255)
            };
            var shader = SKShader.CreateSweepGradient(
                new SKPoint(128, 128),
                colors,
                null);

            // use the shader
            var paint = new SKPaint
            {
                Shader = shader
            };

            canvas.DrawPaint(paint);
        }
    }
}
