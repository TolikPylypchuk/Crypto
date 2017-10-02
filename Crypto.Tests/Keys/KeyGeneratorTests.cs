using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SimpleSubstitutionKey = Crypto.Key<(string From, string To)>;

namespace Crypto.Keys
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

		[TestMethod]
		public void GenerateRSAKeysTest()
		{
			const int p = 17;
			const int q = 11;
			const int fn = (p - 1) * (q - 1);

			var keys = KeyGenerator.GenerateRSAKeys(p, q);

			Assert.AreEqual(p * q, keys.EncKey.Value.N);
			Assert.AreEqual(p * q, keys.DecKey.Value.N);

			Assert.IsTrue(
				keys.EncKey.Value.Value * keys.DecKey.Value.Value % fn == 1);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void GenerateRSAKeysPInvalidTest()
		{
			KeyGenerator.GenerateRSAKeys(-1, 2);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void GenerateRSAKeysQInvalidTest()
		{
			KeyGenerator.GenerateRSAKeys(2, -2);
		}
	}
}
