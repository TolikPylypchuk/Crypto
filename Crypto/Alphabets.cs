using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Crypto
{
	/// <summary>
	/// Contains commonly used alphabets.
	/// </summary>
	[ExcludeFromCodeCoverage]
	public static class Alphabets
	{
		static Alphabets()
		{
			ASCII = new Alphabet(
				Enumerable.Range(0, 256)
					      .Select(i => (char)i)
					      .Aggregate(String.Empty, (acc, ch) => acc + ch))
			{
				Name = "ASCII"
			};

			EnglishUpper = new Alphabet("ABCDEFGHIJKLMNOPQRSTUVWXYZ")
			{
				Name = "English (uppercase)"
			};

			EnglishLower = new Alphabet("abcdefghijklmnopqrstuvwxyz")
			{
				Name = "English (lowercase)"
			};

			English = EnglishUpper + EnglishLower;
			English.Name = "English";

			UkrainianUpper = new Alphabet("АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ")
			{
				Name = "Ukrainian (uppercase)"
			};

			UkrainianLower = new Alphabet("абвгґдеєжзиіїйклмнопрстуфхцчшщьюя")
			{
				Name = "Ukrainian (lowercase)"
			};

			Ukrainian = UkrainianUpper + UkrainianLower;
			Ukrainian.Name = "Ukrainian";

			Punctuation = new Alphabet(" .,;:!?/\\'\"-()`")
			{
				Name = "Punctuation"
			};

			DecimalDigits = new Alphabet("0123456789")
			{
				Name = "Digits"
			};

			BinaryDigits = new Alphabet("01")
			{
				Name = "Bits"
			};

			SpecialCharacters = new Alphabet("[]{}<>~@#$%^&*-_+=")
			{
				Name = "Special characters"
			};

			EnglishWithPunctuation = English + Punctuation;
			UkrainianWithPunctuation = Ukrainian + Punctuation;
		}

		/// <summary>
		/// Gets the alphabet of ASCII characters.
		/// </summary>
		public static Alphabet ASCII { get; }

		/// <summary>
		/// Gets the lowercase letters of the English alphabet.
		/// </summary>
		public static Alphabet EnglishUpper { get; }

		/// <summary>
		/// Gets the uppercase letters of the English alphabet.
		/// </summary>
		public static Alphabet EnglishLower { get; }

		/// <summary>
		/// Gets the English alphabet.
		/// </summary>
		public static Alphabet English { get; }

		/// <summary>
		/// Gets the lowercase letters of the Ukrainian alphabet.
		/// </summary>
		public static Alphabet UkrainianUpper { get; }

		/// <summary>
		/// Gets the uppercase letters of the Ukrainian alphabet.
		/// </summary>
		public static Alphabet UkrainianLower { get; }

		/// <summary>
		/// Gets the Ukrainian alphabet.
		/// </summary>
		public static Alphabet Ukrainian { get; }

		/// <summary>
		/// Gets the alpabet that contains space and the punctuation characters.
		/// </summary>
		public static Alphabet Punctuation { get; }

		/// <summary>
		/// Gets the alpabet that contains digits 0-9.
		/// </summary>
		public static Alphabet DecimalDigits { get; }

		/// <summary>
		/// Gets the alpabet that contains digits 0 and 1.
		/// </summary>
		public static Alphabet BinaryDigits { get; }

		/// <summary>
		/// Gets the alpabet that contains the special characters.
		/// </summary>
		public static Alphabet SpecialCharacters { get; }

		/// <summary>
		/// Gets the English alphabet with punctuation characters.
		/// </summary>
		public static Alphabet EnglishWithPunctuation { get; }

		/// <summary>
		/// Gets the Ukrainian alphabet with punctuation characters.
		/// </summary>
		public static Alphabet UkrainianWithPunctuation { get; }
	}
}
