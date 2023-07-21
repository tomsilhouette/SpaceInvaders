using Microsoft.Extensions.Logging;
using Plugin.Maui.Audio;
using SkiaSharp.Views.Maui.Controls.Hosting;
using SpaceInvaders.Levels;
using SpaceInvaders.Models;
using SpaceInvaders.ViewModel;
using SpaceInvaders.Views.Game;
using SpaceInvaders.Views.GameOver;
using SpaceInvaders.Views.HighScores;
using SpaceInvaders.Views.LevelComplete;
using SpaceInvaders.Views.Upgrades;
using SpaceInvaders.Weapons;

namespace SpaceInvaders;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseSkiaSharp()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("SegoeIcons.ttf", "Segoe Fluent Icons");
                fonts.AddFont("Lcd-Expanded.ttf", "Lcd Expanded");
                fonts.AddFont("Seven-Segment.ttf", "Seven Segment");
            });

		builder.Services.AddSingleton<Database>();
		builder.Services.AddSingleton(AudioManager.Current);
		builder.Services.AddSingleton<GameState>();
		builder.Services.AddSingleton<GameOverViewModel>();
		builder.Services.AddSingleton<Player.Player>();
		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<GamePage>();
		builder.Services.AddTransient<HighScoresPage>();
		builder.Services.AddSingleton<LevelCompletePage>();
		builder.Services.AddSingleton<UpgradesPage>();
		builder.Services.AddSingleton<UpgradesViewModel>();
		builder.Services.AddSingleton<GameOverPage>();
		builder.Services.AddSingleton<Laser>();
		builder.Services.AddSingleton<GameViewModel>();
		builder.Services.AddSingleton<HighScoresViewModel>();
		builder.Services.AddSingleton<MainPageViewModel>();
		builder.Services.AddSingleton<BackgroundGenerator>();
#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
