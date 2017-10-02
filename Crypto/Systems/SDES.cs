using System;
using System.Collections;
using System.Linq;

using Crypto.Infrastructure;

namespace Crypto.Systems
{
	/// <summary>
	/// Represents the Simple DES encryption system.
	/// </summary>
	public class SDES : ICryptosystem<BitArray>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SDES" /> class.
		/// </summary>
		/// <param name="alphabet">The alphabet of this system.</param>
		/// <exception cref="ArgumentNullException">
		/// The alpahbet is <c>null</c>.
		/// </exception>
		public SDES(Alphabet alphabet)
		{
			this.Alphabet = alphabet
				?? throw new ArgumentNullException(
					nameof(alphabet),
					"The alphabet cannot be null.");

			if (this.Alphabet.Length != 256)
			{
				throw new ArgumentException(
					"The S-DES requires an alphabet of 256 characters",
					nameof(alphabet));
			}
		}

		/// <summary>
		/// Gets the alphabet this system operates on.
		/// </summary>
		public Alphabet Alphabet { get; }

		/// <summary>
		/// Gets or sets whether this system is strict
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
		/// This system is strict and the message does not belong
		/// to this system's alphabet or the key cannot be used for encryption.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// The message or the key is <c>null</c>.
		/// </exception>
		public string Encrypt(string plaintext, Key<BitArray> key)
			=> this.Transform(plaintext, key, true);

		/// <summary>
		/// Decrypts the message using the specified key.
		/// </summary>
		/// <param name="ciphertext">The message to encrypt.</param>
		/// <param name="key">The encryption key.</param>
		/// <returns>The ecrypted message.</returns>
		/// <exception cref="ArgumentException">
		/// This system is strict and the message does not belong
		/// to this system's alphabet or the key cannot be used for decryption.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// The message or the key is <c>null</c>.
		/// </exception>
		public string Decrypt(string ciphertext, Key<BitArray> key)
			=> this.Transform(ciphertext, key, false);

		/// <summary>
		/// Transforms the message using the specified key.
		/// </summary>
		/// <param name="message">The message to transform.</param>
		/// <param name="key">The encryption key.</param>
		/// <param name="encrypt">
		/// <c>true</c>, if the transformation is encryption,
		/// <c>false</c>, if decryption.</param>
		/// <returns>The transformed message.</returns>
		/// <exception cref="ArgumentException">
		/// This system is strict and the message does not belong
		/// to this system's alphabet or the key cannot be used for decryption.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// The message or the key is <c>null</c>.
		/// </exception>
		public string Transform(string message, Key<BitArray> key, bool encrypt)
		{
			this.ValidateParameters(message, key);

			return message
				.Select(ch => this.Alphabet.IndexOf(ch))
				.Select(index => this.EncodeByte((byte)index, key.Value, encrypt))
				.Select(output => this.Alphabet[output])
				.Aggregate(String.Empty, (acc, ch) => acc + ch);
		}

		/// <summary>
		/// Encodes the specified byte using the specified key.
		/// </summary>
		/// <param name="input">The byte to encode.</param>
		/// <param name="key">The key to use in the encoding.</param>
		/// <param name="encrypt">
		/// <c>true</c>, if the encoding is encryption,
		/// <c>false</c>, if decryption.</param>
		/// <returns>The encoded byte.</returns>
		private byte EncodeByte(byte input, BitArray key, bool encrypt)
		{
			var (cyclicKey1, cyclicKey2) = this.GenerateCycleSubkeys(key);
			
			return new BitArray(new[] { input })
				.Permute(1, 5, 2, 0, 3, 7, 4, 6)
				.CyclicTransform(encrypt ? cyclicKey1 : cyclicKey2)
				.Permute(4, 5, 6, 7, 0, 1, 2, 3)
				.CyclicTransform(encrypt ? cyclicKey2 : cyclicKey1)
				.Permute(3, 0, 2, 4, 6, 1, 7, 5)
				.ToByte();
		}
		
		/// <summary>
		/// Generates the cycle subkeys from the specified key.
		/// </summary>
		/// <param name="key">The specified key.</param>
		/// <returns>A pair of cycle subkeys.</returns>
		private (BitArray, BitArray) GenerateCycleSubkeys(BitArray key)
		{
			key = key.Permute(2, 4, 1, 6, 3, 9, 0, 8, 7, 5)
				     .Permute(1, 2, 3, 4, 0, 6, 7, 8, 9, 5);

			var result1 = key.Permute(5, 2, 6, 3, 7, 4, 9, 8);

			key = key.Permute(1, 2, 3, 4, 0, 6, 7, 8, 9, 5);

			var result2 = key.Permute(5, 2, 6, 3, 7, 4, 9, 8);
			
			return (result1, result2);
		}

		/// <summary>
		/// Checks whether the parameters meet the cipher's requirements.
		/// </summary>
		/// <param name="message">The message to validate</param>
		/// <param name="key">The key to validate.</param>
		/// <exception cref="ArgumentException">
		/// The system is strict and the message
		/// does not belong to the cipher's alphabet,
		/// or the key's value is <c>null</c>, or its length is not 10.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// The message or key is <c>null</c>.
		/// </exception>
		private void ValidateParameters(string message, Key<BitArray> key)
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

			if (key.Value == null)
			{
				throw new ArgumentException(
					"The key value cannot be null.",
					nameof(key));
			}

			if (key.Value.Length != 10)
			{
				throw new ArgumentException(
					"The key must contain exactly 10 bits.",
					nameof(key));
			}
		}
	}
}
