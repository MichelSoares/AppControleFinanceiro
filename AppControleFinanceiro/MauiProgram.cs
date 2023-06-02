using AppControleFinanceiro.Repositories;
using LiteDB;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using AppControleFinanceiro.Views;

namespace AppControleFinanceiro;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.RegisterDatabaseAndRepositories()
			.RegisterViews();
			
#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

    public static MauiAppBuilder RegisterDatabaseAndRepositories(this MauiAppBuilder mauiAppBuilder) 
	{
		mauiAppBuilder.Services.AddSingleton<LiteDatabase>(
				options =>
				{
					return new LiteDatabase($"Filename={AppSettings.DatabasePath};Connection=Shared");
				}
			);
		mauiAppBuilder.Services.AddTransient<ITransactionRepository, TransactionRepository>();
		return mauiAppBuilder;
	}

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddTransient<TransactionList>();
        mauiAppBuilder.Services.AddTransient<TransactionAdd>();
        mauiAppBuilder.Services.AddTransient<TransactionEdit>();
        return mauiAppBuilder;
    }
}
