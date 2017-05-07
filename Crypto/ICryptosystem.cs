using System;

namespace Crypto
{
	/// <summary>
	/// Represents a generic cryptosystem.
	/// </summary>
	/// <typeparam name="T">The type of the key.</typeparam>
	public interface ICryptosystem<T>
		where T : IEquatable<T>
	{
		/// <summary>
		/// Gets the alphabet this cryptosystem operates on.
		/// </summary>
		Alphabet Alphabet { get; }

		/// <summary>
		/// Gets or sets whether this cryptosystem is strict
		/// i.e. whether to throw an exception if the message
		/// doesn't belong to this system's alphabet.
		/// </summary>
		bool IsStrict { get; set; }

		/// <summary>
		/// Encrypts the message using the specified key.
		/// </summary>
		/// <param name="plaintext">The message to encrypt.</param>
		/// <param name="key">The encryption key.</param>
		/// <returns>The ecrypted message.</returns>
		/// <exception cref="ArgumentException">
		/// The message does not belong to this system's alphabet.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// The message or the key is <c>null</c>.
		/// </exception>
		string Encrypt(string plaintext, Key<T> key);

		/// <summary>
		/// Decrypts the message using the specified key.
		/// </summary>
		/// <param name="ciphertext">The message to decrypt.</param>
		/// <param name="key">The decryption key.</param>
		/// <returns>The derypted message.</returns>
		/// <exception cref="ArgumentException">
		/// The message does not belong to this system's alphabet.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// The message or the key is <c>null</c>.
		/// </exception>
		string Decrypt(string ciphertext, Key<T> key);
	}
}
