using System;
using osu.Framework.Lists;

namespace OsuFrameworkExamples.Lists {
	public static class LazyList {
		public static void Run() {
			/**
			 * osu.Framework.Lists.LazyList
			 *
			 * Applies a function on the element you
			 * index before returning the element to you
			 */

			ExamplesMain.PrintLongHorizontalLineSeparator();
			
			// I pride myself on being able to count
			var someNumbers = new[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};

			// Yes, I took primary school maths
			var squareFunction = new Func<int, int>(x => x * x);

			// Let's see this in action
			var lazyList = new LazyList<int, int>(someNumbers, squareFunction);
			Console.WriteLine("3 squared is " + lazyList[3]);

			// We can loop over it too
			foreach (var squaredNumber in lazyList) {
				Console.Write(squaredNumber + " ");
			}
			Console.WriteLine("");
			
			ExamplesMain.PrintLongHorizontalLineSeparator();
		}
	}
}