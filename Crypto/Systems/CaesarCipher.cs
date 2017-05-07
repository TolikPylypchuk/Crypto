using System;
using System.Linq;

namespace Crypto.Systems
{
	/// <summary>
	/// Represents the generalized version of the Casear's cipher.
	/// </summary>
	public class CaesarCipher: ICryptosystem<int>
	{
		/// <summary>
		/// Initializes a new instance of the CaesarCipher class.
		/// </summary>
		/// <param name="alpahbet">The alphabet of this cipher.</param>
		/// <exception cref="ArgumentNullException">
		/// The alpahbet is <c>null</c>.
		/// </exception>
		public CaesarCipher(Alphabet alpahbet)
		{
			this.Alphabet = alpahbet
				?? throw new ArgumentNullException(
					nameof(alpahbet),
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
		/// The message does not belong to this cipher's alphabet.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// The message or the key is <c>null</c>.
		/// </exception>
		public string Encrypt(string plaintext, Key<int> key)
		{
			this.CheckParameters(plaintext, key);

			return this.Shift(plaintext, key.Value);
		}

		/// <summary>
		/// Decrypts the message using the specified key.
		/// </summary>
		/// <param name="ciphertext">The message to decrypt.</param>
		/// <param name="key">The decryption key.</param>
		/// <returns>The derypted message.</returns>
		/// <exception cref="ArgumentException">
		/// The message does not belong to this cipher's alphabet.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// The message or the key is <c>null</c>.
		/// </exception>
		public string Decrypt(string ciphertext, Key<int> key)
			=> this.Encrypt(ciphertext, key);
		
		/// <summary>
		/// Shifts the letters in the message.
		/// </summary>
		/// <param name="message">The message to shift.</param>
		/// <param name="shift">The amount of letters to shift to.</param>
		/// <returns></returns>
		private string Shift(string message, int shift)
			=> new String(
				message.Select(
						ch => this.Alphabet.Contains(ch)
							? this.Alphabet[
								(this.Alphabet.IndexOf(ch) + shift) %
									this.Alphabet.Length]
							: ch)
					.ToArray());

		/// <summary>
		/// Checks whether the parameters meet the cipher's requirements.
		/// </summary>
		/// <param name="message">The message to check.</param>
		/// <param name="key">The key to check.</param>
		private void CheckParameters(string message, Key<int> key)
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
		}
	}
}
