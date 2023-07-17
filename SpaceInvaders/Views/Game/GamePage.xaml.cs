using Microsoft.Maui.Graphics;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SpaceInvaders.Levels;
using SpaceInvaders.ViewModel;
using SpaceInvaders.Weapons;
using System.Diagnostics;

namespace SpaceInvaders.Views.Game;

public partial class GamePage : ContentPage
{
    private GameViewModel ViewModel { get; set; }

    private List<ILevelBackground> LevelBackgrounds { get; } = new();

    public GamePage(GameViewModel viewModel)
	{
		InitializeComponent();
		Title = "Space Invaders From Space 1.1.0";

        BindingContext = ViewModel = viewModel;

        LevelBackgrounds.Add(new LevelOneBackground());
        LevelBackgrounds.Add(new LevelTwoBackground());
        LevelBackgrounds.Add(new LevelThreeBackground());
        LevelBackgrounds.Add(new LevelFourBackground());
        LevelBackgrounds.Add(new LevelFiveBackground());
        LevelBackgrounds.Add(new LevelSixBackground());
    }

    protected override void OnAppearing()
    {
        ViewModel.TickEvent += (_, _) => canvasView?.InvalidateSurface();
        ViewModel.StartGame();
    }

    protected override void OnDisappearing()
    {
        ViewModel.StopGame();
    }

    // Init canvas
    private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        SKSurface surface = e.Surface;
        var canvas = surface.Canvas; // Assign the canvas to the property
        canvasView.IgnorePixelScaling = true;

        canvas.Clear(SKColors.White);
        DrawBackground(canvas, new SKPoint((float)canvasView.Width / 2.0f, 128), new SKPoint((float)canvasView.Width / 2.0f, 16));

        ViewModel.DrawGame(canvas);
    }

    // Draw background
    private void DrawBackground(SKCanvas canvas, SKPoint start, SKPoint end)
    {
        if (LevelBackgrounds.ElementAtOrDefault(ViewModel.State.currentLevel) is ILevelBackground background)
        {
            background.Draw(canvas, start, end);
        }
    }

    // Player moves
    private void RightButton_Clicked(object sender, EventArgs e)
    {
        ViewModel.player.playerXcord += 100;
    }

    private void LeftButton_Clicked(object sender, EventArgs e)
    {
        ViewModel.player.playerXcord -= 100;
    }

    private void FireButton_Clicked(object sender, EventArgs e)
    {
        ViewModel.BoltsFired.Add(new Bolt(ViewModel.player.playerXcord + 50, ViewModel.player.playerYcord));
    }
}