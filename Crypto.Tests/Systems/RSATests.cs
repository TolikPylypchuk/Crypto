using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using RSAKey = Crypto.Key<(int N, int Value)>;

namespace Crypto.Systems
{
	[TestClass]
	[SuppressMessage("ReSharper", "UnusedVariable")]
	public class RSATests
	{
		[TestMethod]
		public void CreateTest()
		{
			var system = new RSA(Alphabets.English);
			Assert.AreEqual(Alphabets.English, system.Alphabet);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CreateWithNullTest()
		{
			var system = new RSA(null);
		}

		[TestMethod]
		public void EncryptTest()
		{
			var system = new RSA(Alphabets.EnglishLower);
			
			var key = new RSAKey((33, 7));

			string ciphertext = system.Encrypt("text", key);

			Assert.AreEqual("abbfaabgcjajabbf", ciphertext);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void EncryptNullTest()
		{
			var key = new RSAKey((17, 11));

			var system = new RSA(new Alphabet("ab"));

			system.Encrypt(null, key);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void EncryptWithNullTest()
		{
			const string plaintext = "abbaba";

			var system = new RSA(new Alphabet("ab"));

			system.Encrypt(plaintext, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void EncryptWrongTextStrictTest()
		{
			var key = new RSAKey((17, 11));

			const string plaintext = "aacabc";

			var system = new RSA(new Alphabet("ab"))
			{
				IsStrict = true
			};

			system.Encrypt(plaintext, key);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void EncryptWrongTextTest()
		{
			var key = new RSAKey((17, 11));

			const string plaintext = "aacabc";

			var system = new RSA(new Alphabet("ab"))
			{
				IsStrict = false
			};

			system.Encrypt(plaintext, key);
		}

		[TestMethod]
		public void DecryptTest()
		{
			var system = new RSA(Alphabets.EnglishLower);

			var key = new RSAKey((33, 3));

			string plaintext = system.Decrypt("abbfaabgcjajabbf", key);

			Assert.AreEqual("text", plaintext);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void DecryptNullTest()
		{
			var key = new RSAKey((17, 11));

			var system = new RSA(new Alphabet("ab"));

			system.Decrypt(null, key);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void DecryptWithNullTest()
		{
			const string ciphertext = "aaabiebeg";

			var system = new RSA(new Alphabet("ab"));

			system.Decrypt(ciphertext, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void DecryptWrongTextStrictTest()
		{
			var key = new RSAKey((17, 11));

			const string ciphertext = "aaabiebeg";

			var system = new RSA(new Alphabet("ab"))
			{
				IsStrict = true
			};

			system.Decrypt(ciphertext, key);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void DecryptWrongTextTest()
		{
			var key = new RSAKey((17, 11));

			const string ciphertext = "aaabiebeg";

			var system = new RSA(new Alphabet("ab"))
			{
				IsStrict = false
			};

			system.Decrypt(ciphertext, key);
		}

		[TestMethod]
		public void AbsorbtionTest()
		{
			var encKey = new RSAKey((33, 7));
			var decKey = new RSAKey((33, 3));
			const string plaintext = "text";

			var system = new RSA(Alphabets.EnglishLower);

			Assert.AreEqual(
				plaintext,
				system.Decrypt(system.Encrypt(plaintext, encKey), decKey));
		}
	}
}
