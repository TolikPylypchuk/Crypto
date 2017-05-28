using System;

namespace Crypto.Keys
{
	public class KeyPartGeneratedEventArgs : EventArgs
	{
		public KeyPartGeneratedEventArgs(Key<int> key)
		{
			this.Key = key;
		}

		public Key<int> Key { get; }
	}
}
