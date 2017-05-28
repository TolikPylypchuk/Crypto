using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Crypto.Keys;
using Crypto.Systems;

namespace Crypto.Tests.Systems
{
	[TestClass]
	[SuppressMessage("ReSharper", "UnusedVariable")]
	public class CaesarCipherTests
	{
		[TestMethod]
		public void CreateTest()
		{
			var cipher = new CaesarCipher(Alphabets.English);
			Assert.AreEqual(Alphabets.English, cipher.Alphabet);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CreateWithNullTest()
		{
			var cipher = new CaesarCipher(null);
		}
		
		[TestMethod]
		public void EncryptTest()
		{
			var key = new Key<int>(3);

			const string plaintext = "aabcabc";

			string ciphertext = new String(
					plaintext.Select(ch => (char)(ch + key.Value)).ToArray());

			var cipher = new CaesarCipher(Alphabets.EnglishLower);

			Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, key));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void EncryptNullTest()
		{
			var cipher = new CaesarCipher(Alphabets.English);
			cipher.Encrypt(null, new Key<int>(3));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void EncryptWithNullTest()
		{
			var cipher = new CaesarCipher(Alphabets.English);
			cipher.Encrypt("aaa", null);
		}
		
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void EncryptWrongTextStrictTest()
		{
			const string plaintext = "aa,cabc";
			var cipher = new CaesarCipher(Alphabets.EnglishLower)
			{
				IsStrict = true
			};

			cipher.Encrypt(plaintext, new Key<int>(3));
		}

		[TestMethod]
		public void EncryptWrongTextTest()
		{
			const string plaintext = "aa,cabc";
			var cipher = new CaesarCipher(Alphabets.EnglishLower)
			{
				IsStrict = false
			};

			Assert.AreEqual(
				"dd,fdef", cipher.Encrypt(plaintext, new Key<int>(3)));
		}

		[TestMethod]
		public void DecryptTest()
		{
			var encryptionKey = new Key<int>(3);
			var decryptionKey = KeyGenerator.GetCaesarDecryptionKey(
				encryptionKey);

			const string plaintext = "aabcabc";

			string ciphertext = new String(
				plaintext.Select(
					ch => (char)(ch + encryptionKey.Value)).ToArray());

			var cipher = new CaesarCipher(Alphabets.EnglishLower);

			Assert.AreEqual(
				plaintext, cipher.Decrypt(ciphertext, decryptionKey));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void DecryptNullTest()
		{
			var cipher = new CaesarCipher(Alphabets.English);
			cipher.Decrypt(null, new Key<int>(-3));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void DecryptWithNullTest()
		{
			var cipher = new CaesarCipher(Alphabets.English);
			cipher.Decrypt("aaa", null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void DecryptWrongTextStrictTest()
		{
			const string ciphertext = "aa,cabc";
			var cipher = new CaesarCipher(Alphabets.EnglishLower)
			{
				IsStrict = true
			};

			cipher.Decrypt(ciphertext, new Key<int>(-3));
		}

		[TestMethod]
		public void DecryptWrongTextTest()
		{
			const string ciphertext = "dd,fdef";
			var cipher = new CaesarCipher(Alphabets.EnglishLower)
			{
				IsStrict = false
			};

			Assert.AreEqual(
				"aa,cabc",
				cipher.Decrypt(ciphertext, new Key<int>(-3)));
		}

		[TestMethod]
		public void AbsorbtionTest()
		{
			var encryptionKey = new Key<int>(3);
			var decryptionKey = KeyGenerator.GetCaesarDecryptionKey(encryptionKey);

			const string plaintext = "aabcabc";

			var cipher = new CaesarCipher(Alphabets.English);

			string ciphertext = cipher.Encrypt(plaintext, encryptionKey);

			Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, decryptionKey));
		}
	}
}
