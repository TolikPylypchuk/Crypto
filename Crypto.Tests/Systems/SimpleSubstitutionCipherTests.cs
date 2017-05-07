using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Crypto.Systems;

using SimpleSubstitutionKey = Crypto.Key<(string From, string To)>;

namespace Crypto.Tests.Systems
{
	[TestClass]
	[SuppressMessage("ReSharper", "UnusedVariable")]
	public class SimpleSubstitutionCipherTests
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
			var key = new SimpleSubstitutionKey(("ab", "ba"));
			const string plaintext = "abbaba";
			const string ciphertext = "baabab";

			var cipher = new SimpleSubstitutionCipher(new Alphabet("ab"));

			Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, key));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void EncryptNullTest()
		{
			var key = new SimpleSubstitutionKey(("ab", "ba"));

			var cipher = new SimpleSubstitutionCipher(new Alphabet("ab"));

			cipher.Encrypt(null, key);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void EncryptWithNullTest()
		{
			const string plaintext = "abbaba";

			var cipher = new SimpleSubstitutionCipher(new Alphabet("ab"));

			cipher.Encrypt(plaintext, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void EncryptWrongTextStrictTest()
		{
			var key = new SimpleSubstitutionKey(("ab", "ba"));

			const string plaintext = "aacabc";

			var cipher = new SimpleSubstitutionCipher(new Alphabet("ab"))
			{
				IsStrict = true
			};

			cipher.Encrypt(plaintext, key);
		}

		[TestMethod]
		public void EncryptWrongTextTest()
		{
			var key = new SimpleSubstitutionKey(("ab", "ba"));

			const string plaintext = "aacabc";

			var cipher = new SimpleSubstitutionCipher(new Alphabet("ab"))
			{
				IsStrict = false
			};

			Assert.AreEqual("bbcbac", cipher.Encrypt(plaintext, key));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void EncryptWrongKeyTest1()
		{
			var key = new SimpleSubstitutionKey(("ab", "ba"));

			const string plaintext = "aacabc";

			var cipher = new SimpleSubstitutionCipher(new Alphabet("abc"));

			cipher.Encrypt(plaintext, key);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void EncryptWrongKeyTest2()
		{
			var key = new SimpleSubstitutionKey(("abc", "ba"));

			const string plaintext = "aacabc";

			var cipher = new SimpleSubstitutionCipher(new Alphabet("abc"));

			cipher.Encrypt(plaintext, key);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void EncryptWrongKeyTest3()
		{
			var key = new SimpleSubstitutionKey(("abcd", "badc"));

			const string plaintext = "aacabc";

			var cipher = new SimpleSubstitutionCipher(new Alphabet("abc"));

			cipher.Encrypt(plaintext, key);
		}

		[TestMethod]
		public void DecryptTest()
		{
			var key = new SimpleSubstitutionKey(("ab", "ba"));

			const string plaintext = "abbaba";
			const string ciphertext = "baabab";

			var cipher = new SimpleSubstitutionCipher(new Alphabet("ab"));

			Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, key));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void DecryptNullTest()
		{
			var key = new SimpleSubstitutionKey(("ab", "ba"));

			var cipher = new SimpleSubstitutionCipher(new Alphabet("ab"));

			cipher.Decrypt(null, key);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void DecryptWithNullTest()
		{
			const string ciphertext = "abbaba";

			var cipher = new SimpleSubstitutionCipher(new Alphabet("ab"));

			cipher.Decrypt(ciphertext, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void DecryptWrongTextStrictTest()
		{
			var key = new SimpleSubstitutionKey(("ab", "ba"));

			const string ciphertext = "aacabc";

			var cipher = new SimpleSubstitutionCipher(new Alphabet("ab"))
			{
				IsStrict = true
			};

			cipher.Decrypt(ciphertext, key);
		}

		[TestMethod]
		public void DecryptWrongTextTest()
		{
			var key = new SimpleSubstitutionKey(("ab", "ba"));

			const string ciphertext = "aacabc";

			var cipher = new SimpleSubstitutionCipher(new Alphabet("ab"))
			{
				IsStrict = false
			};

			Assert.AreEqual("bbcbac", cipher.Decrypt(ciphertext, key));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void DecryptWrongKeyTest1()
		{
			var key = new SimpleSubstitutionKey(("ab", "ba"));

			const string ciphertext = "aacabc";

			var cipher = new SimpleSubstitutionCipher(new Alphabet("abc"));

			cipher.Decrypt(ciphertext, key);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void DecryptWrongKeyTest2()
		{
			var key = new SimpleSubstitutionKey(("abc", "ba"));

			const string ciphertext = "aacabc";

			var cipher = new SimpleSubstitutionCipher(new Alphabet("abc"));

			cipher.Decrypt(ciphertext, key);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void DecryptWrongKeyTest3()
		{
			var key = new SimpleSubstitutionKey(("abcd", "badc"));

			const string ciphertext = "aacabc";

			var cipher = new SimpleSubstitutionCipher(new Alphabet("abc"));

			cipher.Decrypt(ciphertext, key);
		}

		[TestMethod]
		public void AbsorbtionTest()
		{
			var encKey = new SimpleSubstitutionKey(("abc", "bca"));
			var decKey = KeyGenerator.GetSimpleSubstitutionDecryptionKey(encKey);
			const string plaintext = "abcbabca";

			var cipher = new SimpleSubstitutionCipher(new Alphabet("abc"));

			Assert.AreEqual(
				plaintext, 
				cipher.Decrypt(cipher.Encrypt(plaintext, encKey), decKey));
		}
	}
}
