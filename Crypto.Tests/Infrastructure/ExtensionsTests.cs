using System.Collections;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Crypto.Infrastructure
{
	[TestClass]
	public class ExtensionsTests
	{
		[TestMethod]
		public void ToByteTest()
		{
			const byte input = 0b11001100;
			var array = new BitArray(new[] { input });

			Assert.AreEqual(input, array.ToByte());
		}
	}
}
