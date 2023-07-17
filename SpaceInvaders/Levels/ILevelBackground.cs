using SkiaSharp;

namespace SpaceInvaders.Levels
{
    public interface ILevelBackground
    {
        public void Draw(SKCanvas canvas, SKPoint start, SKPoint end);
    }
}
