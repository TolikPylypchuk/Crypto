using System;

namespace Crypto
{
	/// <summary>
	/// Represents a generic key for encryption or decryption.
	/// </summary>
	/// <typeparam name="T">The type of the key.</typeparam>
	public class Key<T> : IEquatable<Key<T>>
	{
		/// <summary>
		/// Initializes a new instance of the Key class.
		/// </summary>
		/// <param name="value">The value of the key.</param>
		public Key(T value)
		{
			this.Value = value;
		}

		/// <summary>
		/// Gets the value of the key.
		/// </summary>
		public T Value { get; }

		/// <summary>
		/// Compates the equality of this object to the other object.
		/// </summary>
		/// <param name="other">The object to compare to.</param>
		/// <returns>
		/// <c>true</c>, if this object equals the other.
		/// Otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object other)
		{
			var key = other as Key<T>;
			return key != null && this.Equals(key);
		}

		/// <summary>
		/// Compates the equality of this key to the other key.
		/// </summary>
		/// <param name="other">The key to compare to.</param>
		/// <returns>
		/// <c>true</c>, if this key equals the other.
		/// Otherwise, <c>false</c>.
		/// </returns>
		public bool Equals(Key<T> other)
			=> other != null && this.Value.Equals(other.Value);

		/// <summary>
		/// Gets the hash code of this key.
		/// </summary>
		/// <returns>The hash code of this key.</returns>
		public override int GetHashCode() => this.Value.GetHashCode();

		/// <summary>
		/// Returns a string that represents the value of this key.
		/// </summary>
		/// <returns>A string that represents the value of this key.</returns>
		public override string ToString() => this.Value.ToString();

		/// <summary>
		/// Compates two keys for equality.
		/// </summary>
		/// <param name="k1">The first key.</param>
		/// <param name="k2">The second key.</param>
		/// <returns>
		/// <c>true</c>, if the first key equals the second.
		/// Otherwise, <c>false</c>.
		/// </returns>
		public static bool operator ==(Key<T> k1, Key<T> k2)
			=> k1?.Equals(k2) ?? Equals(k2, null);

		/// <summary>
		/// Compates two keys for inequality.
		/// </summary>
		/// <param name="k1">The first key.</param>
		/// <param name="k2">The second key.</param>
		/// <returns>
		/// <c>true</c>, if the first key does not equal the second.
		/// Otherwise, <c>false</c>.
		/// </returns>
		public static bool operator !=(Key<T> k1, Key<T> k2) => !(k1 == k2);
	}
}
