using SkiaSharp;

namespace SpaceInvaders.Levels
{
    public class LevelTwoBackground : ILevelBackground
    { 
        public void Draw(SKCanvas canvas, SKPoint start, SKPoint end)
        {
            // create a gradient
            var colors = new SKColor[] {
                    new SKColor(52, 58, 235),
                    new SKColor(235, 235, 23)
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
