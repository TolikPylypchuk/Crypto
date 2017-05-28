using System;

using Crypto.Infrastructure;

using SimpleSubstitutionKey = Crypto.Key<(string From, string To)>;
using RSAKey = Crypto.Key<(int N, int Value)>;

namespace Crypto.Keys
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

		/// <summary>
		/// Generates a pair of RSA keys.
		/// </summary>
		/// <param name="p">The first prime number.</param>
		/// <param name="q">The second prime number.</param>
		/// <returns>
		/// The encryption and decryption RSA keys.
		/// </returns>
		public static (RSAKey EncKey, RSAKey DecKey) GenerateRSAKeys(int p, int q)
		{
			if (p <= 0)
			{
				throw new ArgumentOutOfRangeException(
					nameof(p),
					"p must be greater than 0.");
			}

			if (q <= 0)
			{
				throw new ArgumentOutOfRangeException(
					nameof(q),
					"q must be greater than 0.");
			}

			int n = p * q;
			int fn = (p - 1) * (q - 1);

			var r = new Random();

			int e = 0;
			int d = 0;
			(int Value, int X, int Y) gcd;

			do
			{
				e = r.Next(1, fn);
				gcd = Algorithms.GCD(fn, e);
				d = gcd.Y > 0
					? gcd.Y
					: gcd.Y + fn;
			} while (gcd.Value != 1);

			return (new RSAKey((n, e)), new RSAKey((n, d)));
		}
	}
}
