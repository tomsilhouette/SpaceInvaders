using SkiaSharp;
using SkiaSharp.Views.Maui;
using SpaceInvaders.Levels;
using SpaceInvaders.ViewModel;
using SpaceInvaders.Weapons;
using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace SpaceInvaders.Views.Game;

public partial class GamePage : ContentPage
{
    Random random = new Random();
    private GameViewModel ViewModel { get; set; }
    public GameState State { get; set; }

    readonly int[] randomNumbers = new int[6];

    public GamePage(GameViewModel viewModel, GameState state)
	{
		InitializeComponent();

        BindingContext = ViewModel = viewModel;
        State = state;
    }

    protected override void OnAppearing()
    {
        ViewModel.TickEvent += (_, _) => canvasView?.InvalidateSurface();
        ViewModel.StartGame();

        for (int i = 0; i < randomNumbers.Length; i++)
        {
            randomNumbers[i] = (byte)random.Next(256);
        }
    }

/*    protected override void OnDisappearing()
    {
        // init
    }*/

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
        BackgroundGenerator bgGen = new ();
        bgGen.GenBg(State.CurrentLevel, canvas, start, end, randomNumbers);
    }

    // Player moves
    private void RightButton_Clicked(object sender, EventArgs e)
    {
        ViewModel.GoRight();
    }

    private void LeftButton_Clicked(object sender, EventArgs e)
    {
        ViewModel.GoLeft();
    }

    private void FireButton_Clicked(object sender, EventArgs e)
    {
        ViewModel.FireWeapon();
    }

}