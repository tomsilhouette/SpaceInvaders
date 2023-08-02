using SkiaSharp;
using SkiaSharp.Views.Maui;
using SpaceInvaders.Levels;
using SpaceInvaders.ViewModel;
using System.Diagnostics;
using SkiaSharp.Views.Maui.Controls;

namespace SpaceInvaders.Views.Game;

public partial class GamePage : ContentPage
{
    Random random = new();
    private GameViewModel ViewModel { get; set; }
    public GameState State { get; set; }

    readonly int[] randomNumbers = new int[6];

    public bool SizesSet { get; set; } = false;

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
            if (i == 1)
            {
                randomNumbers[i] = (byte)random.Next(160);
            }
            else
            {
                randomNumbers[i] = (byte)random.Next(256);
            }
        }
    }

    protected override void OnDisappearing()
    {
        SizesSet = false;
    }
    // Init canvas
    private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        SKSurface surface = e.Surface;
        var canvas = surface.Canvas; // Assign the canvas to the property
        canvasView.IgnorePixelScaling = true;

        canvas.Clear();
        ViewModel.SetCanvas(canvas);
        
        // TODO
        if (!SizesSet)
        {
            ViewModel.SetDeviceDimensions();
            SizesSet = true;
        }
        
        DrawBackground(canvas, new SKPoint((float)canvasView.Width / 2.0f, 128), new SKPoint((float)canvasView.Width / 2.0f, 16)); 

        ViewModel.DrawGame();
    }

    // Draw background
    private void DrawBackground(SKCanvas canvas, SKPoint start, SKPoint end)
    {
        BackgroundGenerator bgGen = new();
        bgGen.GenBg(State.CurrentLevel, canvas, start, end, randomNumbers);
    }
}