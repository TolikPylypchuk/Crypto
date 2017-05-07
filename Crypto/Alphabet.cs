using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Crypto
{
	/// <summary>
	/// Represents an alphabet - a numbered set of unique characters.
	/// </summary>
	public class Alphabet : IEnumerable<char>, IEquatable<Alphabet>
	{
		/// <summary>
		/// The character to index dictionary.
		/// </summary>
		private Dictionary<int, char> characters = new Dictionary<int, char>();

		/// <summary>
		/// The index to character dictionary.
		/// </summary>
		private Dictionary<char, int> indices = new Dictionary<char, int>();

		/// <summary>
		/// Initializes a new instance of the Alphabet class.
		/// </summary>
		/// <param name="characters">The characters of this alphabet.</param>
		/// <exception cref="ArgumentNullException">
		/// The argument is <c>null</c>.
		/// </exception>
		public Alphabet(IEnumerable<char> characters)
		{
			characters = characters?.Distinct().ToArray()
				?? throw new ArgumentNullException(
					nameof(characters), "Cannot create empty alphabet.");

			if (!characters.Any())
			{
				throw new ArgumentException(
					"Cannot create empty alphabet.", nameof(characters));
			}

			int index = 0;
			foreach (char ch in characters)
			{
				this.characters.Add(index, ch);
				this.indices.Add(ch, index);
				index++;
			}

			this.Length = index;
		}

		/// <summary>
		/// Gets the number of characters in this alphabet.
		/// </summary>
		public int Length { get; }

		/// <summary>
		/// Gets the character with the specified index.
		/// </summary>
		/// <param name="index">The index of the character.</param>
		/// <returns>The character with the specified index.</returns>
		public char this[int index] => this.characters[index];

		/// <summary>
		/// Gets the index of the specified character.
		/// </summary>
		/// <param name="ch">The specified character.</param>
		/// <returns>The index of the specified character.</returns>
		public int this[char ch] => this.indices[ch];

		/// <summary>
		/// Creates a new alphabet which is a union of two alphabets.
		/// </summary>
		/// <param name="a1">The first alphabet.</param>
		/// <param name="a2">The second alphabet.</param>
		/// <returns>A new alphabet which is a union of two alphabets.</returns>
		/// <remarks>This operation is not commutative.</remarks>
		public static Alphabet Union(Alphabet a1, Alphabet a2)
			=> new Alphabet(a1.indices.Keys.Concat(a2.indices.Keys));

		/// <summary>
		/// Gets the character with the specified index.
		/// </summary>
		/// <param name="index">The index of the character.</param>
		/// <returns>The character with the specified index.</returns>
		public char CharAt(int index) => this[index];

		/// <summary>
		/// Gets the index of the specified character.
		/// </summary>
		/// <param name="ch">The character.</param>
		/// <returns>The index of the specified character.</returns>
		public int IndexOf(char ch) => this.indices[ch];

		/// <summary>
		/// Checks whether the text belongs to this alphabet.
		/// </summary>
		/// <param name="text">The text to check.</param>
		/// <returns>
		/// <c>true</c>, if the text belongs to this alphabet.
		/// Otherwise, <c>false</c>.
		/// </returns>
		public bool Belongs(string text)
		{
			var distinct = text.Distinct().OrderBy(c => c);

			return distinct.Intersect(this.indices.Keys)
						   .OrderBy(c => c)
						   .SequenceEqual(distinct);
		}

		/// <summary>
		/// Checks whether this alphabet contains the specified character.
		/// </summary>
		/// <param name="ch">The character to check.</param>
		/// <returns>
		/// <c>true</c> if this alphabet contains the specified character.
		/// Otherwise, <c>false</c>.
		/// </returns>
		public bool Contains(char ch)
			=> this.indices.ContainsKey(ch);

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
			var alphabet = other as Alphabet;
			return alphabet != null && this.Equals(alphabet);
		}

		/// <summary>
		/// Compates the equality of this alphabet to the other alphabet.
		/// </summary>
		/// <param name="other">The alphabet to compare to.</param>
		/// <returns>
		/// <c>true</c>, if this alphabet equals the other.
		/// Otherwise, <c>false</c>.
		/// </returns>
		public bool Equals(Alphabet other)
			=> other != null && this.indices.Keys.SequenceEqual(other.indices.Keys);

		/// <summary>
		/// Gets the hash code of this alphabet.
		/// </summary>
		/// <returns>The hash code of this alphabet.</returns>
		public override int GetHashCode() => this.ToString().GetHashCode();

		/// <summary>
		/// Gets the enumerator for this alphabet.
		/// </summary>
		/// <returns>The enumerator for this alphabet.</returns>
		public IEnumerator<char> GetEnumerator()
			=> this.indices.Keys.GetEnumerator();

		/// <summary>
		/// Returns a string that contains the characters of this alphabet.
		/// </summary>
		/// <returns>
		/// A string that contains the characters of this alphabet.
		/// </returns>
		public override string ToString()
			=> new String(this.indices.Keys.ToArray());

		/// <summary>
		/// Compates two alphabets for equality.
		/// </summary>
		/// <param name="a1">The first alphabet.</param>
		/// <param name="a2">The second alphabet.</param>
		/// <returns>
		/// <c>true</c>, if the first alphabet equals the second.
		/// Otherwise, <c>false</c>.
		/// </returns>
		public static bool operator ==(Alphabet a1, Alphabet a2)
			=> a1?.Equals(a2) ?? Equals(a2, null);

		/// <summary>
		/// Compates two alphabets for inequality.
		/// </summary>
		/// <param name="a1">The first alphabet.</param>
		/// <param name="a2">The second alphabet.</param>
		/// <returns>
		/// <c>true</c>, if the first alphabet does not equal the second.
		/// Otherwise, <c>false</c>.
		/// </returns>
		public static bool operator !=(Alphabet a1, Alphabet a2) => !(a1 == a2);

		/// <summary>
		/// Creates a new alphabet which is a union of two alphabets.
		/// </summary>
		/// <param name="a1">The first alphabet.</param>
		/// <param name="a2">The second alphabet.</param>
		/// <returns>A new alphabet which is a union of two alphabets.</returns>
		/// <remarks>This operation is not commutative.</remarks>
		public static Alphabet operator +(Alphabet a1, Alphabet a2)
			=> Union(a1, a2);

		/// <summary>
		/// Gets the enumerator for this alphabet.
		/// </summary>
		/// <returns>The enumerator for this alphabet.</returns>
		IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
	}
}
