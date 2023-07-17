using Microsoft.Extensions.Logging;
using Plugin.Maui.Audio;
using SkiaSharp.Views.Maui.Controls.Hosting;
using SpaceInvaders.ViewModel;
using SpaceInvaders.Views.Game;
using SpaceInvaders.Views.GameOver;
using SpaceInvaders.Views.HighScores;
using SpaceInvaders.Views.LevelComplete;
using SpaceInvaders.Views.Upgrades;

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
            });

		builder.Services.AddSingleton(AudioManager.Current);
		builder.Services.AddSingleton<GameState>();
		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<GamePage>();
		builder.Services.AddSingleton<LevelCompletePage>();
		builder.Services.AddSingleton<UpgradesPage>();
		builder.Services.AddSingleton<GameOverPage>();
		builder.Services.AddSingleton<GameViewModel>();
		builder.Services.AddSingleton<Player.Player>();
		// builder.Services.AddTransient<HighScoresPage>();
#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
