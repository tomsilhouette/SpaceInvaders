using SkiaSharp;

namespace SpaceInvaders.Levels
{
    public class LevelOneBackground : ILevelBackground
    {
        public void Draw(SKCanvas canvas, SKPoint start, SKPoint end)
        {
            var colors = new SKColor[] {
                    new SKColor(0, 0, 0),
                    new SKColor(0, 255, 0)
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
