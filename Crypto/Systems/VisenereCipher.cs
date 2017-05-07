using System;
using System.Linq;

namespace Crypto.Systems
{
	/// <summary>
	/// Represents the Visenere cipher.
	/// </summary>
	public class VisenereCipher : ICryptosystem<string>
	{
		/// <summary>
		/// Initializes a new instance of the VisenereCipher class.
		/// </summary>
		/// <param name="alphabet">The alphabet of this cipher.</param>
		/// <exception cref="ArgumentNullException">
		/// The alpahbet is <c>null</c>.
		/// </exception>
		public VisenereCipher(Alphabet alphabet)
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
		public string Encrypt(string plaintext, Key<string> key)
		{
			this.CheckParameters(plaintext, key);

			return this.Transform(plaintext, key, true);
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
		public string Decrypt(string ciphertext, Key<string> key)
		{
			this.CheckParameters(ciphertext, key);

			return this.Transform(ciphertext, key, false);
		}

		/// <summary>
		/// Performs the Visenere transformation.
		/// </summary>
		/// <param name="message">The message to transform</param>
		/// <param name="key">The key to use.</param>
		/// <param name="encrypt">
		/// <c>true</c>, if the transformation is encryption.
		/// <c>false</c>, if decryption.
		/// </param>
		/// <returns>The transformed message.</returns>
		private string Transform(string message, Key<string> key, bool encrypt)
			=> new String(message.Select((c, index) =>
				{
					if (!this.Alphabet.Contains(c))
					{
						return c;
					}

					index = (this.Alphabet.IndexOf(c) +
						(encrypt ? 1 : -1) * this.Alphabet.IndexOf(
							key.Value[index % key.Value.Length])) %
						this.Alphabet.Length;

					return this.Alphabet[
						index >= 0 ? index : index + this.Alphabet.Length];
				})
				.ToArray());
		
		/// <summary>
		/// Checks whether the parameters meet the cipher's requirements.
		/// </summary>
		/// <param name="message">The message to check.</param>
		/// <param name="key">The key to check.</param>
		private void CheckParameters(string message, Key<string> key)
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

			if (!this.Alphabet.Belongs(key.Value))
			{
				throw new ArgumentException(
					"The key cannot be used on this cipher's alphabet.",
					nameof(key));
			}
		}
	}
}
