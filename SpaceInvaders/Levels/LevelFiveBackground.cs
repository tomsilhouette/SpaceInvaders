using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Levels
{
    public class LevelFiveBackground : ILevelBackground
    {
        public void Draw(SKCanvas canvas, SKPoint start, SKPoint end)
        {

            // create the shader
            var shader = SKShader.CreatePerlinNoiseTurbulence(0.05f, 0.05f, 4, 0);

            // use the shader
            var paint = new SKPaint
            {
                Shader = shader
            };
            canvas.DrawPaint(paint);
        }
    }
}
