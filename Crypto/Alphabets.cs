using System.Diagnostics.CodeAnalysis;

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
			EnglishUpper = new Alphabet("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
			EnglishLower = new Alphabet("abcdefghijklmnopqrstuvwxyz");
			English = EnglishUpper + EnglishLower;

			UkrainianUpper = new Alphabet("АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ");
			UkrainianLower = new Alphabet("абвгґдеєжзиіїйклмнопрстуфхцчшщьюя");
			Ukrainian = UkrainianUpper + UkrainianLower;

			Punctuation = new Alphabet(" .,;:!?/\\'\"-()`");
			DecimalDigits = new Alphabet("0123456789");
			BinaryDigits = new Alphabet("01");
			SpecialCharacters = new Alphabet("[]{}<>~@#$%^&*-_+=");

			EnglishWithPunctuation = English + Punctuation;
			UkrainianWithPunctuation = Ukrainian + Punctuation;
		}

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
