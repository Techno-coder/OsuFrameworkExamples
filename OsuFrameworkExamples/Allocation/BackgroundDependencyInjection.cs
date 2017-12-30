using System;
using osu.Framework.Allocation;

namespace OsuFrameworkExamples.Allocation {
	public static class BackgroundDependencyInjection {
		public static void Run() {
			/**
			 * osu.Framework.Allocation.DependencyContainer
			 *
			 * This class stores objects
			 * Stored objects can be automatically passed as arguments
			 * to methods that have the attribute BackgroundDependencyLoader
			 * Stored objects are retrieved from a cache that you fill
			 * Implementation of the Dependency Injection pattern
			 */
			
			/**
			 * osu.Framework.Allocation.BackgroundDependencyLoader
			 *
			 * This attribute can be used on methods
			 * It indicates that this method should be called
			 * by a DependencyContainer
			 */
			
			ExamplesMain.PrintLongHorizontalLineSeparator();
			
			// Let's say we have an Renderer that's used in a lot of places
			// but we don't want to deal with passing the object around
			// We can use a DependencyContainer
			var container = new DependencyContainer();
			
			// The container doesn't know how to create a Renderer so
			// we'll give it a Renderer
			container.Cache(new Renderer(100));
			
			// Hey look a sprite ... but it needs to use a Renderer
			var sprite = new Sprite();
			
			// Inject calls the load(Renderer newRenderer) method
			// and uses its stored objects for the arguments
			// Note: Method name can be anything as long as it has
			// the attribute BackgroundDependencyLoader
			container.Inject(sprite);

			try {
				sprite.Draw();
			}
			catch (NullReferenceException) {
				Console.WriteLine("Looks like dependency injection didn't work ...");
			}
			
			ExamplesMain.PrintLongHorizontalLineSeparator();
			
			// DependencyContainers can be nested inside of each other
			var anotherContainer = new DependencyContainer(container);
			
			// This one could be a sprite of a unicorn
			var anotherSprite = new Sprite();
			
			// Notice how we haven't provided a renderer to this container
			// In this case, it will go to its parent container and
			// see if it has a Renderer
			anotherContainer.Inject(anotherSprite);
			
			try {
				sprite.Draw();
			}
			catch (NullReferenceException) {
				Console.WriteLine("But I just wanted a unicorn ...");
			}
			
			ExamplesMain.PrintLongHorizontalLineSeparator();
			
			// You can have as many cached objects as you want
			anotherContainer.Cache(new AudioPlayer());
			
			// And it works with methods that have multiple arguments too
			// Note: It won't work if you have multiple methods with
			// the attribute BackgroundDependencyLoader
			var player = new Player();
			anotherContainer.Inject(player);

			try {
				player.Dance();
			}
			catch (NullReferenceException) {
				Console.WriteLine("Who turned off the disco ball? ...");
			}
			
			ExamplesMain.PrintLongHorizontalLineSeparator();
		}
	}

	internal class Renderer {
		private readonly int renderQuality;

		public Renderer(int renderQuality) {
			this.renderQuality = renderQuality;
		}
		
		public void Render() {
			Console.WriteLine("Rendering some stuff with quality " + renderQuality);
		}
	}

	internal class Sprite {
		private Renderer renderer;

		[BackgroundDependencyLoader]
        // Note: Method name can be anything as long as it has
        // the attribute BackgroundDependencyLoader
		private void Load(Renderer newRenderer) {
			renderer = newRenderer;
		}

		public void Draw() {
			renderer.Render();
		}
	}

	internal class AudioPlayer {
		public void PlayAudio() {
			Console.WriteLine("Playing some techno style dance music ...");
		}
	}

	internal class Player {
		private Renderer renderer;
		private AudioPlayer audioPlayer;

		[BackgroundDependencyLoader]
		private void LoadDependencies(Renderer newRenderer, AudioPlayer newAudioPlayer) {
			renderer = newRenderer;
			audioPlayer = newAudioPlayer;
		}
		
		public void Dance() {
			renderer.Render();
			audioPlayer.PlayAudio();
			Console.WriteLine("Dance was successful! (but no one saw me)");
		}
	}
}