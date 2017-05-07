using System;

using SimpleSubstitutionKey = Crypto.Key<(string From, string To)>;

namespace Crypto
{
	/// <summary>
	/// Contains helper methods for key generation.
	/// </summary>
	public static class KeyGenerator
	{
		/// <summary>
		/// Gets the decryption key for Caesar's cipher
		/// based on the specified encryption key.
		/// </summary>
		/// <param name="encryptionKey">The encryption key.</param>
		/// <returns>
		/// The decryption key for Caesar's cipher
		/// based on the specified encryption key.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// The encryption key is <c>null</c>.
		/// </exception>
		public static Key<int> GetCaesarDecryptionKey(Key<int> encryptionKey)
		{
			if (encryptionKey == null)
			{
				throw new ArgumentNullException(
					nameof(encryptionKey),
					"The encryption key cannot be null.");
			}

			return new Key<int>(-encryptionKey.Value);
		}

		/// <summary>
		/// Gets the decryption key for the simple substitution cipher
		/// based on the specified encryption key.
		/// </summary>
		/// <param name="encryptionKey">The encryption key.</param>
		/// <returns>
		/// The decryption key for the simple substitution cipher
		/// based on the specified encryption key.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// The encryption key is <c>null</c>.
		/// </exception>
		public static SimpleSubstitutionKey GetSimpleSubstitutionDecryptionKey(
			SimpleSubstitutionKey encryptionKey)
		{
			if (encryptionKey == null)
			{
				throw new ArgumentNullException(
					nameof(encryptionKey),
					"The encryption key cannot be null.");
			}

			return new SimpleSubstitutionKey(
				(encryptionKey.Value.To, encryptionKey.Value.From));
		}
	}
}
