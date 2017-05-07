using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SimpleSubstitutionKey = Crypto.Key<(string From, string To)>;

namespace Crypto.Tests
{
	[TestClass]
	public class KeyGeneratorTests
	{
		[TestMethod]
		public void GetCaesarDecryptionKeyTest()
		{
			const int value = 42;

			Assert.AreEqual(
				new Key<int>(-value),
				KeyGenerator.GetCaesarDecryptionKey(new Key<int>(value)));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetCaesarDecryptionKeyNullTest()
		{
			KeyGenerator.GetCaesarDecryptionKey(null);
		}

		[TestMethod]
		public void GetSimpleSubstitutionDecryptionKeyTest()
		{
			const string from = "abc";
			const string to = "bca";

			Assert.AreEqual(
				new SimpleSubstitutionKey((from, to)),
				KeyGenerator.GetSimpleSubstitutionDecryptionKey(
					new SimpleSubstitutionKey((to, from))));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetSimpleSubstitutionDecryptionKeyNullTest()
		{
			KeyGenerator.GetSimpleSubstitutionDecryptionKey(null);
		}
	}
}
