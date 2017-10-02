using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Crypto.Systems
{
	[TestClass]
	[SuppressMessage("ReSharper", "UnusedVariable")]
	public class VisenereCipherTests
	{
		[TestMethod]
		public void CreateTest()
		{
			var cipher = new VisenereCipher(Alphabets.English);

			Assert.AreEqual(Alphabets.English, cipher.Alphabet);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CreateWithNullTest()
		{
			var cipher = new VisenereCipher(null);
		}

		[TestMethod]
		public void EncryptTest()
		{
			const string plaintext = "CRYPTOISSHORTFORCRYPTOGRAPHY";
			const string ciphertext = "CSASTPKVSIQUTGQUCSASTPIUAQJB";

			var key = new Key<string>("ABCD");
			var alphabet = new Alphabet("ABCDEFGHIJKLMNOPQRSTUVWXYZ");

			var cipher = new VisenereCipher(alphabet);

			Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, key));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void EncryptNullTest()
		{
			var key = new Key<string>("ABCD");

			var cipher = new VisenereCipher(new Alphabet("ABCD"));

			cipher.Encrypt(null, key);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void EncryptWithNullTest()
		{
			const string plaintext = "abbaba";

			var cipher = new VisenereCipher(new Alphabet("ab"));

			cipher.Encrypt(plaintext, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void EncryptWrongTextStrictTest()
		{
			var key = new Key<string>("ab");

			const string plaintext = "aacabc";

			var cipher = new VisenereCipher(new Alphabet("ab"))
			{
				IsStrict = true
			};

			cipher.Encrypt(plaintext, key);
		}

		[TestMethod]
		public void EncryptWrongTextTest()
		{
			var key = new Key<string>("ab");

			const string plaintext = "aacabc";

			var cipher = new VisenereCipher(new Alphabet("ab"))
			{
				IsStrict = false
			};

			Assert.AreEqual("abcbbc", cipher.Encrypt(plaintext, key));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void EncryptWrongKeyTest()
		{
			var key = new Key<string>("abc");

			const string plaintext = "aabab";

			var cipher = new VisenereCipher(new Alphabet("ab"));

			cipher.Encrypt(plaintext, key);
		}

		[TestMethod]
		public void DecryptTest()
		{
			const string plaintext = "CRYPTOISSHORTFORCRYPTOGRAPHY";
			const string ciphertext = "CSASTPKVSIQUTGQUCSASTPIUAQJB";

			var key = new Key<string>("ABCD");
			var alphabet = new Alphabet("ABCDEFGHIJKLMNOPQRSTUVWXYZ");

			var cipher = new VisenereCipher(alphabet);

			Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, key));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void DecryptNullTest()
		{
			var key = new Key<string>("ABCD");

			var cipher = new VisenereCipher(new Alphabet("ABCD"));

			cipher.Decrypt(null, key);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void DecryptWithNullTest()
		{
			const string ciphertext = "abbaba";

			var cipher = new VisenereCipher(new Alphabet("ab"));

			cipher.Encrypt(ciphertext, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void DecryptWrongTextStrictTest()
		{
			var key = new Key<string>("ab");

			const string ciphertext = "aacabc";

			var cipher = new VisenereCipher(new Alphabet("ab"))
			{
				IsStrict = true
			};

			cipher.Decrypt(ciphertext, key);
		}

		[TestMethod]
		public void DecryptWrongTextTest()
		{
			var key = new Key<string>("ab");

			const string ciphertext = "aacabc";

			var cipher = new VisenereCipher(new Alphabet("ab"))
			{
				IsStrict = false
			};

			Assert.AreEqual("abcbbc", cipher.Decrypt(ciphertext, key));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void DecryptWrongKeyTest()
		{
			var key = new Key<string>("abc");

			const string ciphertext = "aabab";

			var cipher = new VisenereCipher(new Alphabet("ab"));

			cipher.Decrypt(ciphertext, key);
		}

		[TestMethod]
		public void AbsorbtionTest()
		{
			const string plaintext = "CRYPTOISSHORTFORCRYPTOGRAPHY";

			var key = new Key<string>("ABCD");
			var alphabet = new Alphabet("ABCDEFGHIJKLMNOPQRSTUVWXYZ");

			var cipher = new VisenereCipher(alphabet);

			Assert.AreEqual(
				plaintext,
				cipher.Decrypt(cipher.Encrypt(plaintext, key), key));
		}
	}
}
