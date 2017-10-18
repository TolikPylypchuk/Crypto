using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Crypto.Infrastructure;

using RSAKey = Crypto.Key<(int N, int Value)>;

namespace Crypto.Systems
{
	/// <summary>
	/// Represents the RSA cryptosystem.
	/// </summary>
	public class RSA : ICryptosystem<(int N, int Value)>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RSA" /> class.
		/// </summary>
		/// <param name="alphabet">The alphabet of this cipher.</param>
		/// <exception cref="ArgumentNullException">
		/// The alpahbet is <c>null</c>.
		/// </exception>
		public RSA(Alphabet alphabet)
		{
			this.Alphabet = alphabet
				?? throw new ArgumentNullException(
					nameof(alphabet),
					"The alphabet cannot be null.");
			this.IsStrict = true;
		}

		/// <summary>
		/// Gets the alphabet this system operates on.
		/// </summary>
		public Alphabet Alphabet { get; }

		/// <summary>
		/// Has no meaning as this system is always strict.
		/// </summary>
		[ExcludeFromCodeCoverage]
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
		public string Encrypt(string plaintext, RSAKey key)
		{
			this.ValidateParameters(plaintext, key, true);

			int alphabetLength = (this.Alphabet.Length - 1).NumberOfDigits();
			int keyLength = key.Value.N.NumberOfDigits();

			return plaintext
				.Select(this.Alphabet.IndexOf)
				.Select(num => num.ToString($"D{alphabetLength}"))
				.Aggregate(String.Empty, (acc, s) => acc + s)
				.SplitByLength(keyLength - 1)
				.Select(Int32.Parse)
				.Select(num => Algorithms.Modulo(num, key.Value.Value, key.Value.N))
				.Dump()
				.Select(num => num.ToString($"D{alphabetLength}"))
				.Aggregate(String.Empty, (acc, s) => acc + s)
				.Print()
				.SplitByLength(alphabetLength - 1)
				.Select(Int32.Parse)
				.Select(num => this.Alphabet[num])
				.Aggregate(String.Empty, (acc, ch) => acc + ch);
		}

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
		public string Decrypt(string ciphertext, RSAKey key)
		{
			this.ValidateParameters(ciphertext, key);

			int keyLength = key.Value.N.NumberOfDigits();
			int alphabetLength = (this.Alphabet.Length - 1).NumberOfDigits();
			
			return ciphertext
				.Select(this.Alphabet.IndexOf)
				.Select(num => num.ToString($"D{alphabetLength - 1}"))
				.Aggregate(String.Empty, (acc, s) => acc + s)
				.SplitByLength(keyLength)
				.Select(Int32.Parse)
				.Select(num => Algorithms.Modulo(num, key.Value.Value, key.Value.N))
				.Select(num => num.ToString($"D{alphabetLength - 1}"))
				.Aggregate(String.Empty, (acc, s) => acc + s)
				.Print()
				.SplitByLength(keyLength)
				.Select(Int32.Parse)
				.Select(num => this.Alphabet[num])
				.Aggregate(String.Empty, (acc, ch) => acc + ch);
		}
		
		/// <summary>
		/// Checks whether the parameters meet the system's requirements.
		/// </summary>
		/// <param name="message">The message to validate.</param>
		/// <param name="key">The key to validate.</param>
		/// <param name="encrypting">
		/// <c>true</c>, if encrypting.
		/// Otherwise, <c>false</c>.</param>
		/// <exception cref="ArgumentException">
		/// The system is strict and the message
		/// does not belong to the system's alphabet.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// The message or key is <c>null</c>.
		/// </exception>
		private void ValidateParameters(
			string message,
			Key<(int, int)> key,
			bool encrypting = false)
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

			if (!this.Alphabet.Belongs(message))
			{
				throw new ArgumentException(
					"The message does not belong to the system's alphabet.",
					nameof(message));
			}
		}
	}
}
