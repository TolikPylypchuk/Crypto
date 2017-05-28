using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Crypto
{
	/// <summary>
	/// Contains extension methods for various classes.
	/// </summary>
	[ExcludeFromCodeCoverage]
	public static class Extensions
	{
		/// <summary>
		/// Splits the string into equal parts.
		/// </summary>
		/// <param name="str">The string to split.</param>
		/// <param name="maxLength">The length of each part.</param>
		/// <returns>The splitted string</returns>
		public static IEnumerable<string> SplitByLength(
			this string str,
			int maxLength)
		{
			for (int index = 0; index < str.Length; index += maxLength)
			{
				yield return str.Substring(
					index, Math.Min(maxLength, str.Length - index));
			}
		}

		/// <summary>
		/// Returns the number of digits of the specified number.
		/// </summary>
		/// <param name="n">The number.</param>
		/// <returns>The number of digits of n.</returns>
		public static int NumberOfDigits(this int n)
			=> (int)Math.Log10(n) + 1;
	}
}
