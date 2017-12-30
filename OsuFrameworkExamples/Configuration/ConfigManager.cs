using System;
using osu.Framework.Configuration;
using osu.Framework.Platform;

namespace OsuFrameworkExamples.Configuration {
	public static class ConfigManager {
		public static void Run() {
			/**
			 * osu.Framework.Configuration.ConfigManager
			 *
			 * ConfigManager is a wrapper over Storage to store keys with values
			 * See Platform.Storage if you are unfamiliar with the class Storage
			 */
			
			ExamplesMain.PrintLongHorizontalLineSeparator();
			
			// We'll need to a place to store our game configuration
			var storageFolder = new DesktopStorage("BadRPGGame");
			
			// The type inside the angle brackets is the "key" type
			// The "key" type must be an enum but it can be any enum you want
			var config = new ConfigManager<WeirdKeys>(storageFolder);
			
			// Now let's set some configuration values!
			// Note: You MUST add null as the last parameter if your
			// value is of type Double, Float or Int otherwise Load()
			// will throw a parsing error
			config.Set(WeirdKeys.NumberOfTimesIHaveDied, 3041, null);
			config.Set(WeirdKeys.TechnoHasGirlfriend, false);
			config.Set(WeirdKeys.KillDeathRatioInMinecraft, 0.34, null);
			config.Set(WeirdKeys.TechnosOsuProfile, "https://osu.ppy.sh/users/10338558");
			
			// Don't forget to save the configuration file!
			config.Save();
			
			// Go check the file!
			storageFolder.OpenInNativeExplorer();
			
			ExamplesMain.WaitToContinue();
			
			// The Set method also returns a bindable that we can bind to
			// This allows the configuration to automatically change when
			// one of our bindables change.
			// See Configuration.Bindable if you are unfamilar with the class Bindable
			var deathCounter = new Bindable<int>(3041);
			config.Set(WeirdKeys.NumberOfTimesIHaveDied, 3041).BindTo(deathCounter);
			
			// You can achieve the same effect this way if you've already invoked the Set method
			config.BindWith(WeirdKeys.NumberOfTimesIHaveDied, deathCounter);
			
			// Let's see this in action!
			deathCounter.Value = 9001;
			Console.WriteLine("Configuration Death Count: " + config.Get<int>(WeirdKeys.NumberOfTimesIHaveDied));
			
			// We can load configuration values from a file too!
			config.Save();
			config.Load();
			
			// Just to make sure it works let's check
			if (config.Get<int>(WeirdKeys.NumberOfTimesIHaveDied) == 9001) {
				Console.WriteLine("Yep, the ConfigManager can save and load!");
			}
			else {
				Console.WriteLine("Hold up, we mucked up somewhere");
			}
			
			ExamplesMain.WaitToContinue();
			
			// By default the filename is game.ini
			// To change this you need to create your own class
			// and inherit ConfigManager
			// and override the Filename property
			var customConfigManager = new CustomConfigManager<WeirdKeys>(storageFolder);
			
			// Let's put in some dummy values
			customConfigManager.Set(WeirdKeys.TechnoHasGirlfriend, true);
			customConfigManager.Set(WeirdKeys.NumberOfTimesIHaveDied, -1);
			
			// Let's save it so it will appear on disk
			customConfigManager.Save();

			if (storageFolder.Exists("this_amazing_filename.ini")) {
				Console.WriteLine("Yep, this custom class doohickey works!");
			}
			else {
				Console.WriteLine("Nope.");
			}
			
			// You can check for yourself too!
			ExamplesMain.WaitToContinue();
			
			// Don't mind me; just doing some janitor buisness
			storageFolder.DeleteDirectory("../BadRPGGame");
			
			ExamplesMain.PrintLongHorizontalLineSeparator();
		}
	}

	internal class CustomConfigManager<T> : ConfigManager<T> where T: struct {
		protected override string Filename => @"this_amazing_filename.ini";

		public CustomConfigManager(Storage storage) : base(storage) {}
	}

	internal enum WeirdKeys {
		NumberOfTimesIHaveDied,
		TechnoHasGirlfriend,
		KillDeathRatioInMinecraft,
		TechnosOsuProfile
	}
}