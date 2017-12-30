using System;
using System.IO;
using osu.Framework.Platform;

namespace OsuFrameworkExamples.Platform {
	public static class Storage {
		public static void Run() {
			/**
			 * osu.Framework.Platform.Storage
			 *
			 * Storage represents a folder where any game data can be stored
			 */
			
			ExamplesMain.PrintLongHorizontalLineSeparator();
			
			// Let's call our game BadRPGGame
			// Ahh, we have some save data we need to store somewhere
			// Note: DesktopStorage is currently the only concrete implementation of Storage
			var storage = new DesktopStorage("BadRPGGame");

			// Let's open up the folder to see what's going on
			storage.OpenInNativeExplorer();

			{
                // Let's create the file to save our data
                // The file is created at ./BadRPGGame/PlayerSaveData.txt
                using (Stream stream = storage.GetStream("PlayerSaveData.txt", FileAccess.Write, FileMode.Create))
                
                // Time to write our save data
                using (StreamWriter writer = new StreamWriter(stream)) {
                    writer.WriteLine("Health: 100");
                    writer.WriteLine("Position: 1234 6789");
                    writer.WriteLine("Peppy: The Creator");
                }
			}
			
			ExamplesMain.WaitToContinue();

			{
				// Let's read back the data from the file
				using (Stream stream = storage.GetStream("PlayerSaveData.txt"))
				using (StreamReader reader = new StreamReader(stream)) {
                    Console.Write(reader.ReadToEnd());
				}
			}
			
			// The file must also exist right?
			if (storage.Exists("PlayerSaveData.txt")) {
				Console.WriteLine("Yep the file exists!");
			}
			else {
				Console.WriteLine("Looks like we mucked up somewhere.");
			}
			
			ExamplesMain.WaitToContinue();

			// Let's create another storage inside our main storage to store level data
			var levelStorage = storage.GetStorageForDirectory("levels");

			{
				// Let's write some level data
                using (Stream stream = levelStorage.GetStream("level0.txt", FileAccess.Write, FileMode.Create))
                using (StreamWriter writer = new StreamWriter(stream)) {
	                // Please don't ever ask me to do level design
	                writer.WriteLine("0 0 0 0 0 0");
	                writer.WriteLine("0 1 1 1 1 0");
	                writer.WriteLine("0 1 0 0 1 0");
	                writer.WriteLine("0 1 0 0 1 0");
	                writer.WriteLine("0 1 1 1 1 0");
	                writer.WriteLine("0 0 0 0 0 0");
                }
			}
			
			// Go on, check the folder to see what new things there are
			ExamplesMain.WaitToContinue();
			
			// My design is so bad I just need to delete it
			levelStorage.Delete("level0.txt");
			
			// Let's clean up after ourselves
			storage.DeleteDirectory("levels");

			// There shouldn't be anymore folders here
			if (storage.GetDirectories(".").Length == 0) {
				Console.WriteLine("Yep, our janitor service is in working condition");
			}
			else {
				Console.WriteLine("Ehhh?! Who put this rubbish here?!");
			}
			
			ExamplesMain.WaitToContinue();
			
			// We were never here [yes, it's a bit of a hack]
			storage.DeleteDirectory("");
			
			ExamplesMain.PrintLongHorizontalLineSeparator();
		}
	}
}