using System;
using osu.Framework.Configuration;

namespace OsuFrameworkExamples.Configuration {
	public static class Bindable {
		public static void Run() {
			/**
			 * osu.Framework.Configuration.Bindable
			 * 
			 * The class Bindable wraps another object
			 * When that object is changed, delegates are invoked
			 * The class can be enabled or disabled to set whether the object can be changed
			 * This is a variant of the Observer pattern
			 * Bindables can be linked together so if the value changes, the other bindables are changed too
			 */
			
			ExamplesMain.PrintLongHorizontalLineSeparator();
			
			// Starting health is 100
			var healthMeter = new Bindable<int>(100);
			
			// Let's make 100 the default health as well
			healthMeter.Default = 100;

			// If the health changes we'll need to update the health GUI
			healthMeter.ValueChanged += UpdateHealthInterface;

			// Oof, we got hit by an enemy
			healthMeter.Value -= 50;

			// Let's freeze the health (because the player paused the game)
			healthMeter.Disabled = true;

			try {
				healthMeter.Value = 100;
			}
			catch (InvalidOperationException) {
				Console.WriteLine("Oops, looks like we can't change the value because it's disabled");
			}
			
			ExamplesMain.PrintLongHorizontalLineSeparator();

			// If the health gets enabled again we'll need to unpause the game
			healthMeter.DisabledChanged += ChangePauseState;
			healthMeter.Disabled = false;
			
			// You can add as many listeners as you want
			healthMeter.ValueChanged += SendHealthChangePacket;
			healthMeter.ValueChanged += UpdatePlayerDeathState;
			healthMeter.Value = -30;
			
			// And even remove listeners you no longer need (if you still have a reference)
			bool singlePlayer = true;
			if (singlePlayer) {
				healthMeter.ValueChanged -= SendHealthChangePacket;
			}
			
			// Let's respawn the player
			healthMeter.SetDefault();
			
			ExamplesMain.PrintLongHorizontalLineSeparator();
			
			// Need to trigger listeners without changing the value? No problem
			// Note: Triggers both ValueChanged and DisabledChanged listeners
			healthMeter.TriggerChange();
			
			// Because we're great programmers, let's split GUI handling from state handling
			healthMeter.ValueChanged -= UpdateHealthInterface;
			var guiHealthMeter = new Bindable<int>();
			guiHealthMeter.ValueChanged += UpdateHealthInterface;
			
			// We can link these together so if one changes, the other does too
			// Note: Binding triggers ValueChanged and DisabledChanged listeners
			// on the object that is doing the binding [ie. guiHealthMeter]
			guiHealthMeter.BindTo(healthMeter);
			healthMeter.Value -= 50;
			if (healthMeter.Value == guiHealthMeter.Value) {
				Console.WriteLine("Hey, they're the same value!");
			}
			
			ExamplesMain.PrintLongHorizontalLineSeparator();
			
			// Let's split up state handling as well
			healthMeter.DisabledChanged -= ChangePauseState;
			var healthPausedMeter = new Bindable<int>();
			healthPausedMeter.DisabledChanged += ChangePauseState;
			
			// You can link more than one bindable to the same bindable
			// Remember that binding triggers ValueChanged and DisabledChanged listeners
			healthPausedMeter.BindTo(healthMeter);
			healthMeter.Disabled = true;
			if (healthMeter.Disabled == healthPausedMeter.Disabled) {
				Console.WriteLine("I told you they were the same!");
			}
			
			// You can get rid of all the listeners
			healthMeter.UnbindEvents();
			
			// And all the bindings
			healthMeter.UnbindBindings();
			
			// Or both at the same time!
			healthMeter.UnbindAll();
			
			ExamplesMain.PrintLongHorizontalLineSeparator();
		}

		private static void UpdateHealthInterface(int newHealth) {
			Console.WriteLine("The health has been changed to " + newHealth);
		}

		private static void SendHealthChangePacket(int newHealth) {
			Console.WriteLine("HealthChangePacket sent with value " + newHealth);
		}

		private static void UpdatePlayerDeathState(int newHealth) {
			if (newHealth <= 0) {
				Console.WriteLine("Player has died");
			}
		}

		private static void ChangePauseState(bool isHealthDisabled) {
			if (isHealthDisabled) {
				Console.WriteLine("The game has been paused");
			}
			else {
				Console.WriteLine("The game is no longer paused");
			}
		}
	}
}