using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Levels
{
    public class LevelSixBackground : ILevelBackground
    {
        public void Draw(SKCanvas canvas, SKPoint start, SKPoint end)
        {
            // create the first shader
            var colors = new SKColor[] {
                new SKColor(0, 255, 255),
                new SKColor(255, 0, 255),
                new SKColor(255, 255, 0),
                new SKColor(0, 255, 255)
            };
            var sweep = SKShader.CreateSweepGradient(new SKPoint(128, 128), colors, null);

            // create the second shader
            var turbulence = SKShader.CreatePerlinNoiseTurbulence(0.05f, 0.05f, 4, 0);

            // create the compose shader
            var shader = SKShader.CreateCompose(sweep, turbulence, SKBlendMode.SrcOver);

            // use the compose shader
            var paint = new SKPaint
            {
                Shader = shader
            };
            canvas.DrawPaint(paint);
        }
    }
}
