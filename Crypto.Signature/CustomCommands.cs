using System.Windows.Input;

namespace Crypto.Signature
{
	public static class CustomCommands
	{
		public static RoutedUICommand Exit { get; }
		public static RoutedUICommand Encrypt { get; }
		public static RoutedUICommand Decrypt { get; }
		public static RoutedUICommand Sign { get; }
		public static RoutedUICommand Check { get; }

		static CustomCommands()
		{
			Exit = new RoutedUICommand(
				"Exit", nameof(Exit), typeof(CustomCommands));
			Exit.InputGestures.Add(new KeyGesture(Key.F4, ModifierKeys.Alt));

			Encrypt = new RoutedUICommand(
				"Encrypt", nameof(Encrypt), typeof(CustomCommands));
			Encrypt.InputGestures.Add(new KeyGesture(Key.E, ModifierKeys.Control));

			Decrypt = new RoutedUICommand(
				"Decrypt", nameof(Decrypt), typeof(CustomCommands));
			Decrypt.InputGestures.Add(new KeyGesture(Key.D, ModifierKeys.Control));

			Sign = new RoutedUICommand(
				"Sign", nameof(Sign), typeof(CustomCommands));
			Sign.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));

			Check = new RoutedUICommand(
				"Check", nameof(Check), typeof(CustomCommands));
			Check.InputGestures.Add(new KeyGesture(Key.C, ModifierKeys.Control));
		}
	}
}
