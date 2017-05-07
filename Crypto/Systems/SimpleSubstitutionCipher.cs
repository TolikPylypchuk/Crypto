using System;
using System.Linq;

using SimpleSubstitutionKey = Crypto.Key<(string From, string To)>;

namespace Crypto.Systems
{
	/// <summary>
	/// Represents a simple substitution cipher.
	/// </summary>
	public class SimpleSubstitutionCipher
		: ICryptosystem<(string From, string To)>
	{
		/// <summary>
		/// Initializes a new instance of the SimpleSubstitutionCipher class.
		/// </summary>
		/// <param name="alphabet">The alphabet of this cipher.</param>
		/// <exception cref="ArgumentNullException">
		/// The alpahbet is <c>null</c>.
		/// </exception>
		public SimpleSubstitutionCipher(Alphabet alphabet)
		{
			this.Alphabet = alphabet ??
				throw new ArgumentNullException(
					nameof(alphabet),
					"The alphabet cannot be null.");
		}

		/// <summary>
		/// Gets the alphabet this cipher operates on.
		/// </summary>
		public Alphabet Alphabet { get; }

		/// <summary>
		/// Gets or sets whether this cipher is strict
		/// i.e. whether to throw an exception if the message
		/// doesn't belong to this cipher's alphabet.
		/// </summary>
		public bool IsStrict { get; set; }

		/// <summary>
		/// Encrypts the message using the specified key.
		/// </summary>
		/// <param name="plaintext">The message to encrypt.</param>
		/// <param name="key">The encryption key.</param>
		/// <returns>The ecrypted message.</returns>
		/// <exception cref="ArgumentException">
		/// The message does not belong to this cipher's alphabet
		/// or the key cannot be used for encryption.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// The message or the key is <c>null</c>.
		/// </exception>
		public string Encrypt(string plaintext, SimpleSubstitutionKey key)
		{
			this.CheckParameters(plaintext, key);

			return Transform(plaintext, key.Value.From, key.Value.To);
		}

		/// <summary>
		/// Decrypts the message using the specified key.
		/// </summary>
		/// <param name="ciphertext">The message to decrypt.</param>
		/// <param name="key">The decryption key.</param>
		/// <returns>The derypted message.</returns>
		/// <exception cref="ArgumentException">
		/// The message does not belong to this cipher's alphabet
		/// or the key cannot be used for encryption.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// The message or the key is <c>null</c>.
		/// </exception>
		public string Decrypt(string ciphertext, SimpleSubstitutionKey key)
			=> this.Encrypt(ciphertext, key);

		/// <summary>
		/// Performs the simple transformation.
		/// </summary>
		/// <param name="message">The message to transform</param>
		/// <param name="from">The source pattern of letters.</param>
		/// <param name="to">The destination pattern of letters.</param>
		/// <returns>The transformed message.</returns>
		private static string Transform(
			string message,
			string from,
			string to)
		{
			var fromAlphabet = new Alphabet(from);
			var toAlphabet = new Alphabet(to);
			
			return new String(message.Select(
				c => fromAlphabet.Contains(c)
					? toAlphabet.CharAt(fromAlphabet.IndexOf(c))
					: c).ToArray());
		}
		
		/// <summary>
		/// Checks whether the key can be used for encryption or decryption.
		/// </summary>
		/// <param name="key">The key to check.</param>
		/// <returns>
		/// <c>true</c>, if the key can be used for encryption or decryption.
		/// Otherwise, <c>false</c>.
		/// </returns>
		private bool IsKeyCorrect(SimpleSubstitutionKey key)
		{
			if (key.Value.From.Length != key.Value.To.Length)
			{
				return false;
			}

			var alphabet = this.Alphabet.OrderBy(c => c);

			return key.Value.From.OrderBy(c => c).SequenceEqual(alphabet) &&
			       key.Value.To.OrderBy(c => c).SequenceEqual(alphabet);
		}

		/// <summary>
		/// Checks whether the parameters meet the cipher's requirements.
		/// </summary>
		/// <param name="message">The message to check.</param>
		/// <param name="key">The key to check.</param>
		private void CheckParameters(string message, SimpleSubstitutionKey key)
		{
			if (message == null)
			{
				throw new ArgumentNullException(
					nameof(message),
					"The message cannot be null.");
			}

			if (key == null)
			{
				throw new ArgumentNullException(
					nameof(key),
					"The key cannot be null.");
			}

			if (this.IsStrict && !this.Alphabet.Belongs(message))
			{
				throw new ArgumentException(
					"The message does not belong to the cipher's alphabet.",
					nameof(message));
			}

			if (!this.IsKeyCorrect(key))
			{
				throw new ArgumentException(
					"The key cannot be used on this cipher's alphabet.",
					nameof(key));
			}
		}
	}
}
