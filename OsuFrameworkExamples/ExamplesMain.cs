using System;

namespace OsuFrameworkExamples {
	public static class ExamplesMain {
		private static void Main() {
			// Insert a Run method here to launch a code example
		}

		public static void PrintLongHorizontalLineSeparator() {
			Console.WriteLine("==========================================================");
		}

		public static void WaitToContinue() {
			Console.WriteLine("==== [ Press any key to continue executing the code ] ====");
			Console.ReadKey();
			Console.WriteLine("============= [ Continuing Execution ... ] ==============");
		}
	}
}