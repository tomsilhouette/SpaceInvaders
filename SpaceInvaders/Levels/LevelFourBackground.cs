using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Levels
{
    public class LevelFourBackground : ILevelBackground
    {
        public void Draw(SKCanvas canvas, SKPoint start, SKPoint end)
        {
            var colors = new SKColor[] {
                    new SKColor(0, 0, 0),
                    new SKColor(255, 0, 0)
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
        }
    }
}
